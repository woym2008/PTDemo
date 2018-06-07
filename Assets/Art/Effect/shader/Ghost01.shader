// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:5,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-7651-OUT;n:type:ShaderForge.SFN_Color,id:9938,x:31869,y:32924,ptovrint:False,ptlb:OutColor,ptin:_OutColor,varname:node_9938,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.7450981,c3:0.9490197,c4:1;n:type:ShaderForge.SFN_Color,id:5942,x:31869,y:32708,ptovrint:False,ptlb:InColor,ptin:_InColor,varname:node_5942,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Fresnel,id:2383,x:31869,y:33121,varname:node_2383,prsc:2|EXP-712-OUT;n:type:ShaderForge.SFN_Lerp,id:3743,x:32114,y:32808,varname:node_3743,prsc:2|A-5942-RGB,B-9938-RGB,T-2383-OUT;n:type:ShaderForge.SFN_ValueProperty,id:712,x:31578,y:33107,ptovrint:False,ptlb:Strength,ptin:_Strength,varname:node_712,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:7651,x:32319,y:32724,varname:node_7651,prsc:2|A-4964-OUT,B-3743-OUT,C-3845-RGB;n:type:ShaderForge.SFN_ValueProperty,id:4964,x:31869,y:32410,ptovrint:False,ptlb:Glow,ptin:_Glow,varname:node_4964,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:3845,x:31869,y:32509,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_3845,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;proporder:9938-5942-712-4964-3845;pass:END;sub:END;*/

Shader "Taecg/VFX/Ghost01" {
    Properties {
        _OutColor ("OutColor", Color) = (0,0.7450981,0.9490197,1)
        _InColor ("InColor", Color) = (1,0,0,1)
        _Strength ("Strength", Float ) = 1
        _Glow ("Glow", Float ) = 1
        _MainTex ("MainTex", 2D) = "white" {}
		_QOffset("Offset", Vector) = (0,0,0,0)
		_Dist("Distance", Float) = 100.0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            //Blend SrcAlpha OneMinusSrcAlpha
            //ZTest NotEqual
            ZWrite Off
			Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _OutColor;
            uniform float4 _InColor;
            uniform float _Strength;
            uniform float _Glow;

			float4 _QOffset;
			float _Dist;

            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
				float4 vPos = mul(UNITY_MATRIX_MV, v.vertex);
				//o.normal = mul(v.normal, (float3x3)unity_WorldToObject);
				float zOff = vPos.z / _Dist;
				vPos += _QOffset * zOff * zOff;
				

                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.pos = mul(UNITY_MATRIX_P, vPos);
                //o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float node_2383 = pow(1.0-max(0,dot(normalDirection, viewDirection)),_Strength);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 emissive = (_Glow*lerp(_InColor.rgb,_OutColor.rgb,node_2383)*_MainTex_var.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
