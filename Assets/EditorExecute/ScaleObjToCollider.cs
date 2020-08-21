using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleObjToCollider : MonoBehaviour
{
    public Material material;
    MeshRenderer meshRenderer;
    private void Start()
    {
        meshRenderer =  gameObject.AddComponent<MeshRenderer>();
        
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        transform.localScale = boxCollider.size;
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.material = material;
    }
}
