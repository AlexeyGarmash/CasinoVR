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

    private List<OVRGrabbableCustom> GetAllGrabbleCom()
    {
        var grabbleChipsComp = new List<OVRGrabbableCustom>();
        foreach (var chip in Chips)
        {
            grabbleChipsComp.Add(chip.GetComponent<OVRGrabbableCustom>());
        }

        return grabbleChipsComp;
    }

}




