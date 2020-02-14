#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

float3 IncomingLight (Surface surface, Light light) {
	return saturate(dot(surface.normal, light.direction)) * light.color;
}

float3 IncomingLightCelShaded (Surface surface, Light light) {

    float NdotL = saturate(dot(surface.normal, light.direction));
	
	float lightIntensity = 0.3;
	if(NdotL > 0)
	{
	    lightIntensity = 1.0;
	}
	return light.color * lightIntensity;
}

float3 GetLighting (Surface surface, Light light) {
	return IncomingLightCelShaded(surface, light) * surface.color;
}

float3 GetIncomingLight (Surface surface) {
	return GetLighting(surface, GetDirectionalLight());
}

#endif