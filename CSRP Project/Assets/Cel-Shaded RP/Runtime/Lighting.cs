using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting {

	private const string bufferName = "Lighting";

	private const int maxDirLightCount = 200;

	private static int
		dirLightCountId = Shader.PropertyToID("_DirectionalLightCount"),
		dirLightColorsId = Shader.PropertyToID("_DirectionalLightColors"),
		dirLightDirectionsOrPositionsId = Shader.PropertyToID("_DirectionalLightDirectionsOrPositions"),
		dirLightShadowDataId = Shader.PropertyToID("_DirectionalLightShadowData"),
		dirLightRangeId = Shader.PropertyToID("_DirectionalLightRange"),
		defaultShadowBrightnesId = Shader.PropertyToID("lightIntensity"),
		brightnessMultiplierId = Shader.PropertyToID("brightness");
			
	private static Vector4[]
		dirLightColors = new Vector4[maxDirLightCount],
		dirLightDirectionsOrPositions = new Vector4[maxDirLightCount],
		dirLightShadowData = new Vector4[maxDirLightCount],
		dirLightRange = new Vector4[maxDirLightCount];

	private CommandBuffer buffer = new CommandBuffer 
	{
		name = bufferName
	};

	private CullingResults cullingResults;

	private Shadows shadows = new Shadows();

	public void Setup (ScriptableRenderContext context, CullingResults cullingResults,ShadowSettings shadowSettings, float defaultShadowBrightness, float brightnessMultiplier) 
	{
		this.cullingResults = cullingResults;
		buffer.BeginSample(bufferName);
		shadows.Setup(context, cullingResults, shadowSettings);
		SetupLights(defaultShadowBrightness, brightnessMultiplier);
		shadows.Render();
		buffer.EndSample(bufferName);
		context.ExecuteCommandBuffer(buffer);
		buffer.Clear();
	}

	public void Cleanup () 
	{
		shadows.Cleanup();
	}

	private void SetupLights (float defaultShadowBrightness, float brightnessMultiplier) 
	{
		NativeArray<VisibleLight> visibleLights = cullingResults.visibleLights;
		int dirLightCount = 0;
		
		for (int i = 0; i < visibleLights.Length; i++) 
		{
			VisibleLight visibleLight = visibleLights[i];
			if (visibleLight.lightType == LightType.Directional)
			{
				SetupDirectionalLight(dirLightCount++, ref visibleLight);
			}
			else if(visibleLight.lightType == LightType.Point)
			{
				SetupPointLight(dirLightCount++, ref visibleLight);
			}
		}

		buffer.SetGlobalFloat(defaultShadowBrightnesId, defaultShadowBrightness);
		buffer.SetGlobalFloat(brightnessMultiplierId, brightnessMultiplier);
		buffer.SetGlobalInt(dirLightCountId, dirLightCount);
		buffer.SetGlobalVectorArray(dirLightColorsId, dirLightColors);
		buffer.SetGlobalVectorArray(dirLightDirectionsOrPositionsId, dirLightDirectionsOrPositions);
		buffer.SetGlobalVectorArray(dirLightShadowDataId, dirLightShadowData);
		buffer.SetGlobalVectorArray(dirLightRangeId, dirLightRange);
	}

	private void SetupPointLight(int index, ref VisibleLight visibleLight)
	{
		dirLightColors[index] = visibleLight.finalColor;
		dirLightDirectionsOrPositions[index] = visibleLight.localToWorldMatrix.GetColumn(3);
		//dirLightShadowData[index] = shadows.ReservePointShadows(visibleLight.light, index);
		dirLightRange[index] = Vector4.zero;
		dirLightRange[index].x = 1 / Mathf.Max(visibleLight.range * visibleLight.range, 0.00001f);
	}

	private void SetupDirectionalLight (int index, ref VisibleLight visibleLight) 
	{
		dirLightColors[index] = visibleLight.finalColor;
		dirLightDirectionsOrPositions[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
		dirLightShadowData[index] = shadows.ReserveDirectionalShadows(visibleLight.light, index);
		dirLightRange[index] = Vector4.zero;
		dirLightRange[index].y = 0.15f;
	}
}