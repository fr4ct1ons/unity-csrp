Shader "Cel-Shaded RP/Lit" {
    Properties
    {
        _BaseColor("Color", Color) = (0.5, 0.5, 0.5, 1.0)
    }
    
    SubShader{
    
        Pass
        {
            
            Tags{
                "LightMode" = "CustomLit"
            }
        
            HLSLPROGRAM
			#pragma vertex LitPassVertex
			#pragma fragment LitPassFragment
			#include "LitPass.hlsl"
			ENDHLSL
        }
        
    }
}