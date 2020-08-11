using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SyncController : MonoBehaviourPun
{
    private OVRGrabbableCustom _customGrabbable;
    private bool _isGrabbed = false;
    void Awake()
    {
        _customGrabbable = GetComponent<OVRGrabbableCustom>();
        photonView.Synchronization = ViewSynchronization.Off;
    }

    void Update()
    {
        if(_customGrabbable == null) return;
        if(_customGrabbable.isGrabbed && !_isGrabbed) {
            photonView.RPC("SyncOn_RPC", RpcTarget.AllBuffered);
            print("Sync -> on");
        }
        else if(!_customGrabbable.isGrabbed && _isGrabbed) {
            photonView.RPC("SyncOff_RPC", RpcTarget.AllBuffered);
            print("Sync -> off");
        }
    }


    [PunRPC]
    public void SyncOn_RPC() {
        _isGrabbed = true;
        photonView.Synchronization = ViewSynchronization.UnreliableOnChange;
    }

    [PunRPC]
    public void SyncOff_RPC() {
        _isGrabbed = false;
        photonView.Synchronization = ViewSynchronization.Off;
    }
}
