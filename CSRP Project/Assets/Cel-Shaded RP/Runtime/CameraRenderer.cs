using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer {

	private const string bufferName = "Render Camera";

	private static ShaderTagId
		unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit"),
		litShaderTagId = new ShaderTagId("CustomLit");

	private static int
		defaultShadowBrightnesId = Shader.PropertyToID("lightIntensity"),
		brightnessMultiplierId = Shader.PropertyToID("brightness"), 
		shadowTresholdId = Shader.PropertyToID("shadowTreshold");

	private CommandBuffer buffer = new CommandBuffer {
		name = bufferName
	};

	private ScriptableRenderContext context;

	private Camera camera;

	private CullingResults cullingResults;

	private Lighting lighting = new Lighting();

	public void Render (ScriptableRenderContext context, Camera camera,bool useDynamicBatching, bool useGPUInstancing,ShadowSettings shadowSettings, 
		float defaultShadowBrightness, float brightnessMultiplier, float shadowTreshold) 
	{
		this.context = context;
		this.camera = camera;

		PrepareBuffer();
		PrepareForSceneWindow();
		if (!Cull(shadowSettings.maxDistance)) 
		{
			return;
		}
		
		buffer.BeginSample(SampleName);
		ExecuteBuffer();
		lighting.Setup(context, cullingResults, shadowSettings, shadowTreshold);
		buffer.SetGlobalFloat(defaultShadowBrightnesId, defaultShadowBrightness);
		buffer.SetGlobalFloat(brightnessMultiplierId, brightnessMultiplier);
		buffer.SetGlobalFloat(shadowTresholdId, shadowTreshold);
		buffer.EndSample(SampleName);
		Setup();
		DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
		DrawUnsupportedShaders();
		DrawGizmos();
		lighting.Cleanup();
		Submit();
	}

	private bool Cull (float maxShadowDistance) 
	{
		if (camera.TryGetCullingParameters(out ScriptableCullingParameters p)) 
		{
			p.shadowDistance = Mathf.Min(maxShadowDistance, camera.farClipPlane);
			cullingResults = context.Cull(ref p);
			return true;
		}
		return false;
	}

	private void Setup () 
	{
		context.SetupCameraProperties(camera);
		CameraClearFlags flags = camera.clearFlags;
		buffer.ClearRenderTarget(
			flags <= CameraClearFlags.Depth,
			flags == CameraClearFlags.Color,
			flags == CameraClearFlags.Color ?
				camera.backgroundColor.linear : Color.clear
		);
		buffer.BeginSample(SampleName);
		ExecuteBuffer();
	}

	private void Submit () 
	{
		buffer.EndSample(SampleName);
		ExecuteBuffer();
		context.Submit();
	}

	private void ExecuteBuffer () 
	{
		context.ExecuteCommandBuffer(buffer);
		buffer.Clear();
	}

	private void DrawVisibleGeometry (bool useDynamicBatching, bool useGPUInstancing) 
	{
		var sortingSettings = new SortingSettings(camera) 
		{
			criteria = SortingCriteria.CommonOpaque
		};
		
		var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings) 
		{
			enableDynamicBatching = useDynamicBatching,
			enableInstancing = useGPUInstancing
		};
		
		drawingSettings.SetShaderPassName(1, litShaderTagId);

		var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

		context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

		context.DrawSkybox(camera);

		sortingSettings.criteria = SortingCriteria.CommonTransparent;
		drawingSettings.sortingSettings = sortingSettings;
		filteringSettings.renderQueueRange = RenderQueueRange.transparent;

		context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
	}
}