using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    public PlayerPlace currcentPlace;

    public string PlayerNick;

    public int AllMoney;

    public PlayerStats(string nick, int allMoney = 1000)
    {
        PlayerNick = nick;
        AllMoney = allMoney;
    }

    private void OnDestroy()
    {
        if (currcentPlace != null)
        {
            currcentPlace.GoOutFromPlace();
        }
    }

}
