using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class OutlineController : MonoBehaviour
{

    public void EnableOutlines()
    {
        GetComponentsInChildren<Outline>().ToList().ForEach(o => o.enabled = true);        
    }

    public void DisableOutlines()
    {
        GetComponentsInChildren<Outline>().ToList().ForEach(o => o.enabled = false);
    }


}
