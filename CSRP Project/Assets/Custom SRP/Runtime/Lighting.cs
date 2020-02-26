using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting {

    private const string bufferName = "Lighting";
    private const int maxDirLightCount = 200;
    
    private static int
        dirLightCountId = Shader.PropertyToID("_DirectionalLightCount"),
        dirLightColorsId = Shader.PropertyToID("_DirectionalLightColors"),
        visibleLightDirectionsOrPositionsId = Shader.PropertyToID("_VisibleLightDirectionsOrPositions");
    
    static Vector4[]
        dirLightColors = new Vector4[maxDirLightCount],
        visibleLightDirectionsOrPositions = new Vector4[maxDirLightCount];

    private CommandBuffer buffer = new CommandBuffer {
        name = bufferName
    };

    private CullingResults cullingResults;
	
    public void Setup (ScriptableRenderContext context, CullingResults cullingResults)
    {
        this.cullingResults = cullingResults;
        buffer.BeginSample(bufferName);
        SetupLights();
        buffer.EndSample(bufferName);
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    void SetupLights()
    {
        NativeArray<VisibleLight> visibleLights = cullingResults.visibleLights;

        int dirLightCount = 0;
        for (int i = 0; i < visibleLights.Length; i++) {
            VisibleLight visibleLight = visibleLights[i];
            if (visibleLight.lightType == LightType.Directional)
            {
                SetupDirectionalLight(dirLightCount++, ref visibleLight);
                if (dirLightCount >= maxDirLightCount)
                {
                    break;
                }
            }
            else if(visibleLight.lightType == LightType.Point)
            {
                SetupPointLight(dirLightCount++, ref visibleLight);
            }
        }

        buffer.SetGlobalInt(dirLightCountId, visibleLights.Length);
        buffer.SetGlobalVectorArray(dirLightColorsId, dirLightColors);
        buffer.SetGlobalVectorArray(visibleLightDirectionsOrPositionsId, visibleLightDirectionsOrPositions);
        /*Light light = RenderSettings.sun;
        buffer.SetGlobalVector(dirLightColorId, light.color.linear * light.intensity);
        buffer.SetGlobalVector(dirLightDirectionId, -light.transform.forward);*/
    }
    
    void SetupDirectionalLight (int index, ref VisibleLight visibleLight)
    {
        dirLightColors[index] = visibleLight.finalColor;
        visibleLightDirectionsOrPositions[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
    }

    void SetupPointLight(int index, ref VisibleLight visibleLight)
    {
        visibleLightDirectionsOrPositions[index] = visibleLight.localToWorldMatrix.GetColumn(3);
    }
}