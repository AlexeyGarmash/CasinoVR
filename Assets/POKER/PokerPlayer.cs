using Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokerPlayerType
{
    None,
    Diller,
    Default,
    SmallBlind,
    BigBlind
}


public class PokerCards
{
    public List<CardData> Cards { get; set; }

    public int Count { get => Cards.Count; }
    public PokerCards()
    {
        Cards = new List<CardData>();
    }

    public void AddCard(CardData card)
    {
        Cards.Add(card);
    }

    public void RemoveCard(CardData card)
    {
        Cards.Remove(card);
    }

}

[Serializable]
public class PokerPlayer
{
    public PokerPlayerType PokerPlayerType;
    public bool PlayerReadyPlay { get; set; }

    public PokerCards CardsInPlayer;
    
    public bool LeaveCurrentGame { get; set; }

    public PokerPlayer(PokerPlayerType playerType)
    {
        PokerPlayerType = playerType;
        PlayerReadyPlay = false;
        CardsInPlayer = new PokerCards();
    }

    public void ReceiveCard(CardData card)
    {
        if (CardsInPlayer.Count < 2)
        {
            CardsInPlayer.AddCard(card);
        }
    }
}
