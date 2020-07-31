using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerChipsField : ChipsField
{
    GameObject yellowChipPrefab;
    GameObject redChipPrefab;
    GameObject blueChipPrefab;
    GameObject greenChipPrefab;
    GameObject blackChipPrefab;
    GameObject purpleChipPrefab;

    [SerializeField]
    private float yOffset = 0.0073f;   
    
    [SerializeField]
    private Transform SpawnPos;
 
    private void Start()
    {      
        SetPrefabs();
       
    }


    private void SetPrefabs()
    {
        yellowChipPrefab = ChipUtils.Instance.yellowChipPrefab;
        redChipPrefab = ChipUtils.Instance.redChipPrefab;
        blueChipPrefab = ChipUtils.Instance.blueChipPrefab;
        greenChipPrefab = ChipUtils.Instance.greenChipPrefab;
        blackChipPrefab = ChipUtils.Instance.blackChipPrefab;
        purpleChipPrefab = ChipUtils.Instance.purpleChipPrefab;
    }


    private void InstatiateChip(GameObject prefab, Chips chipsCost, ref int money)   
    {
        Instantiate(prefab, SpawnPos);
        money -= (int)chipsCost;
    }
    public void InstantiateToStackWithColor(Chips chipsCost, ref int money)
    {
             
        switch (chipsCost)
        {
            case Chips.BLACK:
                
                InstatiateChip(blackChipPrefab, Chips.BLACK, ref money);
                break;
            case Chips.BLUE:
                InstatiateChip(blueChipPrefab, Chips.BLUE, ref money);
                break;
            case Chips.GREEN:
                InstatiateChip(greenChipPrefab, Chips.GREEN, ref money);
                break;
            case Chips.PURPLE:
                InstatiateChip(purpleChipPrefab, Chips.PURPLE, ref money);
                break;
            case Chips.RED:
                InstatiateChip(redChipPrefab, Chips.RED, ref money);
                break;
            case Chips.YELLOW:
                InstatiateChip(yellowChipPrefab, Chips.YELLOW, ref money);
                break;
        }       
       
    }
      
}
