using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowPart : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    [SerializeField] private Material _allowMaterial;
    [SerializeField] private Material _denyMaterial;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    public void GlowCell(bool glow, bool allow)
    {
        meshRenderer.material = allow ? _allowMaterial : _denyMaterial;
        meshRenderer.enabled = glow;
    }

}
