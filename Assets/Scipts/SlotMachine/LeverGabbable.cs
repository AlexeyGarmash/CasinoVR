using OVRTouchSample;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverGabbable : OVRGrabbableCustom
{

    public Transform handler;
    private Rigidbody handlerRB;
  
    Vector3 handlePosition;

    Quaternion handRotation;
    Quaternion snapOffsetRotation;
    Quaternion handleRotation;

    [SerializeField]
    float leverDistanceMax = 0.1f;
    protected override void Start()
    {
        base.Start();
        handlePosition = handler.localPosition;
        handleRotation = handler.rotation;

        handlerRB = handler.GetComponent<Rigidbody>();
    }

    
    public override void GrabBegin(OVRGrabberCustom hand, Collider grabPoint)
    {

        handRotation = hand.transform.rotation;

        base.GrabBegin(hand, grabPoint);
        GetComponentInParent<PhotonView>().RequestOwnership();

      
        handRotation = hand.transform.localRotation;

        hand.transform.rotation = snapOffsetRotation;

        transform.parent = hand.transform.parent;
      
        hand.transform.parent = handlerRB.transform;
       
    }
    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        handler.localPosition = handlePosition;
       
        handler.rotation = handleRotation;

        var gb = grabbedBy;

        

        gb.transform.parent = transform.parent;
        transform.parent = handler.transform;

        gb.transform.localPosition = Vector3.zero;
        gb.transform.localRotation = handRotation;

        base.GrabEnd(Vector3.zero, Vector3.zero);

        transform.parent = handler;
        transform.position = handler.transform.position;
        transform.rotation = handler.transform.rotation;
       

        Rigidbody rbhandler = handler.GetComponent<Rigidbody>();
        rbhandler.velocity = Vector3.zero;
        rbhandler.angularVelocity = Vector3.zero;

      
    }

    private void LateUpdate()
    {
        
        if (isGrabbed)
        {
            if (Vector3.Distance(handler.position, transform.position) > leverDistanceMax)
            {
                grabbedBy.ForceRelease(this);
            }
            else
            {
                grabbedBy.transform.position = snapOffset.position;
                grabbedBy.transform.rotation = snapOffset.rotation;
            }


        }

    }


}
