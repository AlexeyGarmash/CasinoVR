using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CustomPoseWithButton : MonoBehaviour
{
    public Transform customPose;
    private OvrAvatar avatar;

    private void Start()
    {
        avatar = GetComponent<OvrAvatar>();
    }

    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            avatar.RightHandCustomPose = customPose;
        }
        else {
            avatar.RightHandCustomPose = null;
        }
    }
}

