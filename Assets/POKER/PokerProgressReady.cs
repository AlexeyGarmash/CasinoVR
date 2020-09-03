using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerProgressReady : PokerProgressButton
{
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<LongClickHand>() != null
            && inProgress == false
            && PokerPlayerPlace.PlaceState == PlaceState.Taken)
        {
            inProgress = true;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LongClickHand>() != null)
        {
            ResetProgress();
        }
    }

    protected override void InvokeClickIn()
    {
        photonView.RequestOwnership();
        ReadyPlay();
        photonView.RPC("ReadyPlay", RpcTarget.Others);

    }

    protected override void InvokeClickOut()
    {
        photonView.RequestOwnership();
        NotReadyPlay();
        photonView.RPC("NotReadyPlay", RpcTarget.Others);
    }

    private void ReadyPlay()
    {
        clickedAlready = true;
        backImage.texture = positiveTexture;
        buttonText.text = "Playing...";
        PokerPlayerPlace.InvokeReadyPlay();
    }

    private void NotReadyPlay()
    {
        clickedAlready = false;
        backImage.texture = negativeTexture;
        buttonText.text = "Not playing";
        PokerPlayerPlace.InvokeNotReadyPlay();
    }

    [PunRPC]
    public void Ready_RPC()
    {
        ReadyPlay();
    }
    
    [PunRPC]
    public void NotReady_RPC()
    {
        NotReadyPlay();
    }


}
