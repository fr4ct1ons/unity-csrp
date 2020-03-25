#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

float3 IncomingLight (Surface surface, Light light, float isCelShaded)
{
    float3 lightVector = light.directionOrPosition.xyz - surface.position * light.directionOrPosition.w;
    float3 lightDirection = normalize(lightVector);
    
    float distanceSqr = max(dot(lightVector, lightVector), 0.00001);
	float NdotL = saturate(dot(surface.normal, lightDirection) * light.attenuation);
	
	float rangeFade = dot(lightVector, lightVector) * light.range.x;
	rangeFade = saturate(1.0 - rangeFade * rangeFade);
	rangeFade *= rangeFade;
	
	NdotL *= rangeFade / distanceSqr;

    if(isCelShaded != 0.0)
    {
        float lightIntensity = 0.01;
        if(NdotL > (0.15 * light.range.x + light.range.y))
        {
            lightIntensity = 1;
        }
        return lightIntensity * light.color * surface.color;
	}

	return NdotL * light.color * surface.color;
}

float3 GetLighting (Surface surface, Light light, float isCelShaded) {
	return IncomingLight(surface, light, isCelShaded);
}

float3 GetLighting (Surface surfaceWS, float isCelShaded) {
	ShadowData shadowData = GetShadowData(surfaceWS);
	float3 color = 0.0;
	for (int i = 0; i < GetDirectionalLightCount(); i++) {
		Light light = GetDirectionalLight(i, surfaceWS, shadowData);
		color += GetLighting(surfaceWS, light, isCelShaded);
	}
	return color;
}

#endif