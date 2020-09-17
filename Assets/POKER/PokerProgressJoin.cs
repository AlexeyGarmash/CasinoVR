using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerProgressJoin : PokerProgressButton
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LongClickHand>() != null 
            && inProgress == false 
            && other.GetComponentInParent<PlayerStats>() != null
            && !PokerPlayerPlace.PokerTableGame.IsPlayerOnOtherPlace(other.GetComponentInParent<PlayerStats>()))
        {
            lastCollider = other;
            if (PokerPlayerPlace.PlaceState == PlaceState.Released)
                inProgress = true;

        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LongClickHand>() != null)
        {
            //if (!photonView.IsMine) return;
            ResetProgress();
        }
    }
    protected override void InvokeClickIn()
    {
        photonView.RequestOwnership();
        PokerPlayerPlace.ps = lastCollider.GetComponentInParent<PlayerStats>();
        Join(PokerPlayerPlace.ps.PlayerNick);
        photonView.RPC("JoinPokerGame_RPC", RpcTarget.Others, PokerPlayerPlace.ps.PlayerNick);
    }

    protected override void InvokeClickOut()
    {
        photonView.RequestOwnership();
        Leave();
        photonView.RPC("LeavePokerGame_RPC", RpcTarget.Others);
    }

    private void Join(string nickname)
    {
        clickedAlready = true;
        backImage.texture = positiveTexture;
        buttonText.text = nickname;
        PokerPlayerPlace.InvokeTakePlace();
    }

    private void Leave()
    {
        PokerPlayerPlace.ps = null;
        clickedAlready = false;
        backImage.texture = negativeTexture;
        buttonText.text = "Join";
        PokerPlayerPlace.InvokeReleasePlace();
    }

    [PunRPC]
    public void JoinPokerGame_RPC()
    {
        Join(PokerPlayerPlace.ps.PlayerNick);
    }

    [PunRPC]
    public void LeavePokerGame_RPC()
    {
        Leave();
    }
}
