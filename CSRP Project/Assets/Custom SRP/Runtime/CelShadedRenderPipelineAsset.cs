using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
public class CelShadedRenderPipelineAsset : RenderPipelineAsset
{
    
    [SerializeField]
    ShadowSettings shadows = default;
    
    protected override RenderPipeline CreatePipeline()
    {
        return new CelShadedRenderPipeline(shadows);
        
    }
}
