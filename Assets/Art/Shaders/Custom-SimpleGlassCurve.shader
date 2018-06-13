// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// 玻璃材质shader的简单实现, 同时当距离摄像机越远越会有弯曲效果
// 1. 透明属性 Queue="Transparent" + Alpha blend融合
// 2. 添加漫反射(通过计算发射角度) + 环境光
// 3. 利用立方贴图，通过反射天空盒 模拟环境反射

Shader "Custom/Custom-SimpleGlass"
{
	Properties
	{
		_Color ("Base Color", Color) =(1,1,1,1)
		_Alpha ("Alpha Value", Range(0,0.5)) = 0.1
		_CubeMap("CubeMap", cube) = ""{}

		_QOffset("Offset", Vector) = (0,0,0,0)
		_UpOffset("UpOffset", Vector) = (0,0,0,0)
		_Brightness("Brightness", Float) = 0.0
		_Dist("Distance", Float) = 100.0

			_NearDist("_NearDist", Float) = 20.0
			_FarDist("_FarDist", Float) = 100.0
	}
	SubShader
	{
		LOD 200

		Tags{
			"Queue"="Transparent"
			"RenderType"="Transparent"
		}

		Cull Back //Cull Front	 ,Cull Off   // Cull off关闭剔除操作，采用双面渲染
		ZWrite Off 
		Blend SrcAlpha OneMinusSrcAlpha		// 透明混合实现透明效果

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			samplerCUBE _CubeMap;
			float4 _Color;
			float _Alpha;

			float4 _QOffset;
			float4 _UpOffset;
			float _Brightness;
			float _Dist;
			float _NearDist;
			float _FarDist;

			struct v2f{
				float4 pos :POSITION;
				float4 color :TEXCOORD0;
				float3 normal :TEXCOORD1;		// 法线
				float4 vertex : TEXCOORD2;		// 顶点
				float3 R :TEXCOORD3;
				float4 uv:TEXCOORD4;
				float fade : TEXCOORD5;
			};
			
			v2f vert(appdata_base v)
			{
				v2f o;
				float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
				float diffZ = pos.z / _Dist;
				pos += (_UpOffset*diffZ + _QOffset * diffZ * diffZ);
				o.pos = mul(UNITY_MATRIX_P, pos);
				// o.pos = UnityObjectToClipPos(v.vertex);

				// o.normal = v.normal;
				o.normal = mul(v.normal, (float3x3)unity_WorldToObject);

				o.vertex = v.vertex;
				o.uv = v.texcoord;

				float dis = length(pos.xyz);
				o.fade = 1 - saturate((dis - _NearDist) / (_FarDist - _NearDist));

				return o; 
			}

			float4 frag(v2f IN): COLOR{
				float4 col = UNITY_LIGHTMODEL_AMBIENT;
				float3 nor = UnityObjectToWorldNormal(IN.vertex);
				float3 lightDir = normalize(WorldSpaceLightDir(IN.vertex));

				// 计算漫发射的强度(反射强度)
				float diffuseScale = saturate(dot(nor, lightDir));
				col += (_LightColor0 * diffuseScale + _Color);		// 加上环境光

				float3 norW2O = normalize(mul(IN.normal, (float3x3)unity_WorldToObject));
				float3 viwDir = -1 * WorldSpaceViewDir(IN.vertex);
				col += texCUBE(_CubeMap, reflect(viwDir, norW2O));// 采样反射贴图

				// 高光部分
				// fixed3 worldNormal = normalize(IN.normal);
				// fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);
				// fixed3 NdotL = 0.5 * dot(worldNormal, worldLightDir) + 0.5;
				// col *= _Brightness * half4(NdotL,1.0);
				
				col[3] *= _Alpha * IN.fade;

				return col;
			}
			
			ENDCG
		}
	}

	Fallback "Diffuse"
}
