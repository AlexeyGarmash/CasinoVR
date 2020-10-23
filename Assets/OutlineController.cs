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

        foreach(var renderes in GetComponentsInChildren<MeshRenderer>())
            outlineEffect.OutlineLayers[0].Add(renderes.gameObject);

    }

    public void DisableOutlines()
    {
        var outlineEffect = Camera.main.GetComponent<OutlineBuilder>();

        foreach (var renderes in GetComponentsInChildren<MeshRenderer>())
            outlineEffect.OutlineLayers[0].Remove(renderes.gameObject);
    }


}
