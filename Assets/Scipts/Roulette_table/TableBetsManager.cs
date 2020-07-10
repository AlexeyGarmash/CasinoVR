using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBetsManager : MonoBehaviour
{
    [SerializeField] private TableCell[] TableCells;

    [SerializeField] private RouletteWheelManager RouletteWheelManager;

    private void Awake()
    {
        RouletteWheelManager.OnRouletteWheelFinish -= RouletteWheelManager_OnRouletteWheelFinish;
        RouletteWheelManager.OnRouletteWheelFinish += RouletteWheelManager_OnRouletteWheelFinish;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RouletteWheelManager.StartSpin();
        }
    }

    private void RouletteWheelManager_OnRouletteWheelFinish(WheelCellData obj)
    {
        CheckAndNotifyAllCells(obj);
    }

    private void CheckAndNotifyAllCells(WheelCellData obj)
    {
        foreach (var tCell in TableCells)
        {
            tCell.NotifyWinnersOfCell(obj);
        }
    }


}
