// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "H3D/InGame/RgbaRotationAdditive" {
	Properties{
		_MainTex("粒子纹理(RGB)", 2D) = "white" {}
		_MaskTex("Mask(Alpha)", 2D) = "white" {}
		_MaxColor("颜色最大值", Color) = (0.5, 0.5, 0.5, 0.5)
		_MinColor("颜色最小值", Color) = (0.5, 0.5, 0.5, 0.5)
		_ColorTime("周期时间", Float) = 1
		_ColorCount("颜色变化次数",Float) = -1
		_RotationSpeed("角速度", Float) = 10
		_RotationTime("旋转周期",Float) = -1
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
							float     _RotationSpeed;
							float     _RotationTime;
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

							float t = _Time.y - _StartTime;
							float temp2 = fmod(_RotationSpeed * t, 360);
							float isRotate = step(0, _RotationTime);
							float angle = (1 - isRotate) * temp2;

							float temp = step(t, _RotationTime);
							angle += isRotate * temp * temp2;

							float vcos = cos(angle);
							float vsin = sin(angle);
							float2 rotMat1 = float2(vcos, -vsin);
							float2 rotMat2 = float2(vsin, vcos);
							
							o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
							o.texcoord.x = (dot(rotMat1, v.texcoord.xy - float2(0.5, 0.5)) + 0.5);
							o.texcoord.y = (dot(rotMat2, v.texcoord.xy - float2(0.5, 0.5)) + 0.5);

						float colorCount = step(0, _ColorCount);
						float colorTemp1 = step(t, _ColorCount * _ColorTime);

						float colorTemp2 = abs(frac(t / _ColorTime + 0.5) * 2.0 - 1.0);
						fixed4 colorTemp3 = lerp(_MinColor, _MaxColor, colorTemp2);

						o.color = (1-colorCount + colorCount*colorTemp1) * colorTemp3;
						o.color += colorCount * (1 - colorTemp1) * _MinColor;

							o.vertex = UnityObjectToClipPos(v.vertex);
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



		}//end Category
}