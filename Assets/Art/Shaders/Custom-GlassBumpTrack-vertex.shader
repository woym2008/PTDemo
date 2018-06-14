// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom-GlassBumped-Track" {
    Properties {
        _Color ("Main Color", Color) = (0.9852941,0.912846,0.912846,1)
        _MainTex ("Diffuse", 2D) = "white" {}
        // _MainTexPower ("Diffuse Power", Range(0, 1)) = 0.5213675
        _BumpMap ("Bump", 2D) = "bump" {}

        _Cubemap ("Refraction Cubemap", Cube) = "_Skybox" {}
        _ReflectRatio ("Reflect Ratio", Range(2.5, 10)) = 2.5

        // _Mask ("Mask", 2D) = "white" {}
        _Gloss ("Gloss", Range(0, 1)) = 1.0
        _Specular ("Specular", Range(0, 0.2)) = 0.05982906
		_specularPower("Specular Power", float) = 10
        // [MaterialToggle] _Emissive ("Emissive", Float ) = 0
        // _EmissiveColor01 ("Emissive Color 01", Color) = (0,0.3379312,1,1)
        // _EmissiveColor02 ("Emissive Color 02", Color) = (0,1,0.2965517,1)

        // [MaterialToggle] _Fresnel_ON ("Fresnel_ON", Float ) = 0
        _Fresnel ("Fresnel", Range(0, 10)) = 0.5947657
        _FresnelColor ("Fresnel  Color", Color) = (1,1,1,1)
        _FresnelThick ("Fresnel Thick", Float ) = 1.2
		_AlphaCutout ("Alpha Cutout", Range(0,1)) = 0.8
    }
    SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
		LOD 200

        Pass {
            Name "ForwardBase"
            // Tags {
            //     "LightMode"="ForwardBase"
            // }

            Cull Back         
            Fog {Mode Off}
			ZWrite Off
			Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl

            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform float _Gloss;
            uniform float _Specular;
            uniform samplerCUBE _Cubemap;
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _ReflectRatio;
            // uniform sampler2D _Mask; uniform float4 _Mask_ST;
            // uniform float4 _EmissiveColor01;
            // uniform float4 _EmissiveColor02;
            uniform float _Fresnel;
            // uniform float _MainTexPower;
            uniform fixed _FresnelThick;
            uniform fixed4 _FresnelColor;
            uniform fixed _Emissive;
            // uniform fixed _Fresnel_ON;
			uniform fixed _AlphaCutout;
			uniform float _specularPower;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;

				o.pos = UnityObjectToClipPos(v.vertex);
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
				// float3 viewDirection = normalize(ObjSpaceViewDir(i.posWorld).xyz);	//在平面中使用摄像机计算效果更好些
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);	
				
				float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
				// clip(_MainTex_var.a - _AlphaCutout);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
				
                float3 normalLocal = _BumpMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals                
                fixed nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;

                // 计算反射向量，通过反射向量采样反射贴图
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
				
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                // float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                // float node_146 = (pow(_Mask_var.b,0.1)*_Gloss);
                // float gloss = node_146;
                // float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                // float3 specularColor = mul( unity_WorldToObject, float4(saturate(( 0.09 > 0.5 ? (1.0-(1.0-2.0*(0.09-0.5))*(1.0-viewReflectDirection)) : (2.0*0.09*viewReflectDirection) )),0) ).xyz.rgb;
				float3 specularColor = mul( unity_WorldToObject, float4(saturate( 0.05 *viewReflectDirection) ,0) ).xyz.rgb;

                // float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),0);
				float3 directSpecular =  pow(max(0,dot(halfDirection,normalDirection)), _specularPower);
                float3 specular = directSpecular * specularColor ;

/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
				float4 _Cubemap_var = texCUBE(_Cubemap, float4(viewReflectDirection, (_Gloss*-6.0+6.0)));				
				indirectDiffuse += _Cubemap_var.rgb*_Cubemap_var.a*_ReflectRatio;	

                // float3 diffuse = (directDiffuse + indirectDiffuse) * ((lerp(0,0.5,pow(1.0-max(0,dot(i.normalDir, viewDirection)),_Fresnel))*_Color.rgb*_FresnelColor.rgb*_FresnelThick*_Fresnel_ON)+(_MainTex_var.rgb*(1.0 - _Specular)*2.0*_MainTexPower*_Color.rgb));
				float3 diffuse = _MainTex_var.rgb*(1.0 - _Specular)*2.0 *_Color.rgb;
				diffuse += (lerp(0,0.5,pow(1.0-max(0,dot(i.normalDir, viewDirection)),_Fresnel)) *_Color.rgb*_FresnelColor.rgb*_FresnelThick);
				diffuse *= (directDiffuse + indirectDiffuse);

////// Emissive:
                // float4 node_2002 = _Time + _TimeEditor;
                // float3 emissive = (_Mask_var.g*lerp(_EmissiveColor01.rgb,_EmissiveColor02.rgb,sin(node_2002.a))*_Emissive);
				// float3 emissive = (lerp(_EmissiveColor01.rgb,_EmissiveColor02.rgb,sin(node_2002.a))*_Emissive);
/// Final Color:
                // float3 finalColor = diffuse + specular + emissive;
				float3 finalColor = diffuse + specular;
                return fixed4(finalColor,1);
            }
            ENDCG
        }

    }

	SubShader {
    	Blend DstColor Zero
    	Pass {
        	Name "BASE"
        	SetTexture [_MainTex] { combine texture }
        }
    }

    // FallBack "Diffuse"
    // CustomEditor "ShaderForgeMaterialInspector"
}

