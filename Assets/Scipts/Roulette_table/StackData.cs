using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackData : MonoBehaviour
{
    
    public string playerName = "";
    public float currentY;   
    public List<GameObject> Objects = new List<GameObject>();
    
    public virtual void ClearData()
    {
        foreach (var chip in Objects)       
            Destroy(chip);
        
        Objects.Clear();
        playerName = "";
        currentY = 0;      
        
    }

   

}




