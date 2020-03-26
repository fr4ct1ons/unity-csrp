using UnityEngine;
using UnityEngine.Rendering;

public class CelShadedRenderPipeline : RenderPipeline {

	private CameraRenderer renderer = new CameraRenderer();

	private bool useDynamicBatching, useGPUInstancing;

	private ShadowSettings shadowSettings;

	private float defaultShadowBrightness, brightnessMultiplier, shadowTreshold;

	public CelShadedRenderPipeline (bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher,ShadowSettings shadowSettings,
		float defaultShadowBrightness, float brightnessMultiplier, float shadowTreshold) 
	{
		this.shadowSettings = shadowSettings;
		this.useDynamicBatching = useDynamicBatching;
		this.useGPUInstancing = useGPUInstancing;
		this.defaultShadowBrightness = defaultShadowBrightness;
		this.brightnessMultiplier = brightnessMultiplier;
		this.shadowTreshold = shadowTreshold;
		GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
		GraphicsSettings.lightsUseLinearIntensity = true;
	}

	protected override void Render (ScriptableRenderContext context, Camera[] cameras) 
	{
		foreach (Camera camera in cameras) 
		{
			renderer.Render(context, camera, useDynamicBatching, useGPUInstancing,shadowSettings, defaultShadowBrightness, brightnessMultiplier, shadowTreshold);
		}
	}
}