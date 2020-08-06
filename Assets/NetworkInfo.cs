using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInfo : MonoBehaviourPun
{
    [SerializeField] private bool _disableRigidbody = true;
    /*public bool IsMine = false;
    public int ViewID;
    public bool isGrabbed;
    public ViewSynchronization Synchronization = ViewSynchronization.Off;*/

    

    //private OVRGrabbableCustom grabbale;
    private void Awake()
    {
        /*grabbale = GetComponent<OVRGrabbableCustom>();

        IsMine = photonView.IsMine;
        ViewID = photonView.ViewID;
        Synchronization = ViewSynchronization.Off;*/
        //Synchronization = photonView.Synchronization;


    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(GetComponent<ChipData>());
            Destroy(GetComponent<GrabbableChip>());
            if (_disableRigidbody)
            {
                Destroy(GetComponent<Rigidbody>());
            }
        }
    }

    /*private void Update()
    {
        if(photonView.IsMine)
            isGrabbed = grabbale.isGrabbed;
    }*/
}
