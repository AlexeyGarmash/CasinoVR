using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BetData
{
    public string PlayerStat { get; set; }
    public int BetValue { get; set; }
    
    public BetData (string playerStat, int betValue)
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

    /*public void AddWinnedMoney(int koef)
    {
        PlayerStat.AllMoney += BetValue * koef;
    }*/

    public override string ToString()
    {
        return string.Format("Bet by {0} is = {1}", PlayerStat, BetValue);
    }

}
