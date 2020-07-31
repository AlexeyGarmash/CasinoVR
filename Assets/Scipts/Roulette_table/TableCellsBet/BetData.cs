using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetData
{
    public PlayerStats PlayerStat { get; set; }
    public int BetValue { get; set; }
    
    public BetData (PlayerStats playerStat, int betValue)
    {
        PlayerStat = playerStat;
        BetValue = betValue;
    }

    public void AddBetValue(int value)
    {
        BetValue += value;
    }
   
    public bool RemoveBetValue(int value)
    {
        if(BetValue - value <= 0)
        {
            return false;
        }
        else
        {
            BetValue -= value;
            return true;
        }
    }

    public void AddWinnedMoney(int koef)
    {
        PlayerStat.AllMoney += BetValue * koef;
    }

}
