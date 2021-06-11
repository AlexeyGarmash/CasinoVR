using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRComponents : MonoBehaviour
{
    public static VRComponents Instance;

    public Transform CenterEye;
    public Transform RightAnchor;
    public Transform LeftAnchor;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
