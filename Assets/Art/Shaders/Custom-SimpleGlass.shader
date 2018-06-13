// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// 玻璃材质shader的简单实现
// 1. 透明属性 Queue="Transparent" + Alpha blend融合
// 2. 添加漫反射(通过计算发射角度) + 环境光
// 3. 利用立方贴图，通过反射天空盒 模拟环境反射

Shader "Custom/Custom-SimpleGlassCurve"
{
	Properties
	{
		_Color ("Base Color", Color) =(1,1,1,1)
		_Alpha ("Alpha Value", Range(0,0.5)) = 0.1
		_CubeMap("CubeMap", cube) = ""{}


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
			

			struct v2f{
				float4 pos :POSITION;
				float4 color :TEXCOORD0;
				float3 normal :TEXCOORD1;		// 法线
				float4 vertex : TEXCOORD2;		// 顶点
				float3 R :TEXCOORD3;
			};
			
			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.vertex = v.vertex;
				
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
				
				col[3] *= _Alpha;

				return col;
			}
			
			ENDCG
		}
	}

	Fallback "Diffuse"
}
