using UnityEngine;
using UnityEngine.Rendering;

public class Shadows {

    const string bufferName = "Shadows";
    
    int ShadowedDirectionalLightCount;
    const int maxShadowedDirectionalLightCount = 1;
    
    ShadowedDirectionalLight[] shadowedDirectionalLights =
        new ShadowedDirectionalLight[maxShadowedDirectionalLightCount];

    CommandBuffer buffer = new CommandBuffer {
        name = bufferName
    };
    
    struct ShadowedDirectionalLight {
        public int visibleLightIndex;
    }

    ScriptableRenderContext context;

    CullingResults cullingResults;

    ShadowSettings settings;

    public void Setup (
        ScriptableRenderContext context, CullingResults cullingResults,
        ShadowSettings settings
    ) {
        this.context = context;
        this.cullingResults = cullingResults;
        this.settings = settings;
        ShadowedDirectionalLightCount = 0;
    }

    void ExecuteBuffer () {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }
    
    public void ReserveDirectionalShadows(Light light, int visibleLightIndex)
    {
        if (ShadowedDirectionalLightCount < maxShadowedDirectionalLightCount && light.shadows != LightShadows.None && light.shadowStrength > 0f
            && cullingResults.GetShadowCasterBounds(visibleLightIndex, out Bounds b))
        {
            shadowedDirectionalLights[ShadowedDirectionalLightCount++] =
                new ShadowedDirectionalLight {
                    visibleLightIndex = visibleLightIndex
                };
        }
    }
    
}