using Photon.Pun;
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
public class PokerTableGame : MonoBehaviourPun
{
    public Transform cardsToPlayerSpawnPoint;

    //public PokerBettingField pokerMainBettingField;
    public PokerCardsField pokerCardsField;
    public PokerPlayerInformer pokerPlayerInformer;
    public PokerPlayerPlace[] pokerPlayerPlaces;

    public PokerGameState pokerGameState;

    public int NextVisiblePlace = 0;

    public PokerCardDeck MainPokerDeck;
    public int cardsPerPlayer = 2;
    public float waitBetweenCardsToPlayer = 0.1f;
    public float waitBetweenNextPlayerReceiveCards = 1f;
    public float waitBetweenFreeCardsShow = 0.5f;

    private PokerPlayerPlace lastConfirmedBetPlace = null;

    private void Start()
    {
        pokerGameState = PokerGameState.None;
        if (pokerPlayerInformer == null) pokerPlayerInformer = GetComponentInChildren<PokerPlayerInformer>();
        if (pokerCardsField == null) pokerCardsField = GetComponentInChildren<PokerCardsField>();
        MainPokerDeck = new PokerCardDeck();
        GenerateMainCardDeck();
        ListenPlaceAndPlayersChanges();
        DisableAllPlaces();
        ShowNextPlace();
    }

    private void GenerateMainCardDeck()
    {
        MainPokerDeck.GeneratePokerDeck();
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
            place.OnPlayerConfirmBetChanged += OnPlayerConfirmedBet;
        }
    }

    

    private void ShowNextPlace()
    {
        pokerPlayerPlaces[NextVisiblePlace].SetPlaceVisibility(true);
        NextVisiblePlace++;
    }


    private void OnPlayerConfirmedBet(PokerPlayerPlace pokerPlace)
    {
        lastConfirmedBetPlace = pokerPlace;
        ChangeGameStateByBet(pokerPlace);
        if (pokerGameState == PokerGameState.PreflopRound)
        {
            DistributeCards();
        }
        else
        {
            SetBetConfirmed(pokerPlace, true);
        }
        if(CheckAllPlayersConfirmBet())
        {
            if (pokerGameState == PokerGameState.PreflopRound)
            {
                ChangeGameState(PokerGameState.FlopRound);
            }
            if(pokerGameState == PokerGameState.FlopRound)
            {
                ChangeGameState(PokerGameState.TurnRound);
            }
            if(pokerGameState == PokerGameState.TurnRound)
            {
                ChangeGameState(PokerGameState.ReaverRound);
            }
            
            BlockAllPlacesBetAble();
        }
    }

    private void BlockAllPlacesBetAble()
    {
        for (int i = 0; i < pokerPlayerPlaces.Length; i++)
        {
            pokerPlayerPlaces[i].SetCanBet(false);
            pokerPlayerPlaces[i].SetConfirm(false);
        }
    }



    private void DistributeCards()
    {
        //some coroutine here
        if(photonView.IsMine)
        {
            StartCoroutine(StartDistributeCards());
        }

    }

    private IEnumerator StartDistributeCards()
    {
        for (int i = 0; i < pokerPlayerPlaces.Length; i++)
        {
            DistributeSingleCard(i);
            yield return new WaitForSeconds(waitBetweenCardsToPlayer);
            DistributeSingleCard(i);
            yield return new WaitForSeconds(waitBetweenNextPlayerReceiveCards);
        }

        AllowNextBetsAfterDistributeCards();
    }


    private void DistributeSingleCard(int placeIndex)
    {
        int randomIndex = UnityEngine.Random.Range(0, MainPokerDeck.FinalDeck.Count);
        //in photonView
        var card = MainPokerDeck.GetCard(randomIndex);
        var cardGameObject = (GameObject)Instantiate(Resources.Load(card.PathToCardResource), cardsToPlayerSpawnPoint.position, Quaternion.Euler(Vector3.zero));
        if (placeIndex >= 0 && placeIndex < pokerPlayerPlaces.Length)
        {
            pokerPlayerPlaces[placeIndex].ReceivePokerCard(cardGameObject);
        }
    }


    private void AllowNextBetsAfterDistributeCards()
    {
        for (int i = 0; i < pokerPlayerPlaces.Length; i++)
        {
            if (lastConfirmedBetPlace == pokerPlayerPlaces[i])
            {
                if (i < pokerPlayerPlaces.Length - 1)
                {
                    SetBetAble(pokerPlayerPlaces[i + 1], true);
                }
            }
        }
    }

    private void ChangeGameStateByBet(PokerPlayerPlace pokerPlace)
    {
        switch(pokerPlace.pokerPlayer.PokerPlayerType)
        {
            case PokerPlayerType.SmallBlind:
                ChangeGameState(PokerGameState.BigBlindBetting);
                break;
            case PokerPlayerType.BigBlind:
                ChangeGameState(PokerGameState.PreflopRound);
                break;
        }
    }

    private void ChangeGameState(PokerGameState toGameState)
    {
        pokerGameState = toGameState;

        switch(pokerGameState)
        {
            case PokerGameState.SmallBlindBetting:
                pokerPlayerInformer.SetInfo("Small blind is betting now!");
                break;
            case PokerGameState.BigBlindBetting:
                pokerPlayerInformer.SetInfo("Big blind is betting now!");
                break;
            case PokerGameState.DefaultPlayersBetting:
                pokerPlayerInformer.SetInfo("Default player betting");
                break;
            case PokerGameState.PreflopRound:
                pokerPlayerInformer.SetInfo("Preflop round started");
                break;
            case PokerGameState.FlopRound:
                pokerPlayerInformer.SetInfo("Flop round started");
                if (photonView.IsMine)
                {
                    StartDistributeThreeFreedomCards();
                }
                break;
            case PokerGameState.TurnRound:
                pokerPlayerInformer.SetInfo("Turn round started");
                break;
            case PokerGameState.ReaverRound:
                pokerPlayerInformer.SetInfo("Reaver roung started");
                break;
        }
    }

    private void StartDistributeThreeFreedomCards()
    {
        StartCoroutine(StartShowFreeCardsFlopRound(3));
    }

    private IEnumerator StartShowFreeCardsFlopRound(int cardToShowCount)//3, 1, 1 free cards
    {
        for (int i = 0; i < cardToShowCount; i++)
        {
            ShowFreeCard();
            yield return new WaitForSeconds(waitBetweenFreeCardsShow);
        }
    }

    private void ShowFreeCard()
    {
        int randomCardIndex = UnityEngine.Random.Range(0, MainPokerDeck.FinalDeck.Count);
        //photonView
        var cardRes = MainPokerDeck.GetCard(randomCardIndex);
        var cardGameObject = (GameObject)Instantiate(Resources.Load(cardRes.PathToCardResource), cardsToPlayerSpawnPoint.position, Quaternion.identity);
        pokerCardsField.ReceiveSomePokerCard(cardGameObject);
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

    private bool CheckAllPlayersConfirmBet()
    {
        return pokerPlayerPlaces.ToList().Where(place => place.PlaceState == PlaceState.Taken && place.pokerPlayer.PlayerReadyPlay).All(place => place.pokerPlayer.ConfirmBet);
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
        SetBetAble(PokerPlayerType.SmallBlind, true);
        pokerPlayerInformer.SetInfo("Small blind is making bet now");
    }

    

    private void SetBetAble(PokerPlayerType pokerPlayerType, bool canBet)
    {
        foreach (var place in pokerPlayerPlaces)
        {
            if(place.pokerPlayer.PokerPlayerType == pokerPlayerType && !place.pokerPlayer.ConfirmBet)
            {
                place.SetCanBet(canBet);
            }
            else
            {
                place.SetCanBet(!canBet);
            }
            //place.SetConfirm(false);
        }
    }

    private void SetBetAble(PokerPlayerPlace playerPlace, bool canBet)
    {
        if(playerPlace != null)
        {
            playerPlace.SetCanBet(canBet);
        }
    }

    private void SetBetConfirmed(PokerPlayerPlace pokerPlace, bool confirmedBet)
    {
        for (int i = 0; i < pokerPlayerPlaces.Length; i++)
        {
            if(pokerPlayerPlaces[i] == pokerPlace)
            {
                if(i != pokerPlayerPlaces.Length - 1)
                {
                    pokerPlayerPlaces[i + 1].SetCanBet(true);
                    pokerPlayerPlaces[i + 1].SetConfirm(false);
                }
                break;
            }
        }
    }




    #endregion
}
