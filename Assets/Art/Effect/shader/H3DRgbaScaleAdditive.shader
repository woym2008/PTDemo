// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "H3D/InGame/RgbaScaleAdditive" {
	Properties{
		_MainTex("粒子纹理(RGB)", 2D) = "white" {}
		_MaskTex("Mask(Alpha)", 2D) = "white" {}

		_MaxColor("颜色最大值", Color) = (0.5, 0.5, 0.5, 0.5)
			_MinColor("颜色最小值", Color) = (0.5, 0.5, 0.5, 0.5)
			_ColorTime("周期时间", Float) = 1
					_ColorCount("颜色变化次数",Float) = -1

			_MaxScale("最大缩放", Float) = 10
			_MinScale("最小缩放", Float) = 1
			_ScaleTime("缩放时间", Float) = 1
			_ScaleCount("缩放次数",Float) = -1
			_AnchorPosition("AnchorPosition", Vector) = (0.0, 0.0, 0.0, 0.0)
			_StartTime("起始时间", Float) = 0
			_Alpha("透明度", Range(0, 1)) = 1

	}

	Category{
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "ForceNoShadowCasting" = "True" }
			Blend SrcAlpha One
			ZWrite Off Cull Off
			SubShader{
				Pass{

					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"

					sampler2D _MainTex;
					half4     _MainTex_ST;
					sampler2D _MaskTex;

					fixed4    _MaxColor;
					fixed4    _MinColor;
					float     _ColorTime;
					float     _ColorCount;

					float     _MaxScale;
					float     _MinScale;
					float     _ScaleTime;
					float     _ScaleCount;
					float4    _AnchorPosition;
					float     _StartTime;
					fixed     _Alpha;

					struct v2f {
						half4 vertex : POSITION;
						fixed4 color : COLOR;
						half2 texcoord : TEXCOORD0;
					};


					v2f vert(appdata_full v)
					{
						v2f o;
						float4 npos = 0;
						npos.x = v.vertex.x - _AnchorPosition.x;
						npos.y = v.vertex.y - _AnchorPosition.y;
						npos.z = v.vertex.z;
						npos.w = 1.0;
						
						float t = _Time.y - _StartTime;
						float scaleCount = step(0, _ScaleCount);
						float temp1 = step(t, _ScaleCount * _ScaleTime);

						float temp = (t % _ScaleTime) / _ScaleTime;
						float temp2 = lerp(_MinScale, _MaxScale, temp);

						float scale = (1-scaleCount) * temp2;

						scale += scaleCount * temp1 * temp2;
						scale += (1 - temp1) * _MaxScale;

						npos.x *= scale;
						npos.y *= scale;
						npos.x += _AnchorPosition.x;
						npos.y += _AnchorPosition.y;

						float colorCount = step(0, _ColorCount);
						float colorTemp1 = step(t, _ColorCount * _ColorTime);

						float colorTemp2 = abs(frac(t / _ColorTime + 0.5) * 2.0 - 1.0);
						fixed4 colorTemp3 = lerp(_MinColor, _MaxColor, colorTemp2);

						o.color = (1-colorCount + colorCount*colorTemp1) * colorTemp3;
						o.color += colorCount * (1 - colorTemp1) * _MinColor;

						o.vertex = UnityObjectToClipPos(npos);
						o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
						return o;
					}
					fixed4 frag(v2f i) : COLOR
					{
						fixed4 finalColor = i.color * tex2D(_MainTex, i.texcoord);
						finalColor.a = i.color.a *(tex2D(_MaskTex, i.texcoord)).r*2.0*_Alpha;

						return finalColor;
					}
						ENDCG
				}
			}

		}
}