using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollowRotation : MonoBehaviour
{
    public Transform CenterEye;
    public Transform HeadBone;



    private void Update()
    {
        if (CenterEye != null)
        {
            HeadBone.rotation = CenterEye.rotation;
        }
    }
}
