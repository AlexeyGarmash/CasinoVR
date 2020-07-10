using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingField : MonoBehaviour
{

   
    [SerializeField]
    StackData[] BetStacks;

    private TableCell tableCell;

    private void Awake()
    {
        tableCell = GetComponent<TableCell>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        var chip = other.gameObject.GetComponent<ChipData>();
        if (chip != null && tableCell != null)
        {
            if(ChipsUtils.Instance.MagnetizeChip(other.gameObject, BetStacks))
            {
                tableCell.ReceiveBetData(new BetData(new PlayerStats(chip.player), chip.Cost));
            }
        }
    }
   
    private void OnTriggerStay(Collider other)
    {
       
        var chip = other.gameObject.GetComponent<ChipData>();
        if (chip != null && tableCell != null)
        {
            if (ChipsUtils.Instance.ExtractionChip(other.gameObject, BetStacks))
            {
                tableCell.RemoveBetData(new BetData(new PlayerStats(chip.player), chip.Cost));
            }
        }
    }

}
