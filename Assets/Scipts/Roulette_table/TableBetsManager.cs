using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ROULETTE_EVENT {   
    BET_WIN,
    BET_LOST,
    ROULETTE_GAME_START,
    ROULETTE_GAME_END,
    PLAYER_CONNECTED,
    PLAYER_DISCONNCTED
}

public class TableBetsManager : MonoBehaviour
{
    [SerializeField] private TableCell[] TableCells;
    [SerializeField] private RouletteWheelManager RouletteWheelManager;

    public EventManager<ROULETTE_EVENT> rouletteEventManager = new EventManager<ROULETTE_EVENT>();
    
    private void Awake()
    {
        RouletteWheelManager.OnRouletteWheelFinish -= RouletteWheelManager_OnRouletteWheelFinish;
        RouletteWheelManager.OnRouletteWheelFinish += RouletteWheelManager_OnRouletteWheelFinish;
    }

    private void Update()
    {
        //if (OVRInput.GetDown(OVRInput.Button.One))
        //{
        //    RouletteWheelManager.StartSpin();
        //    rouletteEventManager.PostNotification(ROULETTE_EVENT.ROULETTE_GAME_START, this, null);
        //}
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
