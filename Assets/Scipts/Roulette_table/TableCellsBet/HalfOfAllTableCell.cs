using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfOfAllTableCell : TableCell
{
    [SerializeField, Tooltip("1 (1-18) , 2 (19-36)")] private int HalfPartNum;
    public override bool CheckIsWinCell(WheelCellData wheelCellData)
    {
        switch (HalfPartNum)
        {
            case 1: return wheelCellData.Number >= 1 && wheelCellData.Number <= 18;
            case 2: return wheelCellData.Number >= 19 && wheelCellData.Number <= 36;
            default: return false;
        }
    }
}
