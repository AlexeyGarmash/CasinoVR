using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonSyncCrontroller : MonoBehaviourPun
{


    public bool IsPhotonSync { get => photonView.Synchronization != ViewSynchronization.Off; }
    public void SyncOff_Photon()
    {
        
        Debug.Log("SyncOff_RPC");
        photonView.RPC("SyncOff_RPC", RpcTarget.All);
        
    }
    public void SyncOn_Photon()
    {
       
        Debug.Log("SyncOn_RPC");
        photonView.RPC("SyncOn_RPC", RpcTarget.All);
        
    }

    public void SetOwner_Photon()
    {

    }

    [PunRPC]
    public void SetOwner_RPC()
    {
        photonView.Synchronization = ViewSynchronization.Off;

    }
    [PunRPC]
    public void SyncOff_RPC()
    {
        photonView.Synchronization = ViewSynchronization.Off;

    }
    [PunRPC]
    public void SyncOn_RPC()
    {
        photonView.Synchronization = ViewSynchronization.Unreliable;

    }
}
