using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOnlyTableCell : TableCell
{
    [SerializeField] private BetColor BetColor;
    public override bool CheckIsWinCell(WheelCellData wheelCellData) => BetColor == wheelCellData.BetColor;
}
