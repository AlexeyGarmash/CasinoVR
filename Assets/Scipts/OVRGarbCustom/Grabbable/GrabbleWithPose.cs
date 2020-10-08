using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GrabbleWithPose : OVRGrabbable
{
    public Transform leftPose;
    public Transform rightPose;
    public OvrAvatar avatar;

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);

        //if (grabbedBy.m_controller == OVRInput.Controller.LTouch)
        //{
        //    avatar.LeftHandCustomPose = leftPose;

        //}
        //else if (grabbedBy.m_controller == OVRInput.Controller.RTouch) 
        //    avatar.RightHandCustomPose = rightPose;
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        //if (grabbedBy.m_controller == OVRInput.Controller.LTouch)
        //{
        //    avatar.LeftHandCustomPose = null;

        //}
        //else avatar.RightHandCustomPose = null;

        //base.GrabEnd(linearVelocity, angularVelocity);
    }
}

