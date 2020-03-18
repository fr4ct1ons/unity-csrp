using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting {

	private const string bufferName = "Lighting";

	private const int maxDirLightCount = 4;

	private static int
		dirLightCountId = Shader.PropertyToID("_DirectionalLightCount"),
		dirLightColorsId = Shader.PropertyToID("_DirectionalLightColors"),
		dirLightDirectionsId = Shader.PropertyToID("_DirectionalLightDirections"),
		dirLightShadowDataId =
			Shader.PropertyToID("_DirectionalLightShadowData");

	private static Vector4[]
		dirLightColors = new Vector4[maxDirLightCount],
		dirLightDirections = new Vector4[maxDirLightCount],
		dirLightShadowData = new Vector4[maxDirLightCount];

	private CommandBuffer buffer = new CommandBuffer 
	{
		name = bufferName
	};

	private CullingResults cullingResults;

	private Shadows shadows = new Shadows();

	public void Setup (ScriptableRenderContext context, CullingResults cullingResults,ShadowSettings shadowSettings) 
	{
		this.cullingResults = cullingResults;
		buffer.BeginSample(bufferName);
		shadows.Setup(context, cullingResults, shadowSettings);
		SetupLights();
		shadows.Render();
		buffer.EndSample(bufferName);
		context.ExecuteCommandBuffer(buffer);
		buffer.Clear();
	}

	public void Cleanup () 
	{
		shadows.Cleanup();
	}

	private void SetupLights () 
	{
		NativeArray<VisibleLight> visibleLights = cullingResults.visibleLights;
		int dirLightCount = 0;
		
		for (int i = 0; i < visibleLights.Length; i++) 
		{
			VisibleLight visibleLight = visibleLights[i];
			if (visibleLight.lightType == LightType.Directional) 
			{
				SetupDirectionalLight(dirLightCount++, ref visibleLight);
				if (dirLightCount >= maxDirLightCount) 
				{
					break;
				}
			}
		}

		buffer.SetGlobalInt(dirLightCountId, dirLightCount);
		buffer.SetGlobalVectorArray(dirLightColorsId, dirLightColors);
		buffer.SetGlobalVectorArray(dirLightDirectionsId, dirLightDirections);
		buffer.SetGlobalVectorArray(dirLightShadowDataId, dirLightShadowData);
	}

	private void SetupDirectionalLight (int index, ref VisibleLight visibleLight) 
	{
		dirLightColors[index] = visibleLight.finalColor;
		dirLightDirections[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
		dirLightShadowData[index] = shadows.ReserveDirectionalShadows(visibleLight.light, index);
	}
}