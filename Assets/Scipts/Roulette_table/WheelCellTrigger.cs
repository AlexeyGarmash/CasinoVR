using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelCellTrigger : MonoBehaviour
{
    [SerializeField] private int num;
    [SerializeField] private BetColor BetColor;

    private WheelCellData _wheelCellData;

    public WheelCellData WheelCellData { get => _wheelCellData; }

    private void Awake()
    {
        _wheelCellData = new WheelCellData(num, BetColor);
    }
}
