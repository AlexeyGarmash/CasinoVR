using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNetworkInfo : MonoBehaviourPun
{
    public bool IsMine = false;
    public string Owner;
    public int ViewID;
    public bool isGrabbed;
    public bool InAnimation;
   

    private OVRGrabbableCustom grabbale;

    private void Awake()
    {
        grabbale = GetComponent<OVRGrabbableCustom>();

        IsMine = photonView.IsMine;
        ViewID = photonView.ViewID;
    
        //Synchronization = photonView.Synchronization;


    }
    private void Update()
    {
        if(photonView.IsMine)
            isGrabbed = grabbale.isGrabbed;
    }
}
