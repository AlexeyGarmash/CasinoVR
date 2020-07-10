using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats 
{
    public string PlayerNick { get; set; }

    public int AllMoney { get; set; } = 500000;

    public PlayerStats(string nick, int allMoney = 500000)
    {
        PlayerNick = nick;
        AllMoney = allMoney;
    }
}
