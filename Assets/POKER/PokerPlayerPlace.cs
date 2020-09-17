using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaceState
{
    Taken,
    Released
}

public class PokerPlayerPlace : MonoBehaviour
{
    [SerializeField] private bool optionTakePlace;
    [SerializeField] private bool optionReadyPlay;

    public Action<PokerPlayerPlace, PlaceState> OnPlaceStateChanged { get; set; }
    public Action<PokerPlayer> OnPlayerReadyChanged { get; set; }
    public Action<PokerPlayerPlace> OnPlayerConfirmBetChanged { get; set; }
    public PlaceState PlaceState;
    public PokerPlayer pokerPlayer;
    public PlayerStats ps;
    public PokerTableGame PokerTableGame;
    public PokerCardsField PokerCardsField;
    

    private void Awake()
    {
        PlaceState = PlaceState.Released;
        PokerTableGame = GetComponentInParent<PokerTableGame>();
        PokerCardsField = GetComponentInChildren<PokerCardsField>();
    }

    private void Update()
    {
        if(optionTakePlace)
        {
            InvokeTakePlace();
            optionTakePlace = false;
        }
        if(optionReadyPlay)
        {
            InvokeReadyPlay();
            optionReadyPlay = false;
        }
    }

    public void ConfirmBet()
    {
        SetConfirm(true);
        SetCanBet(false);
        print(string.Format("Poker player {0} confirm bet", pokerPlayer.PokerPlayerType));
        OnPlayerConfirmBetChanged.Invoke(this);
    }

    public void SetCanBet(bool canBet)
    {
        pokerPlayer.CanBet = canBet;
    }

    public void SetConfirm(bool confirm)
    {
        pokerPlayer.ConfirmBet = confirm;
    }

    public void SetPlaceVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void InvokeTakePlace()
    {
        if(PlaceState == PlaceState.Released)
        {
            PlaceState = PlaceState.Taken;
            OnPlaceStateChanged.Invoke(this, PlaceState);
        }
    }

    public void InvokeReadyPlay()
    {
        if(PlaceState == PlaceState.Taken && !pokerPlayer.PlayerReadyPlay)
        {
            pokerPlayer.PlayerReadyPlay = true;
            OnPlayerReadyChanged.Invoke(pokerPlayer);
        }
    }

    public void InvokeReleasePlace()
    {
        PlaceState = PlaceState.Released;
        InvokeNotReadyPlay();
    }

    public void InvokeNotReadyPlay()
    {
        pokerPlayer.PlayerReadyPlay = false;
    }

    public void SetPokerPlayerType(PokerPlayerType pokerPlayerType)
    {
        if(pokerPlayer == null)
        {
            pokerPlayer = new PokerPlayer(pokerPlayerType);
        }
        else
        {
            pokerPlayer.PokerPlayerType = pokerPlayerType;
        }
    }

    public void ReceivePokerCard(GameObject pokerCardObject)
    {
        PokerCardsField.ReceiveSomePokerCard(pokerCardObject);
    } 
   


}
