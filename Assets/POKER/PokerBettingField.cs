using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokerBettingField : MonoBehaviourPun
{
    public PokerTableGame pokerGame;
    public Dictionary<string, ChipData> Bets;

    private List<ChipData> lastEnteredChips;

    private bool chipTriggerField;

    private void Start()
    {
        lastEnteredChips = new List<ChipData>();
        Bets = new Dictionary<string, ChipData>();
    }

    private void Update()
    {
        if (chipTriggerField)
        {
            for (int i = 0; i < lastEnteredChips.Count; i++)
            {
                if (!lastEnteredChips[i].gameObject.GetComponent<GrabbableChip>().isGrabbed)
                {
                    Bets[lastEnteredChips[i].Owner] = lastEnteredChips[i];
                    lastEnteredChips.RemoveAt(i);
                }
            }
            switch(pokerGame.pokerGameState)
            {
                case PokerGameState.SmallBlindBetting:
                    pokerGame.SmallBlindMakeBet();
                    break;
                case PokerGameState.BigBlindBetting:
                    pokerGame.BigBlindMakeBet();
                    break;
                case PokerGameState.DefaultPlayersBetting:
                    pokerGame.DefaultPlayerMakeBet();
                    break;
            }
            chipTriggerField = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var chipData = other.GetComponent<ChipData>();
        if (chipData != null)
        {
            lastEnteredChips.Add(chipData);
            chipTriggerField = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var chipData = other.GetComponent<ChipData>();
        if(lastEnteredChips.Contains(chipData))
        {
            lastEnteredChips.Remove(chipData);
        }
    }

    public bool CheckAllAreBetting(PokerPlayerPlace[] pokerPlayerPlaces)
    {
        //bool allBet = true;
        //foreach (var bet in Bets)
        //{

        //}

        return false;
    }
}
