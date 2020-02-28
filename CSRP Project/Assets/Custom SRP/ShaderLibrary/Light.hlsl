#ifndef CUSTOM_LIGHT_INCLUDED
#define CUSTOM_LIGHT_INCLUDED

#define MAX_DIRECTIONAL_LIGHT_COUNT 200

CBUFFER_START(_CustomLight)
	int _DirectionalLightCount;
	float4 _DirectionalLightColors[MAX_DIRECTIONAL_LIGHT_COUNT];
	float4 _VisibleLightDirectionsOrPositions[MAX_DIRECTIONAL_LIGHT_COUNT];
	float4 _VisibleLightAttenuations[MAX_DIRECTIONAL_LIGHT_COUNT];
	float _VisibleLightRanges[MAX_DIRECTIONAL_LIGHT_COUNT];
CBUFFER_END

struct Light {
	float3 color;
	float4 directionOrPosition;
	float range;
};

int GetDirectionalLightCount () {
	return _DirectionalLightCount;
}

Light GetDirectionalLight (int index) {
	Light light;
	light.color = _DirectionalLightColors[index].rgb;
	light.directionOrPosition = _VisibleLightDirectionsOrPositions[index].xyzw;
	light.range = _VisibleLightRanges[index];
	return light;
}

#endif