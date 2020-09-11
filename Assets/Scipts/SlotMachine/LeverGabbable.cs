using OVRTouchSample;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverGabbable : OVRGrabbableCustom
{

    public Transform handler;

    public override void GrabBegin(OVRGrabberCustom hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
        GetComponentInParent<PhotonView>().RequestOwnership();

        Debug.Log("handle Grabbed");
    }
    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(Vector3.zero, Vector3.zero);

        transform.position = handler.transform.position;
        transform.rotation = handler.transform.rotation;

        Rigidbody rbhandler = handler.GetComponent<Rigidbody>();
        rbhandler.velocity = Vector3.zero;
        rbhandler.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        if(grabbedBy != null)
        if (Vector3.Distance(handler.position, transform.position) > 0.2f)
        {
            grabbedBy.ForceRelease(this);
        }
    }


}
