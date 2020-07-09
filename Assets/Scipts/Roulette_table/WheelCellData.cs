using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelCellData
{
    private int _num;
    public int Number { get => _num; }

    public WheelCellData(int num)
    {
        this._num = num;
    }
}
