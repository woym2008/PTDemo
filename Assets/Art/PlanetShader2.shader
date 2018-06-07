Shader "Custom/PlanetShader2" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_QOffset("Offset", Vector) = (0,0,0,0)
		_Brightness("Brightness", Float) = 0.0
		_Dist("Distance", Float) = 100.0

	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200

		Pass
	{

		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "Lighting.cginc"

		sampler2D _MainTex;
	float4 _QOffset;
	float _Dist;
	float _Brightness;
	float4 _Color;

	struct v2f {
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
		float3 normal : TEXCOORD1;
		fixed4 color : COLOR;
	};

	v2f vert(appdata_full v)
	{
		v2f o;
		float4 vPos = mul(UNITY_MATRIX_MV, v.vertex);
		o.normal = mul(v.normal, (float3x3)unity_WorldToObject);

		float zOff = vPos.z / _Dist;
		vPos += _QOffset * zOff * zOff;
		o.pos = mul(UNITY_MATRIX_P, vPos);
		o.uv = v.texcoord;
		return o;
	}

	half4 frag(v2f i) : COLOR0
	{
		fixed3 worldNormal = normalize(i.normal);
	fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);

	fixed3 NdotL = 0.5 * dot(worldNormal, worldLightDir) + 0.5;
	half4 col = tex2D(_MainTex, i.uv.xy);
	//col *= UNITY_LIGHTMODEL_AMBIENT * _Brightness * half4(NdotL,1.0) * _Color;
	col *= UNITY_LIGHTMODEL_AMBIENT * _Brightness * half4(NdotL, 1.0) *_Color;
	return col;
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
}