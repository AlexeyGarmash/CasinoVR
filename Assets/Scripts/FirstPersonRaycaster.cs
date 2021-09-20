using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonRaycaster : MonoBehaviour
{
    void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, fwd, out raycastHit, 10))
        {
            var longClickPlayReady = raycastHit.collider.gameObject.GetComponent<LongClickReadyPlay>();
            if (longClickPlayReady != null)
            {
                longClickPlayReady.TriggerPlayButton();
                print($"There is {raycastHit.collider.gameObject.name} in front of the object!");
            }
        }
            //
    }
}
