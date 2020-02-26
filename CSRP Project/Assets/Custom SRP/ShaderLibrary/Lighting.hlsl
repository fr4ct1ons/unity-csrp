#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

float3 IncomingLight (Surface surface, Light light, float3 worldPos) {
	float3 lightVec = light.directionOrPosition.xyz - worldPos * light.directionOrPosition.w;
	float3 lightDir = normalize(lightVec);
	return saturate(dot(surface.normal, lightDir)) * light.color;
}

float3 IncomingLightCelShaded (Surface surface, Light light, float3 worldPos) {

	float3 lightVec = light.directionOrPosition.xyz - worldPos * light.directionOrPosition.w;
	float3 lightDir = normalize(lightVec);
    float NdotL = saturate(dot(surface.normal, lightDir));
	
	float lightIntensity = 0.3;
	if(NdotL > 0)
	{
	    lightIntensity = 1.0;
	}
	return light.color * lightIntensity;
}

float3 GetLighting (Surface surface, Light light, float3 worldPos) {
	return IncomingLightCelShaded(surface, light, worldPos) * surface.color;
}

float3 GetLighting (Surface surface, float3 worldPos) {
	float3 color = 0.0;
	for (int i = 0; i < GetDirectionalLightCount(); i++) {
		color += GetLighting(surface, GetDirectionalLight(i), worldPos);
	}
	return color;
}

float3 GetIncomingLight (Surface surface) {
	//return GetLighting(surface, GetDirectionalLight());
}

#endif