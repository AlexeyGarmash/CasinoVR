using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexFingerChipSelection : MonoBehaviour
{
    OVRGrabberCustom grabber;
    public Collider fingerCollider;
    void Start()
    {
        fingerCollider = GetComponent<Collider>();
        grabber = GetComponentInParent<OVRGrabberCustom>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        var grabbableChip = other.GetComponent<GrabbableChip>();
        if (grabbableChip)
        {
            if (grabber.selectedChips.Contains(grabbableChip))
                return;

            if (grabber.max_grabbed_obj == grabber.selectedChips.Count)
            {
                grabber.selectedChips[0].GetComponent<OutlineController>().DisableOutlines();
                grabber.selectedChips.Remove(grabber.selectedChips[0]);
            }

            other.GetComponent<OutlineController>().EnableOutlines();
            grabber.selectedChips.Add(grabbableChip);
            Debug.Log(grabbableChip.gameObject.name);
        }
    }

    public void ActivateFinger(bool activate)
    {
        _activated = activate;
        fingerCollider.enabled = activate;
        fingerCollider.isTrigger = activate;
    }
    bool _activated;
    public bool IsFingerActivated => _activated;



}
