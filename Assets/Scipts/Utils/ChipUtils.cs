
using Cards;
using System.Collections;
using UnityEngine;

public enum Chips { YELLOW = 1, RED = 5, BLUE = 10, GREEN = 25, BLACK = 100, PURPLE = 250 }
class ChipUtils : Singleton<ChipUtils>
{
    
    public GameObject yellowChipPrefab;
    public GameObject redChipPrefab;
    public GameObject blueChipPrefab;
    public GameObject greenChipPrefab;
    public GameObject blackChipPrefab;
    public GameObject purpleChipPrefab;

  

    private void Awake()
    {
        yellowChipPrefab = Resources.Load("Chips/Casino_Chip_Y") as GameObject;
        redChipPrefab = Resources.Load("Chips/Casino_Chip_R") as GameObject;
        blueChipPrefab = Resources.Load("Chips/Casino_Chip_Blue") as GameObject;
        greenChipPrefab = Resources.Load("Chips/Casino_Chip_G") as GameObject;
        blackChipPrefab = Resources.Load("Chips/Casino_Chip_Black") as GameObject;
        purpleChipPrefab = Resources.Load("Chips/Casino_Chip_P") as GameObject;
    }

    public string GetPathToChip(Chips chipcolor)
    {
        switch (chipcolor)
        {
            case Chips.BLACK:
                return "Chips/Casino_Chip_Black";
                break;
            case Chips.BLUE:
                return "Chips/Casino_Chip_Blue";
                break;
            case Chips.GREEN:
                return "Chips/Casino_Chip_G";
                break;
            case Chips.PURPLE:
                return "Chips/Casino_Chip_P";
                break;
            case Chips.RED:
                return "Chips/Casino_Chip_R";
                break;
            case Chips.YELLOW:
                return "Chips/Casino_Chip_Y";
                break;

           
        }
        return null;
    }
   

 }

