using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerProgressConfirmBet : PokerProgressButton
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
        if(other.gameObject.GetComponent<LongClickHand>() != null)
        {
            ResetProgress();
        }
    }

    protected override void InvokeClickIn()
    {
        photonView.RequestOwnership();
        
    }

    private void ConfirmBet()
    {
        clickedAlready = true;
        backImage.texture = positiveTexture;
        buttonText.text = "Confirmed";
        PokerPlayerPlace.ConfirmBet();
    }
}
