using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XrResolutionRenderSettings : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);
        XRSettings.eyeTextureResolutionScale = 1.4f;
    }
}
