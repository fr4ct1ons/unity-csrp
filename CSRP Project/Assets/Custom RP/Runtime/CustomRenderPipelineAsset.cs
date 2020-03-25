using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset {
	
	public bool useDynamicBatching = true, useGPUInstancing = true, useSRPBatcher = true;
	
	public float defaultShadowBrightness = 0.1f;

	public ShadowSettings shadows = default;

	protected override RenderPipeline CreatePipeline () 
	{
		return new CustomRenderPipeline(useDynamicBatching, useGPUInstancing, useSRPBatcher, shadows, defaultShadowBrightness);
	}
}