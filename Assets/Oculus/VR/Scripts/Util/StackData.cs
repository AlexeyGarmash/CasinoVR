using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackData : MonoBehaviour
{
    
    public string playerName = "";
    public float currentY;   
    public List<GameObject> Chips = new List<GameObject>();
    
    public virtual void ClearData()
    {
        foreach (var chip in Chips)       
            Destroy(chip);
        
        Chips.Clear();
        playerName = "";
        currentY = 0;      
        
    }

    

}




