#ifndef CUSTOM_LIT_PASS_INCLUDED
#define CUSTOM_LIT_PASS_INCLUDED

#include "../ShaderLibrary/Common.hlsl"
#include "../ShaderLibrary/Surface.hlsl"
#include "../ShaderLibrary/Shadows.hlsl"
#include "../ShaderLibrary/Light.hlsl"
#include "../ShaderLibrary/Lighting.hlsl"

TEXTURE2D(_BaseMap);
SAMPLER(sampler_BaseMap);

TEXTURE2D(_NormalMap);
SAMPLER(sampler_NormalMap);

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
	UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)
	UNITY_DEFINE_INSTANCED_PROP(float4, _NormalMap_ST)
	UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
	UNITY_DEFINE_INSTANCED_PROP(float, _Cutoff)
	UNITY_DEFINE_INSTANCED_PROP(float, _CelShaded)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

struct Attributes {
	float3 positionOS : POSITION;
	float3 normalOS : NORMAL;
	float2 baseUV : TEXCOORD0;
	float4 tangent: TANGENT;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings {
	float4 positionCS : SV_POSITION;
	float3 positionWS : VAR_POSITION;
	float3 normalWS : VAR_NORMAL;
	float2 baseUV : VAR_BASE_UV;
	float3 tspace0 : TEXCOOR1;
	float3 tspace1 : TEXCOOR2;
	float3 tspace2 : TEXCOOR3;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

Varyings LitPassVertex (Attributes input) {
	Varyings output;
	UNITY_SETUP_INSTANCE_ID(input);
	UNITY_TRANSFER_INSTANCE_ID(input, output);
	output.positionWS = TransformObjectToWorld(input.positionOS);
	output.positionCS = TransformWorldToHClip(output.positionWS);
	output.normalWS = TransformObjectToWorldNormal(input.normalOS);

	float4 baseST = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseMap_ST);
	output.baseUV = input.baseUV * baseST.xy + baseST.zw;
	
	float3 wTangent = TransformObjectToWorldDir(input.tangent.xyz);
	float tangentSign = input.tangent.w * unity_WorldTransformParams.w;
	float3 wBitangent = cross(output.normalWS, wTangent) * tangentSign;
	output.tspace0 = float3(wTangent.x, wBitangent.x, output.normalWS.x);
    output.tspace1 = float3(wTangent.y, wBitangent.y, output.normalWS.y);
    output.tspace2 = float3(wTangent.z, wBitangent.z, output.normalWS.z);
	return output;
}

float4 LitPassFragment (Varyings input) : SV_TARGET {
	UNITY_SETUP_INSTANCE_ID(input);
	float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.baseUV);
	float4 baseColor = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
	float4 base = baseMap * baseColor;
	#if defined(_CLIPPING)
		clip(base.a - UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Cutoff));
	#endif
	
	float3 tNormal = SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, input.baseUV);
	
	tNormal *= 2;
	tNormal -= 1;

	Surface surface;
	surface.position = input.positionWS;
	surface.normal = normalize(input.normalWS);

	surface.normal.x = dot(input.tspace0, tNormal);
	surface.normal.y = dot(input.tspace1, tNormal);
	surface.normal.z = dot(input.tspace2, tNormal);
	
	surface.depth = -TransformWorldToView(input.positionWS).z;
	surface.color = base.rgb;
	surface.alpha = base.a;
	surface.dither = InterleavedGradientNoise(input.positionCS.xy, 0);

	float3 color = GetLighting(surface, _CelShaded);
	return float4(color, surface.alpha);
}

#endif