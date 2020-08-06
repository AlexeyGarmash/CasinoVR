using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInfo : MonoBehaviourPun
{
    /*public bool IsMine = false;
    public int ViewID;
    public bool isGrabbed;
    public ViewSynchronization Synchronization = ViewSynchronization.Off;*/

    private List<Component> _componentsToDelete;

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
        _componentsToDelete = new List<Component>();
        _componentsToDelete.Add(GetComponent<ChipData>());
        _componentsToDelete.Add(GetComponent<GrabbableChip>());

        for (int i = 0; i < _componentsToDelete.Count; i++)
        {
            Destroy(_componentsToDelete[i]);
        }
    }

    /*private void Update()
    {
        if(photonView.IsMine)
            isGrabbed = grabbale.isGrabbed;
    }*/
}
