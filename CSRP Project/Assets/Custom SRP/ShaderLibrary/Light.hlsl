#ifndef CUSTOM_LIGHT_INCLUDED
#define CUSTOM_LIGHT_INCLUDED

#define MAX_DIRECTIONAL_LIGHT_COUNT 200

CBUFFER_START(_CustomLight)
	int _DirectionalLightCount;
	float4 _DirectionalLightColors[MAX_DIRECTIONAL_LIGHT_COUNT];
	float4 _VisibleLightDirectionsOrPositions[MAX_DIRECTIONAL_LIGHT_COUNT];
	float4 _VisibleLightAttenuations[MAX_DIRECTIONAL_LIGHT_COUNT];
	float _VisibleLightRanges[MAX_DIRECTIONAL_LIGHT_COUNT];
	float4 _DirectionalLightShadowData[MAX_DIRECTIONAL_LIGHT_COUNT];
CBUFFER_END

struct Light {
	float3 color;
	float4 directionOrPosition;
	float range;
	float attenuation;
};

int GetDirectionalLightCount () {
	return _DirectionalLightCount;
}

DirectionalShadowData GetDirectionalShadowData (int lightIndex, ShadowData shadowData) {
	DirectionalShadowData data;
	data.strength = _DirectionalLightShadowData[lightIndex].x;
	data.tileOffset = _DirectionalLightShadowData[lightIndex].y + shadowData.cascadeIndex;
	return data;
}

Light GetDirectionalLight (int index, Surface surfaceWS, ShadowData shadowData)
{
	Light light;
	light.color = _DirectionalLightColors[index].rgb;
	light.directionOrPosition = _VisibleLightDirectionsOrPositions[index].xyzw;
	DirectionalShadowData dirShadowData = GetDirectionalShadowData(index, shadowData);
	light.attenuation = GetDirectionalShadowAttenuation(dirShadowData, surfaceWS);
	light.range = _VisibleLightRanges[index];
	return light;
}

#endif