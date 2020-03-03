using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting {

    private const string bufferName = "Lighting";
    private const int maxDirLightCount = 200;

    private static int
        dirLightCountId = Shader.PropertyToID("_DirectionalLightCount"),
        dirLightColorsId = Shader.PropertyToID("_DirectionalLightColors"),
        visibleLightDirectionsOrPositionsId = Shader.PropertyToID("_VisibleLightDirectionsOrPositions"),
        visibleLightAttenuationsId = Shader.PropertyToID("_VisibleLightAttenuations"),
        visibleLightRangesId = Shader.PropertyToID("_VisibleLightRanges");
    
    static Vector4[]
        dirLightColors = new Vector4[maxDirLightCount],
        visibleLightDirectionsOrPositions = new Vector4[maxDirLightCount],
        visibleLightAttenuations = new Vector4[maxDirLightCount];
    
    static float[]
        visibleLightIntensities = new float[maxDirLightCount];

    private CommandBuffer buffer = new CommandBuffer {
        name = bufferName
    };

    private CullingResults cullingResults;
    Shadows shadows = new Shadows();
	
    public void Setup (ScriptableRenderContext context, CullingResults cullingResults, ShadowSettings shadowSettings)
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
    
    public void Cleanup () {
        shadows.Cleanup();
    }

    void SetupLights()
    {
        NativeArray<VisibleLight> visibleLights = cullingResults.visibleLights;

        int dirLightCount = 0;
        for (int i = 0; i < visibleLights.Length; i++) {
            VisibleLight visibleLight = visibleLights[i];
            Vector4 attenuation = Vector4.zero;

            visibleLightIntensities[i] = visibleLight.light.range;
            
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
                attenuation.x = 1f / Mathf.Max(visibleLight.range * visibleLight.range, 0.00001f);
                SetupPointLight(dirLightCount++, ref visibleLight);
            }
            visibleLightAttenuations[i] = attenuation;
        }

        buffer.SetGlobalInt(dirLightCountId, visibleLights.Length);
        buffer.SetGlobalVectorArray(dirLightColorsId, dirLightColors);
        buffer.SetGlobalVectorArray(visibleLightDirectionsOrPositionsId, visibleLightDirectionsOrPositions);
        buffer.SetGlobalVectorArray(visibleLightAttenuationsId, visibleLightAttenuations);
        buffer.SetGlobalFloatArray(visibleLightRangesId, visibleLightIntensities);
        /*Light light = RenderSettings.sun;
        buffer.SetGlobalVector(dirLightColorId, light.color.linear * light.intensity);
        buffer.SetGlobalVector(dirLightDirectionId, -light.transform.forward);*/
    }
    
    void SetupDirectionalLight (int index, ref VisibleLight visibleLight)
    {
        dirLightColors[index] = visibleLight.finalColor;
        visibleLightDirectionsOrPositions[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
        shadows.ReserveDirectionalShadows(visibleLight.light, index);
    }

    void SetupPointLight(int index, ref VisibleLight visibleLight)
    {
        dirLightColors[index] = visibleLight.finalColor;
        visibleLightDirectionsOrPositions[index] = visibleLight.localToWorldMatrix.GetColumn(3);
    }
}