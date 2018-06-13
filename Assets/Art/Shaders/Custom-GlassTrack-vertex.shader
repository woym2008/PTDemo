// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// 环境反射和菲涅尔系数计算可以在定点着色器中计算(性能高)，


Shader "Custom-GlassTtrack_vertex"
{
	Properties {
		_Color ("Color", Color) = (0.1,0.2,0.4,1)
		_MainTex("BaseTex", 2D) = "white"{}
		_BumpMap("BumpTex", 2D) = "bunp"{}
		_RefractTex ("Refraction Texture", Cube) = "" {}
		_ReflectionStrength ("Reflection Strength", Range(0.0,2.0)) = 1.0
		_EnvironmentLight ("Environment Light", Range(0.0,2.0)) = 1.0
		_Emission ("Emission", Range(0.0,2.0)) = 0.0
		
		//菲涅尔系数
		// fresnelPower("fresnelPower",float)=1.0
        // fresnelScale("fresnelScale",float)=1.0
        // fresnelBias("fresnelBias",float)=0.3
	}
	SubShader {
		Tags {
			"Queue" = "Transparent"
		}
		LOD 200

		//  here we render the front faces of the diamonds.
		Pass {
			// ZWrite On
			ZWrite Off
			// Blend One One
			Blend One One
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;	float4 _MainTex_ST;
			sampler2D _BumpMap; float4 _BumpMap_ST;
			fixed4 _Color;
			samplerCUBE _RefractTex;
			half _ReflectionStrength;
			half _EnvironmentLight;
			half _Emission;

			half fresnelPower;
			half fresnelScale;
			half fresnelBias;
			
			// 红蓝绿的折射率
			float3 etaRatio = float3(0.83f,0.67f,0.55f); //1/1.3 1/1.5 1/1.8

			struct appdata_t{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv_main : TEXCOORD0;
				float2 uv_bump : TEXCOORD1;
				float3 uv_reflect : TEXCOORD2;
				// float4 objectPos : float;
				half fresnel : float;
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv_main = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv_bump = TRANSFORM_TEX(v.texcoord, _BumpMap);

				// TexGen CubeReflect:
				// reflect view direction along the normal, in view space.
				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				float3 reflection = -reflect(viewDir, v.normal);
				// 计算反射光照方向，以此作为反射贴图的uv
				o.uv_reflect = mul(unity_ObjectToWorld, float4(reflection,0));

				o.fresnel = 1.0 - saturate(dot(v.normal, viewDir));
				// o.fresnel = min(1.0f,max(0, fresnelBias+ fresnelScale*pow(1.0f+dot(normalize(-viewDir),v.normal),fresnelPower)));

				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			half4 frag (v2f i) : SV_Target
			{
				float2 bump = UnpackNormal(tex2D(_BumpMap, i.uv_bump));

				float4 main_var = tex2D(_MainTex, i.uv_main);
				main_var *= _Color;

				i.uv_reflect.xy += bump;
				

				// half3 refraction = texCUBE(_RefractTex, i.uv).rgb * _Color.rgb;
				half3 refraction = texCUBE(_RefractTex, i.uv_reflect).rgb ;

				// half4 reflection = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, i.uv);
				half4 reflection = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, i.uv_reflect);
				reflection.rgb = DecodeHDR (reflection, unity_SpecCube0_HDR);

				half3 reflection2 = reflection * _ReflectionStrength * i.fresnel;

				half3 multiplier = reflection.rgb * _EnvironmentLight + _Emission;
				
				fixed3 final = main_var.rgb + reflection2 + refraction.rgb * multiplier;
				return fixed4(final, 1.0f);
			}
			ENDCG
		}

		// Shadow casting & depth texture support -- so that gems can
        // cast shadows
        // UsePass "VertexLit/SHADOWCASTER"
	}

	SubShader {
    	Blend DstColor Zero
    	Pass {
        	Name "BASE"
        	SetTexture [_MainTex] { combine texture }
        }
    }

}
