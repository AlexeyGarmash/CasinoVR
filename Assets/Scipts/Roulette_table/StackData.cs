using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackData : MonoBehaviour
{
    public int bet;
    public string playerName = "";
    public float currentY;
    public float startY = 0;
    public List<GameObject> Chips = new List<GameObject>();


    public void ClearData()
    {
        foreach (var chip in Chips)       
            Destroy(chip);
        
        Chips.Clear();
        playerName = "";
        currentY = 0;
        bet = 0;
        
    }

}
