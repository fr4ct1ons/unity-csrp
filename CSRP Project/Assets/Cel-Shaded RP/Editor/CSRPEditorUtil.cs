using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CSRPEditorUtil : MonoBehaviour
{
    
    [MenuItem("CSRP/UpgradeMaterialsToCSRPLit")]
    static void ConvertStandardToCsrp()
    {
        Debug.Log("Upgrading materials");
        var materials = Resources.FindObjectsOfTypeAll<Material>();
        foreach (Material material in materials)
        {
            //Cel-Shaded RP/Lit
            Shader std = Shader.Find("Standard");
            Shader cs = Shader.Find("Cel-Shaded RP/Lit");
            if (material.shader.Equals(std) && material.name != "Default-Material")
            {
                Debug.Log(material.name + ", " + material.shader.name);
                var baseTex = material.GetTexture("_MainTex");
                var baseColor = material.GetColor("_Color");
                var baseNormal = material.GetTexture("_BumpMap");
                var cutoff = material.GetFloat("_Cutoff");
                var srcBlend = material.GetFloat("_SrcBlend");
                var dstBlend = material.GetFloat("_DstBlend");
                var ZWrite = material.GetFloat("_ZWrite");
                material.shader = cs;
                material.SetTexture("_BaseMap", baseTex);
                material.SetColor("_BaseColor", baseColor);
                material.SetTexture("_NormalMap", baseNormal);
                material.SetFloat("_Cutoff", cutoff);
                material.SetFloat("_SrcBlend", srcBlend);
                material.SetFloat("_DstBlend", dstBlend);
                material.SetFloat("_ZWrite", ZWrite);
            }
        }
    }
    
}
