using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollowRotation : MonoBehaviour
{
    public Transform CenterEye;
    public Transform HeadBone;
    public bool Inverse = false;
    public Vector3 RotationOffset;

    private void Update()
    {
        if (CenterEye != null)
        {
            if (!Inverse)
            {
                HeadBone.rotation = CenterEye.rotation;
            }
            else
            {
                var centEyeRotationNew = Quaternion.Euler(CenterEye.rotation.eulerAngles - RotationOffset);
                HeadBone.rotation = centEyeRotationNew;
            }
        }
    }
}
