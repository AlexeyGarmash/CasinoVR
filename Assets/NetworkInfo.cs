using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInfo : MonoBehaviourPun
{
    public bool IsMine = false;
    public int ViewID;
    public bool isGrabbed;
    public ViewSynchronization Synchronization = ViewSynchronization.Off;

    private OVRGrabbableCustom grabbale;
    private void Awake()
    {
        grabbale = GetComponent<OVRGrabbableCustom>();

        IsMine = photonView.IsMine;
        ViewID = photonView.ViewID;
        Synchronization = ViewSynchronization.Off;
        //Synchronization = photonView.Synchronization;


    }
    private void Update()
    {
        if(photonView.IsMine)
            isGrabbed = grabbale.isGrabbed;
    }
}
