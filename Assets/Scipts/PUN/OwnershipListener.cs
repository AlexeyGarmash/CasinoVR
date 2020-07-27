using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnershipListener : MonoBehaviourPun, IPunOwnershipCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("OnOwnershipRequest(): Player " + requestingPlayer + " requests ownership of: " + targetView + ".");
        targetView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("OnOwnershipTransfer(): Player changes from" + previousOwner + " transfer ownership of: " + targetView + ".");
    }
}
