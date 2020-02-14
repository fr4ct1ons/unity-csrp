using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CelShadedRenderPipeline : RenderPipeline
{
 
    public CelShadedRenderPipeline () 
    {
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
    }
    
    CameraRenderer renderer = new CameraRenderer();
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (Camera camera in cameras)
        {
            renderer.Render(context, camera);
        }
    }
}
