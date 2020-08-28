using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneHandler : MonoBehaviour
{
    public Rigidbody connectedBody;

    private FixedJoint fixedJoint;

    public void SetFixedJoint()
    {
        /*if (fixedJoint == null)
        {
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = connectedBody;
        }*/
        GetComponentInParent<WheelRotator>().DisableCrazy(this);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void RemoveFixedJoint()
    {
        /*if(fixedJoint != null)
        {
            Destroy(fixedJoint);
            fixedJoint = null;
        }*/
        GetComponent<Rigidbody>().isKinematic = true;
    }

}
