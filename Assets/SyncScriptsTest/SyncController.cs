using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SyncController : MonoBehaviourPun
{
    private OVRGrabbableCustom _customGrabbable;

    void Start()
    {
        _customGrabbable = GetComponent<OVRGrabbableCustom>();
    }

    void Update()
    {
        if(_customGrabbable == null) return;
        if(_customGrabbable.isGrabbed) {
            photonView.RPC("SyncOn_RPC", RpcTarget.AllBuffered);
        }
        else {
            photonView.RPC("SyncOff_RPC", RpcTarget.AllBuffered);
        }
    }


    [PunRPC]
    public void SyncOn_RPC() {
        photonView.Synchronization = ViewSynchronization.UnreliableOnChange;
    }

    [PunRPC]
    public void SyncOff_RPC() {
        photonView.Synchronization = ViewSynchronization.Off;
    }
}
