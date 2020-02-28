using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CelShadedRenderPipeline : RenderPipeline
{
    
    ShadowSettings shadowSettings;

    public CelShadedRenderPipeline (ShadowSettings shadows)
    {
        shadowSettings = shadows;
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
        GraphicsSettings.lightsUseLinearIntensity = true;
    }
    
    CameraRenderer renderer = new CameraRenderer();
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (Camera camera in cameras)
        {
            renderer.Render(context, camera, shadowSettings);
        }
    }
}
