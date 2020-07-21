using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
    public string PlayerNick;

    public int AllMoney;

    public PlayerStats(string nick, int allMoney = 500000)
    {
        PlayerNick = nick;
        AllMoney = allMoney;
    }
}
