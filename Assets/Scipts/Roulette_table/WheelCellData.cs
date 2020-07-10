using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelCellData
{
    private int _num;
    public int Number { get => _num; }

    public BetColor BetColor { get; set; }
    public WheelCellData(int num, BetColor betColor)
    {
        _num = num;
        BetColor = betColor;
    }
}
