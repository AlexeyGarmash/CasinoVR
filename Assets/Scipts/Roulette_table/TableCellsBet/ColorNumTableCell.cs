using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BetColor { RED, BLACK, GREEN };
public class ColorNumTableCell : TableCell
{
    [SerializeField] private int Number;
    [SerializeField] private BetColor Color;



    private void Start()
    {
        
    }

    public override bool CheckIsWinCell(WheelCellData wheelCellData) => wheelCellData.BetColor == Color && Number == wheelCellData.Number;

    
}
