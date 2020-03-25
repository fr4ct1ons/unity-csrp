using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline {

	private CameraRenderer renderer = new CameraRenderer();

	private bool useDynamicBatching, useGPUInstancing;

	private ShadowSettings shadowSettings;

	private float defaultShadowBrightness;

	public CustomRenderPipeline (bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher,ShadowSettings shadowSettings, float defaultShadowBrightness) 
	{
		this.shadowSettings = shadowSettings;
		this.useDynamicBatching = useDynamicBatching;
		this.useGPUInstancing = useGPUInstancing;
		this.defaultShadowBrightness = defaultShadowBrightness;
		GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
		GraphicsSettings.lightsUseLinearIntensity = true;
	}

	protected override void Render (ScriptableRenderContext context, Camera[] cameras) 
	{
		foreach (Camera camera in cameras) 
		{
			renderer.Render(context, camera, useDynamicBatching, useGPUInstancing,shadowSettings, defaultShadowBrightness);
		}
	}
}