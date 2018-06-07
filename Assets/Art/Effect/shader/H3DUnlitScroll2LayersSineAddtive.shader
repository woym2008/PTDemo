// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "H3D/InGame/Others/Scroll 2 Layers Sine Addtive" {
Properties {
	_MainTex ("Base layer (RGB)", 2D) = "white" {}
	_MaskTex ("Mask(R _Main Alpha,B DetailTex Alpha)", 2D) = "white" {}
//	[Toggle(ENABLE_MASK)] _EnableMask ("Enable Mask", Int) = 1
	_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
	_ScrollX ("Base layer Scroll speed X", Float) = 1.0
	_ScrollY ("Base layer Scroll speed Y", Float) = 0.0
	_Scroll2X ("2nd layer Scroll speed X", Float) = 1.0
	_Scroll2Y ("2nd layer Scroll speed Y", Float) = 0.0
	_SineAmplX ("Base layer sine amplitude X",Float) = 0.5 
	_SineAmplY ("Base layer sine amplitude Y",Float) = 0.5
	_SineFreqX ("Base layer sine freq X",Float) = 10 
	_SineFreqY ("Base layer sine freq Y",Float) = 10
	_SineAmplX2 ("2nd layer sine amplitude X",Float) = 0.5 
	_SineAmplY2 ("2nd layer sine amplitude Y",Float) = 0.5
	_SineFreqX2 ("2nd layer sine freq X",Float) = 10 
	_SineFreqY2 ("2nd layer sine freq Y",Float) = 10
	_Color("Color", Color) = (1,1,1,1) 
	_MMultiplier ("Layer Multiplier", Float) = 2.0
	_Alpha("透明度",Range(0,1)) = 1
}

	
SubShader {
    LOD 200
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	
	
		CGINCLUDE
		#pragma multi_compile __ _ON_H3D_FOG 
		#pragma multi_compile __ _ON_H3D_H_FOG 
//		#pragma shader_feature ENABLE_MASK
		#include "H3DFramework.cginc"
		sampler2D _MainTex;
		sampler2D _MaskTex;
		sampler2D _DetailTex;

		half4 _MainTex_ST;
		half4 _DetailTex_ST;
	
		half _ScrollX;
		half _ScrollY;
		half _Scroll2X;
		half _Scroll2Y;
		half _MMultiplier;
	
		half _SineAmplX;
		half _SineAmplY;
		half _SineFreqX;
		half _SineFreqY;

		half _SineAmplX2;
		half _SineAmplY2;
		half _SineFreqX2;
		half _SineFreqY2;
		fixed _Alpha;
		fixed4 _Color;

		struct v2f {
			half4 pos : SV_POSITION;
			half4 uv : TEXCOORD0;
			fixed4 color : TEXCOORD1;
			H3D_FOG_COORDS(2)
		};	
		v2f vert (appdata_full v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex) + frac(half2(_ScrollX, _ScrollY) * _Time);
			o.uv.zw = TRANSFORM_TEX(v.texcoord.xy,_DetailTex) + frac(half2(_Scroll2X, _Scroll2Y) * _Time);
		
			o.uv.x += sin(_Time * _SineFreqX) * _SineAmplX;
			o.uv.y += sin(_Time * _SineFreqY) * _SineAmplY;
		
			o.uv.z += sin(_Time * _SineFreqX2) * _SineAmplX2;
			o.uv.w += sin(_Time * _SineFreqY2) * _SineAmplY2;
		
			o.color = _MMultiplier * _Color * v.color;
			H3D_TRANSFER_FOG(o,o.pos,v.vertex);  
			return o;
		}
		ENDCG
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 c;
				fixed4 tex = tex2D (_MainTex, i.uv.xy);
//				#if ENABLE_MASK
				fixed4 mask = tex2D (_MaskTex,i.uv.xy);
//				#else
//				fixed4 mask = tex.a;
//				#endif
				fixed4 tex2 = tex2D (_DetailTex, i.uv.zw); 
				c = tex * tex2 * i.color;
				H3D_APPLY_FOG(i.h3d_fogCoord, c);
				c.a = mask.r* mask.g*_Alpha*i.color.a;
					
				return c;
			}
			ENDCG 
		}	
	}

	FallBack "H3D/InGame/SceneStaticObjects/SimpleDiffuse ( no Supports Lightmap)"
}
