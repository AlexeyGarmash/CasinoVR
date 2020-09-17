using Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PokerCardResource
{
    public string PathToCardResource { get; set; }
    public CardData CardData { get; set; }

    public PokerCardResource(string path, CardData data)
    {
        PathToCardResource = path;
        CardData = data;
    }
}

public class PokerCardDeck
{
    public int CountOfDecks { get; set; } = 3;
    public List<PokerCardResource> FinalDeck { get; set; }

    public PokerCardDeck()
    {
        FinalDeck = new List<PokerCardResource>();
    }

    public void GeneratePokerDeck()
    {
        GenerateDefaultDeck();
        //not shuffle because runtime rand == shuffle at start
    }

    private void GenerateDefaultDeck()
    {
        FinalDeck.Clear();
        var defDeck = CardUtils.Instance.CreateDefaultDeck();
        for (int i = 0; i < CountOfDecks; i++)
        {
            foreach (var cardData in defDeck)
            {
                string pathToCard = CardUtils.Instance.GetPathToCard(cardData);
                FinalDeck.Add(new PokerCardResource(pathToCard, cardData));
            }
        }
    }

    public PokerCardResource GetCard(int index, bool remove = true)
    {
        if(FinalDeck.Count > 0 && index >= 0 && index < FinalDeck.Count)
        {
            var pokerCardRes = FinalDeck[index];
            if (remove)
            {
                FinalDeck.RemoveAt(index);
            }
            return pokerCardRes;
        }
        return null;
    }
}
