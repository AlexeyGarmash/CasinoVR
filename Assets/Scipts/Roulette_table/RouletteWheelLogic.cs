using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteWheelLogic
{
    private bool _isWheelStarted;
    private WheelCellData _wheelCellData;


    public bool IsPossibleStartGame { get => !_isWheelStarted; }
    public WheelCellData WheelCellData { get => _wheelCellData; set => _wheelCellData = value; }

    public void StartWheel()
    {
        _isWheelStarted = true;
    }

    public void StopWheel()
    {
        _isWheelStarted = false;
    }

    public void ReceiveWheelCellData(WheelCellData wheelCellData)
    {
        _wheelCellData = wheelCellData;
    }

    
}
