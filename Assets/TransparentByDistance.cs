using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransparentByDistance : MonoBehaviour
{
    [SerializeField]
    Transform target;
    MeshRenderer[] renderers;

    [SerializeField]
    Color color;

    StandardShaderUtils.BlendMode currentMode;
    private void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();

        renderers.ToList().ForEach(r => StandardShaderUtils.ChangeRenderMode(r.material, StandardShaderUtils.BlendMode.Transparent));
        currentMode = StandardShaderUtils.BlendMode.Transparent;
    }

    private void Update()
    {
        var dist = Vector3.Distance(target.position, transform.position);

        if (dist > 0)
        {
            color.a = Mathf.Clamp(dist*3, 0,1);


            if (color.a >= 0.9f && color.a <= 1 && currentMode == StandardShaderUtils.BlendMode.Transparent)
            {
                currentMode = StandardShaderUtils.BlendMode.Opaque;
                renderers.ToList().ForEach(r => StandardShaderUtils.ChangeRenderMode(r.material, StandardShaderUtils.BlendMode.Opaque));
            }
            else if (currentMode == StandardShaderUtils.BlendMode.Opaque && color.a <= 0.9f && color.a >= 0)
            {
                currentMode = StandardShaderUtils.BlendMode.Transparent;
                renderers.ToList().ForEach(r => StandardShaderUtils.ChangeRenderMode(r.material, StandardShaderUtils.BlendMode.Transparent));
            }

            renderers.ToList().ForEach(r => {

                if (color.a <= 0.1f && color.a >= 0)
                    r.gameObject.SetActive(false);
                else if (!r.gameObject.activeSelf)
                    r.gameObject.SetActive(true);

               
                
                
                

                r.material.color = color;
            });
           
        }
    }
}
public static class StandardShaderUtils
{
    public enum BlendMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    public static void ChangeRenderMode(Material standardShaderMaterial, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = -1;
                break;
            case BlendMode.Cutout:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 2450;
                break;
            case BlendMode.Fade:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
            case BlendMode.Transparent:
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
        }

    }
}
