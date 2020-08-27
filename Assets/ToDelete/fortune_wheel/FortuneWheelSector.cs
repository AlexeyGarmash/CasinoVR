using OVRTouchSample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneWheelSector : MonoBehaviour
{
    [SerializeField] private int _cost;

    [SerializeField] private Hand trigerredHand;

    [SerializeField] private Vector3 velocityHand;

    //[SerializeField] private bool holdButtonPressed;
    //[SerializeField] private bool canHeld;

    public int Cost { get => _cost; }

    public override string ToString()
    {
        return string.Format("Sector {0}", _cost < 0 ? "LOSE" : _cost.ToString() + "$");
    }

    private void OnTriggerEnter(Collider other)
    {
        var hand = other.GetComponent<Hand>();
        if (hand != null)
        {
            trigerredHand = hand;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var hand = other.GetComponent<Hand>();
        if (hand != null && hand == trigerredHand)
        {
            trigerredHand = null;
        }
    }


    private void Update()
    {
        
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            if (trigerredHand != null)
            {
                var transformHand = trigerredHand.GetComponent<Transform>();
                transformHand.position = transform.position;
            }

        }
        else if(trigerredHand != null)
        {
            velocityHand = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.Active);
        }
    }
}
