// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'


// Upgrade NOTE: excluded shader from DX11, Xbox360, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 xbox360 gles
#ifndef H3D_FIRMWORK_INCLUDED
#define H3D_FIRMWORK_INCLUDED
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"			
#include "HLSLSupport.cginc"		
#include "UnityShaderVariables.cginc"
#include "UnityPBSLighting.cginc"

//ClipRect
uniform fixed4 _MapClipRect;			
			
//SpotLightInscene
uniform fixed3 _PointLightInscene_PosWorld = fixed3(0, 0, 0);
uniform fixed _PointLightInscene_Intensity = 1;
uniform fixed _PointLightInscene_Radius = 5;
uniform fixed4 _PointLightInscene_Color;
uniform fixed4 _PointLightInscene_SpecularColor;
uniform fixed _PointLightInscene_SpecularGloss = 5;


#if defined(_ON_H3D_SPOTLIGHTINSCENE)
 	#define H3D_SPOTLIGHTINSCENE_COLOR(idx) float3 H3D_spotLightInSceneColor : TEXCOORD##idx;
	#define H3D_SPOTLIGHTINSCENE_COLOR_FOR_SURFACE float3 worldPos;

	#define H3D_SPOTLIGHTINSCENE_CALC(v, o)\
		float3 posWorld = mul(unity_ObjectToWorld, v.vertex);\
		fixed3 normalWorld = UnityObjectToWorldNormal(v.normal);\
		o.H3D_spotLightInSceneColor = H3DAddSpotLightInscene(normalWorld, posWorld);	

	#define H3D_SPOTLIGHTINSCENE_GETCOLOR(color, i)\
		i.H3D_spotLightInSceneColor

	#define H3D_SPOTLIGHTINSCENE_GETCOLOR_FOR_SURFACE(normalDir, posWorld)\
		H3DAddSpotLightInscene(normalDir, posWorld)

#else
 	#define H3D_SPOTLIGHTINSCENE_COLOR(idx)
	#define H3D_SPOTLIGHTINSCENE_COLOR_FOR_SURFACE
	#define H3D_SPOTLIGHTINSCENE_CALC(v, o)
	#define H3D_SPOTLIGHTINSCENE_GETCOLOR(color, i) 0
	#define H3D_SPOTLIGHTINSCENE_GETCOLOR_FOR_SURFACE(normalDir, posWorld) 0

#endif  

inline float3 H3DAddSpotLightInscene(float3 normalDir, float3 posWorld)
{
	float3 normalDirection = normalize(normalDir); 
	float3 vertexToLightSource = _PointLightInscene_PosWorld.xyz - posWorld.xyz;
	float lightDirection = normalize(vertexToLightSource);
    float3 viewDirection = normalize(_WorldSpaceCameraPos - posWorld.xyz);  

    float distance = length(vertexToLightSource);	
	float attenuation = max( 0.0 ,1.0 - distance / max( 0.01 , _PointLightInscene_Radius ) ) * _PointLightInscene_Intensity;
	//float attenuation = (1.0 / distance) * _PointLightInscene_Intensity; // linear attenuation 
	//* _LightColor0.rgb		 
	float3 diffuseReflection = attenuation  * _PointLightInscene_Color.rgb * 2;
	float3 specularReflection = attenuation * _PointLightInscene_SpecularColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _PointLightInscene_SpecularGloss); 

	float3 rgb  = float4(diffuseReflection, 0) + float4(specularReflection, 0);

	return rgb;
}

// lightmap 
#if UNITY_VERSION >= 500
	#define H3D_LightMap_Declare  	
#else
	#define H3D_LightMap_Declare  	fixed4 unity_LightmapST;   // sampler2D unity_Lightmap;
	
#endif


#if UNITY_VERSION >= 500
	#define H3D_SAMPLE_TEX2D(texture_map,input_uv) UNITY_SAMPLE_TEX2D(texture_map, input_uv)
#else
    #define H3D_SAMPLE_TEX2D(texture_map,input_uv) tex2D(texture_map,input_uv)
#endif
// end lightmap

// H3D shader variables

half   _Shininess;

half      _Flash;
fixed4    _FlashColor;
uniform float  _DpiScale;

//用来避免时间精度问题
uniform float  _GlobalWrapShaderTime;


uniform float4 h3d_CharactorLightDir;
uniform fixed4 h3d_CharactorLightColor;
uniform float4 h3d_FogParam;       // x near, y far , z world bottom, w world top
uniform fixed4 h3d_FogDistanceColor;
uniform fixed4 h3d_FogHeightColor;

uniform sampler2D h3d_GlobalMatCapTextrue;
uniform fixed4 h3d_GlobalMatCapColor;

// H3D shader variables end
// Charactor Flash Color
#define H3D_CH_FLASH_COLOR(c) c = ((c / (1.0 + _Flash)) + (_Flash * _FlashColor))

//用于LOD2下Static Shader的Lightmap缩放
#define H3D_LIGHTMAP_LOD2_SCALE 1.5


//lighting
inline fixed4 LightingH3DLambert (SurfaceOutput s, fixed3 lightDir, fixed atten)
{
	fixed3 lightColor = _LightColor0.rgb;
	#if defined(_ON_H3D_CHLIGHT)
	lightDir = h3d_CharactorLightDir;
	lightColor = h3d_CharactorLightColor;
	atten = 1;
	#endif
	
  	fixed4 c;
    fixed diff = max (0,dot(s.Normal,lightDir));
    c.rgb = s.Albedo *  lightColor * diff * atten;
	c.a = s.Alpha;
	return c;
}

inline fixed4 LightingH3DBlinnPhong(SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
{
    fixed3 lightColor = _LightColor0.rgb;
	#if defined(_ON_H3D_CHLIGHT)
	lightDir = h3d_CharactorLightDir;
	lightColor = h3d_CharactorLightColor;
	atten = 1;
	#endif
	fixed3 h    = normalize(lightDir + viewDir);
	fixed  diff = max (0, dot (s.Normal, lightDir));
	fixed  nh   = max (0, dot (s.Normal, h));
	fixed  sp   = pow (nh, s.Specular*128.0) * s.Gloss;
	fixed4 c;	
	c.rgb       = (s.Albedo *  lightColor  * diff +  lightColor  * _SpecColor.rgb * sp) * atten;
	c.a = s.Alpha;
	return c;
}

inline fixed4 LightingH3DBlinnPhongHalf(SurfaceOutput s, fixed3 lightDir, fixed3 halfViewDir, fixed atten)
{
	fixed3 lightColor = _LightColor0.rgb;
	#if defined(_ON_H3D_CHLIGHT)
	lightDir = h3d_CharactorLightDir;
	lightColor = h3d_CharactorLightColor;
	atten = 1;
	#endif
	fixed  diff = max (0, dot (s.Normal, lightDir));
	fixed  nh   = max (0, dot (s.Normal, halfViewDir));
	fixed  sp   = pow (nh, s.Specular*128.0) * s.Gloss;
	fixed4 c;	
	c.rgb       = (s.Albedo * lightColor * diff + lightColor * _SpecColor.rgb * sp) * atten;
	c.a = s.Alpha;
	return c;
}


// lightmap + lighting
inline fixed4 H3DBlinnPhongAdd(SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
{
	fixed3 lightColor = _LightColor0.rgb;
	fixed3 h  = normalize(lightDir + viewDir);
	fixed  nh = max (0, dot (s.Normal, h));
	fixed  sp = pow (nh, s.Specular*128.0) * s.Gloss;
	fixed4 c;	
	c.rgb     = (s.Albedo + lightColor * _SpecColor.rgb * sp ) * atten;
	c.a       = s.Alpha;
	return c;
}

inline fixed4 H3DBlinnPhongAddHalf(SurfaceOutput s,fixed3 halfView,fixed atten)
{
	fixed3 lightColor = _LightColor0.rgb;
    fixed nh = max (0, dot (s.Normal, halfView));
    fixed spec = pow (nh, s.Specular*128) * s.Gloss;
    fixed4 c;
    c.rgb = (s.Albedo + lightColor.rgb * spec * _SpecColor.rgb) * atten;
    c.a = s.Alpha;
    return c;
}
// lightmap + lighting end


//vertex lit lighting
inline fixed3 H3DLightingLambert (half3 normal, half3 lightDir)
{
	fixed3 lightColor = _LightColor0.rgb;
	#if defined(_ON_H3D_CHLIGHT)
	lightDir = h3d_CharactorLightDir;
	lightColor = h3d_CharactorLightColor;
	#endif
	fixed diff = max (0, dot (normal, lightDir));		
	return lightColor.rgb * diff ;
}

inline fixed3 H3DLightingBPhong (half3 normal,half3 viewDir, half3 lightDir)
{
	fixed3 lightColor = _LightColor0.rgb;
	#if defined(_ON_H3D_CHLIGHT)
	lightDir = h3d_CharactorLightDir;
	lightColor = h3d_CharactorLightColor;
	#endif
    half3 h = normalize (lightDir + viewDir);
	fixed diff = max (0, dot (normal, lightDir));
	half nh = max (0, dot (normal,h));
	half spec = pow (nh, _Shininess*128.0)*4.0;
	return  lightColor * diff + lightColor * _SpecColor * spec;
}

//用于计算顶点高光
inline fixed3 H3DLightingBPhongSpec (half3 normal,half3 viewDir, half3 lightDir)
{
	fixed3 lightColor = _LightColor0.rgb;
	#if defined(_ON_H3D_CHLIGHT)
	lightDir = h3d_CharactorLightDir;
	lightColor = h3d_CharactorLightColor;
	#endif
    half3 h = normalize (lightDir + viewDir); 
	half nh = max (0, dot (normal,h));
	half spec = pow (nh, _Shininess*128.0)*4.0;
	return lightColor * _SpecColor * spec;
}


#define H3D_LIGHT_COODS(indx1) fixed3 _vlight : TEXCOORD##indx1;

#define H3D_LAMBERT_LIGHT(o) \
	float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;\
	fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);\
	o._vlight = ShadeSH9 (float4(worldNormal,1.0));\
	o._vlight += H3DLightingLambert (worldNormal, _WorldSpaceLightPos0.xyz);\
	o._vlight += Shade4PointLights (\
    unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,\
    unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,\
    unity_4LightAtten0, worldPos, worldNormal)
    
#define H3D_BLINNPHONG_LIGHT(o) \
	float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;\
	fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);\
	fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));\
	o._vlight = ShadeSH9 (float4(worldNormal,1.0));\
	o._vlight += H3DLightingBPhong (worldNormal,worldViewDir, _WorldSpaceLightPos0.xyz);\
	o._vlight += Shade4PointLights (\
    unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,\
    unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,\
    unity_4LightAtten0, worldPos, worldNormal)
    
//用于计算顶点Phong高光
#define H3D_BLINNPHONG_LIGHT_SPEC(o) \
	float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;\
	fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);\
	fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));\
	o._vlight = H3DLightingBPhongSpec(worldNormal,worldViewDir, _WorldSpaceLightPos0.xyz)
	

#define H3D_SIMPLE_GETLIGHT(i) i._vlight
#define H3D_SURFACE_LIGHT(i,o)  i._vlight * o.Albedo

half      _SHLightingScale;
#define H3D_AMBIENT_LIGHT_COODS(indx1) fixed3 _ambientLight : TEXCOORD##indx1;
#define H3D_AMBIENT_LIGHT(o)\
	fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);\
	o._ambientLight = ShadeSH9 (float4(worldNormal,1.0)) * _SHLightingScale;
#define H3D_AMBIENT_GETLIGHT(i) i._ambientLight
//vertex lit lighting end


//lighting end
    
//TexGen SphereMap
float2 H3DObjSpaceSphereMapUV(float3 refl)
{

	refl = mul((float3x3)UNITY_MATRIX_MV, refl);
	refl.z += 1;
	float m = 2 * length(refl);
	return refl.xy / m + 0.5;
}

float2 H3DObjSpaceSphereMapUV(float4 vertex,float3 normal) 
{
	 float3 viewDir = normalize(ObjSpaceViewDir(vertex));      
	 float3 r = reflect(-viewDir, normal);
	 return H3DObjSpaceSphereMapUV(r);
 }
//TexGen SphereMap end

half3 H3DUnpackScaleNormal(half4 packednormal, half bumpScale)
{
    #if defined(UNITY_NO_DXT5nm)
        half3 normal;
		normal.xy = (packednormal.xy * 2 - 1) * bumpScale; 
		normal.z = sqrt(1.0 - saturate(dot(normal.xy, normal.xy)));
		return normal;
    #else
		half3 normal;
		normal.xy = (packednormal.wy * 2 - 1);
		#if (SHADER_TARGET >= 30)
			// SM2.0: instruction count limitation
			// SM2.0: normal scaler is not supported
			normal.xy *= bumpScale;
		#endif
		normal.z = sqrt(1.0 - saturate(dot(normal.xy, normal.xy)));
		return normal;
    #endif
}
	
 
 // H3D Fog


 
 #if defined(_ON_H3D_FOG)
 	
 	#if defined(_ON_H3D_H_FOG)
 		#define H3D_FOG_COORDS(idx) float2 h3d_fogCoord : TEXCOORD##idx;
	 	#define H3D_TRANSFER_FOG(o,wv,lv)\
		 	o.h3d_fogCoord.x = ( h3d_FogParam.y - wv.z )/(  h3d_FogParam.y - h3d_FogParam.x);\
		    o.h3d_fogCoord.y = ( (mul(unity_ObjectToWorld ,lv)).y - h3d_FogParam.z) / (h3d_FogParam.w - h3d_FogParam.z)
	    #define H3D_APPLY_FOG(coord,col)\
		    col.rgb = lerp(h3d_FogDistanceColor.rgb,col.rgb, saturate(coord.x));\
		    col.rgb = lerp(h3d_FogHeightColor.rgb,col.rgb,saturate(coord.y))
	#else
		#define H3D_FOG_COORDS(idx) float h3d_fogCoord : TEXCOORD##idx;
		#define H3D_TRANSFER_FOG(o,wv,lv)\
		 	o.h3d_fogCoord.x = ( h3d_FogParam.y -wv.z )/(  h3d_FogParam.y - h3d_FogParam.x)
		    
	    #define H3D_APPLY_FOG(coord,col)\
		    col.rgb = lerp(h3d_FogDistanceColor.rgb,col.rgb, saturate(coord.x))
	#endif
 #else
	#define H3D_FOG_COORDS(idx)
	#define H3D_TRANSFER_FOG(o,wv,lv)
	#define H3D_APPLY_FOG(coord,col)
 #endif  
 
struct H3dSurfaceOutput{
	fixed3 Albedo;
	fixed3 Normal;
	fixed3 Emission;
	half   Specular;
	fixed  Gloss;
	fixed  Alpha;
	half2  h3d_fogCoord;
};


inline fixed4 LightingH3DLambertFog (H3dSurfaceOutput s, fixed3 lightDir, fixed atten)
{
	fixed diff = max (0, dot (s.Normal, lightDir));
	fixed4 c;
	c.rgb = s.Albedo *  _LightColor0.rgb * diff * atten;
	c.a = s.Alpha;
	return c;
}

inline fixed4 LightingH3DBlinnPhongFog(H3dSurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
{

	fixed3 h    = normalize(lightDir + viewDir);
	fixed  diff = max (0, dot (s.Normal, lightDir));
	fixed  nh   = max (0, dot (s.Normal, h));
	fixed  sp   = pow (nh, s.Specular*128.0) * s.Gloss;
	fixed4 c;	
	c.rgb       = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * _SpecColor.rgb * sp) * atten;
	c.a = s.Alpha;
	return c;
}
 
 
// H3D Fog end 

// standard lighting
#define H3D_TANGENT_POS_COORDS(indx1) half4  tangentToWorldAndWorldPos[3] : TEXCOORD##indx1;
#define H3D_WORLD_TANGENT_POS(o)\
	float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;\
	fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);\
	fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);\
	fixed  tangentSign = v.tangent.w * unity_WorldTransformParams.w;\
	fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;\
	o.tangentToWorldAndWorldPos[0] = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);\
	o.tangentToWorldAndWorldPos[1] = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);\
	o.tangentToWorldAndWorldPos[2] = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z)
#define H3D_WORLD_POS(i) float3(i.tangentToWorldAndWorldPos[0].w, i.tangentToWorldAndWorldPos[1].w, i.tangentToWorldAndWorldPos[2].w)
#define H3D_WORLD_NORMAL(i,normal) fixed3(dot(i.tangentToWorldAndWorldPos[0].xyz, normal),dot(i.tangentToWorldAndWorldPos[1].xyz, normal),dot(i.tangentToWorldAndWorldPos[2].xyz, normal))
struct H3DSurfaceOutputStandardSpecular {
	SurfaceOutputStandardSpecular standardData;
	float3 worldPos;
	float3 worldViewDir;
	fixed3 sh;
};

#define H3D_WORLD_TANGENT_POS_AND_AMBIENT(o)\
	float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;\
	fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);\
	fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);\
	fixed  tangentSign = v.tangent.w * unity_WorldTransformParams.w;\
	fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;\
	o.tangentToWorldAndWorldPos[0] = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);\
	o.tangentToWorldAndWorldPos[1] = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);\
	o.tangentToWorldAndWorldPos[2] = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);\
	o._ambientLight = ShadeSH9 (float4(worldNormal,1.0))
#define H3D_POS_NORMAL_COORDS(indx1) half3  worldPosNormal[2] : TEXCOORD##indx1;

#define H3D_WORLD_POS_S(i) i.worldPosNormal[0]
#define H3D_WORLD_NORMAL_S(i) i.worldPosNormal[1]

#define H3D_POS_NORMAL_AND_AMBIENT(o)\
	float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;\
	fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);\
	o.worldPosNormal[0]  = worldPos;\
	o.worldPosNormal[1]  = worldNormal;\
	o._ambientLight = ShadeSH9 (float4(worldNormal,1.0))

uniform float4 h3d_OutGame_LightDir0;
uniform fixed4 h3d_OutGame_LightColor0;

uniform float4 h3d_OutGame_LightDir1;
uniform fixed4 h3d_OutGame_LightColor1;

void h3d_stardardLight_GI(H3DSurfaceOutputStandardSpecular o,inout UnityGI gi)
{
	UnityGIInput giInput;
	UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
	giInput.light = gi.light;
	giInput.worldPos = o.worldPos;
	giInput.worldViewDir = o.worldViewDir;
	giInput.atten = 1;
    giInput.ambient = o.sh;
	giInput.probeHDR[0] = unity_SpecCube0_HDR;
	giInput.probeHDR[1] = unity_SpecCube1_HDR;
	LightingStandardSpecular_GI(o.standardData, giInput, gi);
}

UnityGI h3d_unityGI(H3DSurfaceOutputStandardSpecular o, fixed3 lightDir,fixed4 lightColor)
{
	UnityGI gi;
	UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
	gi.indirect.diffuse = 0;
	gi.indirect.specular = 0;
	gi.light.color = lightColor.rgb;
	gi.light.dir = lightDir;
	gi.light.ndotl = LambertTerm (o.standardData.Normal, gi.light.dir);
	return gi;
}

//struct SurfaceOutputStandardSpecular0
//{
//	fixed3 Albedo;		// diffuse color
//	fixed3 Specular;	// specular color
//	fixed3 Normal;		// tangent space normal, if written
//	half3 Emission;
//	half Smoothness;	// 0=rough, 1=smooth
//	half Occlusion;		// occlusion (default 1)
//	fixed Alpha;		// alpha for transparencies
//};

fixed4 h3d_standerdSpecularLighting(H3DSurfaceOutputStandardSpecular o)
{
	fixed4 c = 0;
	fixed4 nc = 0;
    #if !defined(H3D_OUT_GAME_LIGHT)
	fixed3 lightDir0 = _WorldSpaceLightPos0.xyz;
	fixed4 lightColor0 = _LightColor0;
	#else
	fixed3 lightDir0 =  h3d_OutGame_LightDir0.xyz;
	fixed4 lightColor0 = h3d_OutGame_LightColor0;
	#endif
    UnityGI gi = h3d_unityGI(o , lightDir0, lightColor0);
    
	h3d_stardardLight_GI(o,gi);

	c += LightingStandardSpecular (o.standardData, o.worldViewDir, gi);
	c.rgb +=  o.standardData.Emission;
	c.a = 0.0;
	
	#if defined(H3D_OUT_GAME_LIGHT)
		#if defined(H3D_OUT_GAME_LIGHT_ADD)
		UnityGI ngi = h3d_unityGI(o ,h3d_OutGame_LightDir1.xyz, h3d_OutGame_LightColor1);
		nc += LightingStandardSpecular (o.standardData, o.worldViewDir, ngi);
		nc.a =0.0;
		#endif
	#endif
	
	return c + nc;
}
 
 
 
struct H3dSurfaceOutputSepcular{
	fixed3 Albedo;
	fixed3 Normal;
	fixed3 Emission;
	fixed  Alpha;
	fixed  specMask;
	fixed3 viewDir;
	fixed3 SpecColor;
	half   SpecShininess;
	half   SpecIntensity;
};
inline fixed4 h3d_specularLighting_blinPhong(H3dSurfaceOutputSepcular s, fixed3 lightDir,fixed4 lightColor)
{
	s.Normal = normalize(s.Normal);
	fixed3 h    = normalize(lightDir + s.viewDir);
	fixed  diff = max (0, dot (s.Normal, lightDir));
	fixed  nh   = max (0, dot (s.Normal, h));
	fixed  sp   = pow (nh, s.SpecShininess) * s.SpecIntensity;
	fixed4 c;	
	c.rgb       = (s.Albedo * (lightColor.rgb * diff )+ lightColor.rgb * s.SpecColor * sp*s.specMask);
	c.a = s.Alpha;
	return c;
}
fixed4 h3d_specularLighting(H3dSurfaceOutputSepcular o)
{
	fixed4 c = 0;
	fixed4 nc = 0;
    #if !defined(H3D_OUT_GAME_LIGHT)
	fixed3 lightDir0 = _WorldSpaceLightPos0.xyz;
	fixed4 lightColor0 = _LightColor0;
	#else
	fixed3 lightDir0 =  h3d_OutGame_LightDir0.xyz;
	fixed4 lightColor0 = h3d_OutGame_LightColor0;
	#endif

	c += h3d_specularLighting_blinPhong (o,lightDir0, lightColor0);
	c.rgb += o.Emission;
	#if defined(H3D_OUT_GAME_LIGHT)
		#if defined(H3D_OUT_GAME_LIGHT_ADD)
		nc += h3d_specularLighting_blinPhong (o, h3d_OutGame_LightDir1.xyz,h3d_OutGame_LightColor1);
//		nc.rgb += o.Emission;
		#endif
	#endif
	return c+nc;
} 
 

fixed4 h3d_unityLighting_blinPhong(SurfaceOutput s, fixed3 viewDir,fixed3 lightDir,fixed4 lightColor)
{

	fixed3 h    = normalize(lightDir + viewDir);
	fixed  diff = max (0, dot (s.Normal, lightDir));
	fixed  nh   = max (0, dot (s.Normal, h));
	fixed  sp   = pow (nh, s.Specular*128.0) * s.Gloss;

	fixed4 c;	
	c.rgb       = (s.Albedo * lightColor.rgb * diff + lightColor.rgb * _SpecColor.rgb * sp);
	c.a = s.Alpha;
	return c;
}


fixed4 h3d_unityLighting(SurfaceOutput o,fixed3 viewDir)
{
	fixed4 c = 0;
	fixed4 nc = 0;
    #if !defined(H3D_OUT_GAME_LIGHT)
	fixed3 lightDir0 = _WorldSpaceLightPos0.xyz;
	fixed4 lightColor0 = _LightColor0;
	#else
	fixed3 lightDir0 =  h3d_OutGame_LightDir0.xyz;
	fixed4 lightColor0 = h3d_OutGame_LightColor0;
	#endif

	c += h3d_unityLighting_blinPhong (o,viewDir,lightDir0, lightColor0);
	c.rgb += o.Emission;
	#if defined(H3D_OUT_GAME_LIGHT)
		#if defined(H3D_OUT_GAME_LIGHT_ADD)
		nc += h3d_unityLighting_blinPhong (o,viewDir,h3d_OutGame_LightDir1.xyz,h3d_OutGame_LightColor1);
//		nc.rgb += o.Emission;
		#endif
	#endif
	return c+nc;
} 
 
 
  
 //草皮碰撞
 uniform float	_GlobalCycleRad;
 uniform float4 pointForceAreaPos;
 uniform float  pointForceAreaRadius;
 uniform float  pointForceAreaIntensity;
 
 

#endif //H3D_FIRMWORK_INCLUDED
