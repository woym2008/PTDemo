Shader "ji/niuqu_1" {
    Properties {
        _colormap ("color map", 2D) = "white" {}
        _TintColor ("Color", Color) = (1,1,1,1)
        _colorpower ("color power", Range(0, 10)) = 1
        _alpha ("alpha", Range(0, 0)) = 0
        _zhezhao ("zhezhao", 2D) = "white" {}
        _normalmap ("normal map", 2D) = "bump" {}
        _niuqu ("niuqu", Range(0, 1)) = 0.2
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
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
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _zhezhao; uniform float4 _zhezhao_ST;
            uniform float4 _TintColor;
            uniform sampler2D _normalmap; uniform float4 _normalmap_ST;
            uniform float _niuqu;
            uniform float _alpha;
            uniform sampler2D _colormap; uniform float4 _colormap_ST;
            uniform float _colorpower;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 _normalmap_var = UnpackNormal(tex2D(_normalmap,TRANSFORM_TEX(i.uv0, _normalmap)));
                float2 node_3576 = (_normalmap_var.rgb*i.vertexColor.a*_niuqu).rg;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + node_3576;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float4 _zhezhao_var = tex2D(_zhezhao,TRANSFORM_TEX(i.uv0, _zhezhao));
                clip((i.vertexColor.a*_zhezhao_var.a) - 0.5);
////// Lighting:
////// Emissive:
                float4 _colormap_var = tex2D(_colormap,TRANSFORM_TEX(i.uv0, _colormap));
                float3 emissive = (_colormap_var.rgb*_TintColor.rgb*_colorpower*i.vertexColor.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,(_colormap_var.a*_alpha*i.vertexColor.a)),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
