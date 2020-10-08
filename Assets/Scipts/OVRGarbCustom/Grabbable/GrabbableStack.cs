using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableStack : OVRGrabbableCustom
{
    private Transform handParent;
    private Transform stackParent;

    Quaternion handRotation;
    Quaternion snapOffsetRotation;
    Quaternion handleRotation;

    FieldBoarderData boarderData;

    float stackYPos;

    Vector3 lastPositionHand;
    Quaternion lastRotationHand;
    Vector3 lastPositionStack;
    [SerializeField]
    float leverDistanceMax = 0.1f;

    protected override void Start()
    {
        base.Start();

        boarderData = GetComponentInParent<FieldBoarderData>();

        stackYPos = transform.position.y;
       
    }


    public override void GrabBegin(OVRGrabberCustom hand, Collider grabPoint)
    {
        handleRotation = transform.rotation;
        handRotation = hand.transform.rotation;
        stackParent = transform.parent;
        base.GrabBegin(hand, grabPoint);

        photonView.RequestOwnership();


        handRotation = hand.transform.localRotation;

        hand.transform.rotation = snapOffsetRotation;

        
        handParent = hand.transform.parent;
        transform.parent = hand.grabbleObjSpawnPoint;
    


    

    }
    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        

        var gb = grabbedBy;

        gb.transform.parent = handParent;
      

        gb.transform.localPosition = Vector3.zero;
        gb.transform.localRotation = handRotation;

        base.GrabEnd(Vector3.zero, Vector3.zero);


        GetComponent<Rigidbody>().isKinematic = true;

        transform.parent = stackParent;
        transform.rotation = handleRotation;
        transform.position = lastPositionStack;
    }


  
    private void LateUpdate()
    {

        if (isGrabbed)
        {

            //Vector3 vect1 = Vector3.ProjectOnPlane(grabbedBy.transform.position, Vector3.up);
            //Vector3 vect2 = Vector3.ProjectOnPlane(handParent.transform.position, Vector3.up);

            //if (Vector3.Distance(vect1, vect2) > leverDistanceMax)
            //{
            //    grabbedBy.ForceRelease(this);
            //    return;
            //}

            if (boarderData.ContainsPoint(new Vector2(transform.position.x, transform.position.z)))
            {
                grabbedBy.transform.position = new Vector3(handParent.transform.position.x, stackYPos, handParent.transform.position.z);
                grabbedBy.transform.rotation = Quaternion.identity;

                transform.localRotation = snapOffset.localRotation;


                if (!boarderData.ContainsPoint(new Vector2(transform.position.x, transform.position.z)))
                {
                    grabbedBy.transform.position = lastPositionHand;
                    grabbedBy.transform.rotation = lastRotationHand;

                    return;
                }

                lastPositionHand = grabbedBy.transform.position;
                lastRotationHand = grabbedBy.transform.rotation;
                lastPositionStack = transform.position;
            }
            else
            {
                grabbedBy.transform.position = lastPositionHand;
                grabbedBy.transform.rotation = lastRotationHand;
            }

        }

    }


}
