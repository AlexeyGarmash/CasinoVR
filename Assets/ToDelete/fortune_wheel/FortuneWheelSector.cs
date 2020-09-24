using OVRTouchSample;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneWheelSector : MonoBehaviour
{
    [SerializeField] private int _cost;

    [SerializeField] private CustomHand trigerredHand;

    [SerializeField] private Vector3 velocityHand;

    private Transform handAnchor;

    private Vector3 lastAnchorPosition;
    private Vector3 startAnchorPosition;
    private WheelOfFortune WheelOfFortune;
    private bool buttonTriggeredYet = false;
    private bool allowRelease = false;
    //[SerializeField] private bool holdButtonPressed;
    //[SerializeField] private bool canHeld;

    public int Cost { get => _cost; }

    public override string ToString()
    {
        return string.Format("Sector {0}", _cost < 0 ? "LOSE" : _cost.ToString() + "$");
    }

    private void Start()
    {
        WheelOfFortune = GetComponentInParent<WheelOfFortune>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var hand = other.GetComponent<CustomHand>();
        if (hand != null)
        {
            trigerredHand = hand;
            handAnchor = trigerredHand.transform.parent;
            lastAnchorPosition = handAnchor.position;
            startAnchorPosition = handAnchor.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var hand = other.GetComponent<CustomHand>();
        if (hand != null && hand == trigerredHand)
        {
            trigerredHand = null;
            handAnchor = null;
            lastAnchorPosition = Vector3.zero;
            startAnchorPosition = Vector3.zero;
        }
    }


    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            if (trigerredHand != null && handAnchor != null)
            {
                var transformHand = trigerredHand.GetComponent<Transform>();
                transformHand.position = transform.position;
                var directionHandAnchor = handAnchor.position - lastAnchorPosition;
                lastAnchorPosition = handAnchor.position;
                //print(directionHandAnchor.normalized);
                ResolveRotation();
                buttonTriggeredYet = true;
            }
            
        }
        

        if(trigerredHand != null && buttonTriggeredYet && allowRelease)
        {
            WheelOfFortune.ReleaseHandAndStartMotor();
            trigerredHand.transform.position = handAnchor.position;
            velocityHand = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.Active);
            buttonTriggeredYet = false;
            allowRelease = false;
        }
    }

    private void ResolveRotation()
    {
        if (Vector3.Distance(startAnchorPosition, lastAnchorPosition) > 0.3f)
        {
            WheelOfFortune.RotateByHand();
            startAnchorPosition = lastAnchorPosition;
            allowRelease = true;
        } else
        {
            
            //WheelOfFortune.ReleaseHandRotation();
        }
    }
}
