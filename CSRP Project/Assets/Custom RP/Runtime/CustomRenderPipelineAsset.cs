using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset {

	[SerializeField] private bool useDynamicBatching = true, useGPUInstancing = true, useSRPBatcher = true;

	[SerializeField] private ShadowSettings shadows = default;

	protected override RenderPipeline CreatePipeline () 
	{
		return new CustomRenderPipeline(useDynamicBatching, useGPUInstancing, useSRPBatcher, shadows);
	}
}