using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PokerGameState
{
    None,
    SmallBlindBetting,
    BigBlindBetting,
    DefaultPlayersBetting,
    PreflopRound,
    FlopRound,
    TurnRound,
    ReaverRound
}
public class PokerTableGame : MonoBehaviour
{
    public PokerBettingField pokerMainBettingField;
    public PokerPlayerInformer pokerPlayerInformer;
    public PokerPlayerPlace[] pokerPlayerPlaces;

    public PokerGameState pokerGameState;

    public int NextVisiblePlace = 0;
    

    private void Start()
    {
        pokerGameState = PokerGameState.None;
        if (pokerPlayerInformer == null) pokerPlayerInformer = GetComponentInChildren<PokerPlayerInformer>();
        ListenPlaceAndPlayersChanges();
        DisableAllPlaces();
        ShowNextPlace();
    }

    private void DisableAllPlaces()
    {
        foreach (var place in pokerPlayerPlaces)
        {
            place.SetPlaceVisibility(false);
        }
    }

    private void ListenPlaceAndPlayersChanges()
    {
        foreach (var place in pokerPlayerPlaces)
        {
            place.OnPlaceStateChanged += OnPlaceChangeState;
            place.OnPlayerReadyChanged += OnPlayerChangeReady;
        }
    }

    

    private void ShowNextPlace()
    {
        pokerPlayerPlaces[NextVisiblePlace].SetPlaceVisibility(true);
        NextVisiblePlace++;
    }

    

    private void OnPlaceChangeState(PokerPlayerPlace invokedPlace, PlaceState placeState)
    {
        if (placeState == PlaceState.Taken) 
        {
            if (NextVisiblePlace == 1)
            {
                invokedPlace.SetPokerPlayerType(PokerPlayerType.Diller);
            }
            else
            if(NextVisiblePlace == 2)
            {
                invokedPlace.SetPokerPlayerType(PokerPlayerType.SmallBlind);
            }
            else
            if (NextVisiblePlace == 3)
            {
                invokedPlace.SetPokerPlayerType(PokerPlayerType.BigBlind);
            }
            else
            {
                invokedPlace.SetPokerPlayerType(PokerPlayerType.Default);
            }
            ShowNextPlace();
        }
    }

    private void OnPlayerChangeReady(PokerPlayer pokerPlayer)
    {
        if(CheckPlayersOnReady())
        {
            print("Next vis place == " + NextVisiblePlace);
            pokerPlayerPlaces[NextVisiblePlace - 1].SetPlaceVisibility(false);
            print("Start poker game");
            StartPokerGame();
        }
    }

    

    private bool CheckPlayersOnReady()
    {
        return pokerPlayerPlaces.ToList().Where(place => place.PlaceState == PlaceState.Taken).All(place => place.pokerPlayer.PlayerReadyPlay);
    }

    public bool IsPlayerOnOtherPlace(PlayerStats playerStats)
    {
        foreach (var place in pokerPlayerPlaces)
        {
            if(place.ps.PlayerNick == playerStats.PlayerNick)
            {
                return true;
            }
        }
        return false;
    }


    #region Start Poker Game

    private void StartPokerGame()
    {
        InformStartBlindBets();
    }

    private void InformStartBlindBets()
    {
        pokerGameState = PokerGameState.SmallBlindBetting;
        pokerPlayerInformer.SetInfo("Small blind is making bet now");
    }

    public void SmallBlindMakeBet()
    {
        pokerGameState = PokerGameState.BigBlindBetting;
        pokerPlayerInformer.SetInfo("Big blind is making bet now");
    }

    public void BigBlindMakeBet()
    {
        pokerGameState = PokerGameState.DefaultPlayersBetting;
        pokerPlayerInformer.SetInfo("Others is making bet now");
    }

    public void DefaultPlayerMakeBet()
    {
        //CheckAllPlayersMakeBets();
    }




    #endregion
}
