using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityFx.Outline;

[RequireComponent(typeof(Outline))]
public class OutlineController : MonoBehaviour
{

    public void EnableOutlines()
    {
        var outlineEffect = Camera.main.GetComponent<OutlineBuilder>();

        // This adds layer 0 (if it is not there) and then adds myGo.     
        // Now setup the layer.
        var layer = outlineEffect.OutlineLayers[0];

        layer.OutlineColor = Color.red;
        layer.OutlineWidth = 7;
        layer.OutlineRenderMode = OutlineRenderFlags.Blurred;
        layer.Add(gameObject);
    }

    public void DisableOutlines()
    {
        var outlineEffect = Camera.main.GetComponent<OutlineBuilder>();

        // This adds layer 0 (if it is not there) and then adds myGo.
      
        var layer = outlineEffect.OutlineLayers[0];
        if (layer.Contains(gameObject))
            layer.Remove(gameObject);
    }


}
