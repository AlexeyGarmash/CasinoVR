using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EvenOdd { EVEN, ODD };
public class EvenOddTableCell : TableCell
{
    [SerializeField] private EvenOdd EvenOdd;
    public override bool CheckIsWinCell(WheelCellData wheelCellData)
    {
        switch (EvenOdd)
        {
            case EvenOdd.EVEN:
                return IsEven(wheelCellData.Number);
            case EvenOdd.ODD:
                return IsOdd(wheelCellData.Number);
            default:
                return false;
        }
    }

    private bool IsEven(int number) => number % 2 == 0;
    private bool IsOdd(int number) => number % 2 != 0;
}
