using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneOfWheelGrabbable : OVRGrabbable
{

    
    
    public Transform handler;



    private OVRGrabber savedGrabber;

    protected override void Start()
    {
        base.Start();
        handler = transform.parent;
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        //GetComponentInParent<FortuneHandler>().SetFixedJoint();
        GetComponentInParent<FollowPhysics>().startGrab = true;
        base.GrabBegin(hand, grabPoint);
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        //GetComponentInParent<FortuneHandler>().RemoveFixedJoint();
        GetComponentInParent<FollowPhysics>().startGrab = false;
        base.GrabEnd(linearVelocity, angularVelocity);
        transform.position = handler.position;
        transform.rotation = handler.rotation;

        Rigidbody rbHandler = handler.GetComponent<Rigidbody>();

        if(rbHandler != null)
        {
            rbHandler.velocity = linearVelocity;
            rbHandler.angularVelocity = angularVelocity;
        }
    }

    private void Update()
    {
        if(Vector3.Distance(handler.transform.position, transform.position) >= 1f)
        {
            if (isGrabbed)
            {
                grabbedBy.ForceRelease(this);
            }
        }

        if (isGrabbed)
        {
            savedGrabber = grabbedBy;
            //grabbedBy.transform.parent = handler;
            grabbedBy.transform.parent.position = handler.position;
            grabbedBy.transform.localPosition = Vector3.zero;
        }
        else
        {
            if (savedGrabber != null)
            {
                //savedGrabber.transform.parent = savedGrabber.transform.parent;
                //savedGrabber.transform.localPosition = Vector3.zero;
                savedGrabber = null;
            }
        }
    }
}
