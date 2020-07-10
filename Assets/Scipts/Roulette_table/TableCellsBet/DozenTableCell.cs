using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DozenTableCell : TableCell
{
    [SerializeField, Tooltip("1, 2, 3")] private int DozenNum; // 1, 2, 3
    public override bool CheckIsWinCell(WheelCellData wheelCellData)
    {
        switch(DozenNum)
        {
            case 1:
                return NumInRange(wheelCellData.Number, 1, 12);
            case 2:
                return NumInRange(wheelCellData.Number, 13, 24);
            case 3:
                return NumInRange(wheelCellData.Number, 25, 36);
            default:
                return false;
        }
    }

    private bool NumInRange(int num, int from, int to) => num >= from && num <= to;
}
