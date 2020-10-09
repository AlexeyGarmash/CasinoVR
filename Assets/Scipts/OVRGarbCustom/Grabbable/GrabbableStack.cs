using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(StackData))]
public class GrabbableStack : OVRGrabbableCustom
{
    
    private PlayerChipsField field;
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
    Transform npcCenter;
    List<Vector3> lastHandPositions = new List<Vector3>();
    List<Vector3> lastStackPositions = new List<Vector3>();

    float maxSevedPositions = 5f;
    [SerializeField]
    float leverDistanceMax = 0.5f;

    private StackData stack;
    protected void Awake()
    {
        //base.Start();

        field = GetComponentInParent<PlayerChipsField>();
        boarderData = GetComponentInParent<FieldBoarderData>();
        stack = GetComponent<StackData>();
        npcCenter = GetComponentInParent<PlayerChipsField>().npcCenter;
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
        handParent = hand.transform.parent;

        hand.transform.rotation = snapOffsetRotation;
        
        transform.parent = hand.grabbleObjSpawnPoint;

        //if (stack.Objects.Count <= 4)
        //{
        //    hand.ForceRelease(this);

        //    stack.Objects.ForEach(chip => hand.ForceGrabBegin(chip.GetComponent<OVRGrabbableCustom>()));

        //    return;
        //}



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

        for (var i = lastStackPositions.Count - 1; i >= 0; i--)
            if (boarderData.ContainsPoint(new Vector2(lastStackPositions[i].x, lastStackPositions[i].z)))
            {
                transform.position = lastStackPositions[i];
                break;
            }

        
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
                grabbedBy.transform.LookAt(npcCenter);
                grabbedBy.transform.position = new Vector3(handParent.transform.position.x, stackYPos, handParent.transform.position.z);
               
                

                transform.localRotation = snapOffset.localRotation;

                if (!boarderData.ContainsPoint(new Vector2(transform.position.x, transform.position.z)))
                {
                    if (lastHandPositions.Count != 0)
                    {
                        grabbedBy.transform.position = lastHandPositions[lastHandPositions.Count - 1];
                    }
                    else
                    {
                        grabbedBy.transform.position = new Vector3(boarderData.CenerMass.x, stackYPos, boarderData.CenerMass.y);
                    }
                }
                else
                {
                    lastHandPositions.Add(new Vector3(grabbedBy.transform.position.x, stackYPos, grabbedBy.transform.position.z));
                    lastStackPositions.Add(new Vector3(transform.position.x, stackYPos, transform.position.z));
                }

                if (maxSevedPositions <= lastHandPositions.Count)
                {
                    lastHandPositions.Remove(lastHandPositions[0]);
                    lastStackPositions.Remove(lastStackPositions[0]);
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
