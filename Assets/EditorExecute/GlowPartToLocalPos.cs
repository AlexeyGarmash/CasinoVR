using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlowPartToLocalPos : MonoBehaviour
{
    private Vector3 localPos = new Vector3(-0.01f, -0.01f, 0.07f);
    void Start()
    {
        transform.localPosition = localPos;
    }

    
}
