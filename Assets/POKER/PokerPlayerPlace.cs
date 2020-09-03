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

    public Action<PokerPlayerPlace, PlaceState> OnPlaceStateChanged;
    public Action<PokerPlayer> OnPlayerReadyChanged;
    public PlaceState PlaceState;
    public PokerPlayer pokerPlayer;
    public PlayerStats ps;
    public PokerTableGame PokerTableGame;
    

    private void Awake()
    {
        PlaceState = PlaceState.Released;
        PokerTableGame = GetComponentInParent<PokerTableGame>();
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

   


}
