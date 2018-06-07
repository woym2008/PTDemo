// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-3966-OUT,alpha-6807-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31873,y:32894,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_TexCoord,id:2670,x:31074,y:32614,varname:node_2670,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:3894,x:31266,y:32688,varname:node_3894,prsc:2,spu:0,spv:0.6|UVIN-2670-UVOUT;n:type:ShaderForge.SFN_Panner,id:437,x:31266,y:32525,varname:node_437,prsc:2,spu:0,spv:0.3|UVIN-2670-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:3780,x:31473,y:32525,ptovrint:False,ptlb:Texture_1,ptin:_Texture_1,varname:node_3780,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:1,isnm:False|UVIN-437-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:7269,x:31475,y:32716,ptovrint:False,ptlb:Texture_2,ptin:_Texture_2,varname:node_7269,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:1,isnm:False|UVIN-3894-UVOUT;n:type:ShaderForge.SFN_Lerp,id:9468,x:31871,y:32722,varname:node_9468,prsc:2|A-7269-RGB,B-3780-RGB,T-3780-A;n:type:ShaderForge.SFN_Multiply,id:3966,x:32079,y:32815,varname:node_3966,prsc:2|A-9468-OUT,B-7241-RGB,C-1412-OUT;n:type:ShaderForge.SFN_Add,id:4928,x:31872,y:33065,varname:node_4928,prsc:2|A-3780-A,B-7269-A;n:type:ShaderForge.SFN_VertexColor,id:6374,x:31872,y:33202,varname:node_6374,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6807,x:32108,y:33132,varname:node_6807,prsc:2|A-4928-OUT,B-6374-G;n:type:ShaderForge.SFN_ValueProperty,id:1412,x:31868,y:32650,ptovrint:False,ptlb:water_l,ptin:_water_l,varname:node_1412,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:7241-1412-3780-7269;pass:END;sub:END;*/

Shader "ji/pubu" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _water_l ("water_l", Float ) = 1
        _Texture_1 ("Texture_1", 2D) = "gray" {}
        _Texture_2 ("Texture_2", 2D) = "gray" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _Texture_1; uniform float4 _Texture_1_ST;
            uniform sampler2D _Texture_2; uniform float4 _Texture_2_ST;
            uniform float _water_l;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_8247 = _Time + _TimeEditor;
                float2 node_3894 = (i.uv0+node_8247.g*float2(0,0.6));
                float4 _Texture_2_var = tex2D(_Texture_2,TRANSFORM_TEX(node_3894, _Texture_2));
                float2 node_437 = (i.uv0+node_8247.g*float2(0,0.3));
                float4 _Texture_1_var = tex2D(_Texture_1,TRANSFORM_TEX(node_437, _Texture_1));
                float3 emissive = (lerp(_Texture_2_var.rgb,_Texture_1_var.rgb,_Texture_1_var.a)*_Color.rgb*_water_l);
                float3 finalColor = emissive;
                return fixed4(finalColor,((_Texture_1_var.a+_Texture_2_var.a)*i.vertexColor.g));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
