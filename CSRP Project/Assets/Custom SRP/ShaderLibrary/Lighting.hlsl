#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

float3 IncomingLight (Surface surface, Light light, float3 worldPos,  float3 lightAttenuation) {
	float3 lightVec = light.directionOrPosition.xyz - worldPos * light.directionOrPosition.w;
	float3 lightDir = normalize(lightVec);
	return saturate(dot(surface.normal, lightDir)) * light.color;
}

float3 IncomingLightCelShaded (Surface surface, Light light, float3 worldPos, float3 lightAttenuation) {

	float3 lightVec = light.directionOrPosition.xyz - worldPos * light.directionOrPosition.w;
	float3 lightDir = normalize(lightVec);
    float NdotL = saturate(dot(surface.normal, lightDir) * light.attenuation);
    
    float distanceSqr = max(dot(lightVec, lightVec), 0.00001);
    
    float rangeFade = dot(lightVec, lightVec) * lightAttenuation.x;
	rangeFade = saturate(1.0 - rangeFade * rangeFade);
	rangeFade *= rangeFade;
	
	float lightIntensity = 0.005f;
	NdotL *= rangeFade/distanceSqr;
		
	if(NdotL > 0.0)
	{
	    if(NdotL < 0.3 / light.range)
	    {
	        lightIntensity = 0.7;
	    }
	    else
	    {
	        lightIntensity = 1.5;
	    }
	}
	
	return light.color * (lightIntensity);
	//return float3(1.0f, 1.0f, 1.0f) * NdotL;
}

float3 GetLighting (Surface surface, Light light, float3 worldPos,  float3 lightAttenuation) {
	return IncomingLightCelShaded(surface, light, worldPos, lightAttenuation) * surface.color;
}

float3 GetLighting (Surface surface, float3 worldPos)
{
	float3 color = 0.0;
	for (int i = 0; i < GetDirectionalLightCount(); i++) {
		color += GetLighting(surface, GetDirectionalLight(i, surface), worldPos, _VisibleLightAttenuations[i]);
	}
	return color;
}

float3 GetIncomingLight (Surface surface) {
	//return GetLighting(surface, GetDirectionalLight());
}

#endif