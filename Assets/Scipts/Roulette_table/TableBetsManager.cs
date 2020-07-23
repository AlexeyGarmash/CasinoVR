using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ROULETTE_EVENT {   
    BET_WIN,
    BET_LOST,
    ROULETTE_GAME_START,
    CHECK_WINNERS,
    ROULETTE_SPIN_END,
    ROULETTE_GAME_END,
    PLAYER_CONNECTED,
    PLAYER_DISCONNCTED
}
public class RouletteRandom {

    public int RndNext()
    {
        return Random.Range(0, 36);
    }
}
public class TableBetsManager : MonoBehaviour, IListener<ROULETTE_EVENT>
{
    [SerializeField] private TableCell[] TableCells;
    [SerializeField] private RouletteWheelManager RouletteWheelManager;
    [SerializeField] private PlayerPlace[] plyers;

    private RouletteRandom random = new RouletteRandom();
    public EventManager<ROULETTE_EVENT> rouletteEventManager = new EventManager<ROULETTE_EVENT>();
    
    private void Awake()
    {
        rouletteEventManager.AddListener(ROULETTE_EVENT.CHECK_WINNERS, this);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            RouletteWheelManager.StartSpin(random.RndNext());
            Debug.Log("ROULETTE_GAME_START");
            rouletteEventManager.PostNotification(ROULETTE_EVENT.ROULETTE_GAME_START, this, null);
        }
    }

    //private void RouletteWheelManager_OnRouletteWheelFinish(WheelCellData obj)
    //{
    //    CheckAndNotifyAllCells(obj);
    //}

    private void CheckAndNotifyAllCells(WheelCellData obj)
    {

        foreach (PlayerPlace player in plyers)      
        {
            int win = 0; 
            foreach (var tCell in TableCells)
            {
                if (tCell.CheckIsWinCell(obj))
                {
                    foreach (var betData in tCell.BetsData)
                    {
                        print(string.Format("Player {0} win {1}", betData.PlayerStat.PlayerNick, betData.BetValue * tCell.WinKoeff));
                        if (player.ps.PlayerNick.Equals(betData.PlayerStat.PlayerNick))
                        {
                            win += (tCell.WinKoeff * betData.BetValue) - betData.BetValue;
                            print(string.Format("Player {0} win {1}", betData.PlayerStat.PlayerNick, betData.BetValue * tCell.WinKoeff));

                        }

                    }
                }
                else
                {
                    foreach (var betData in tCell.BetsData)
                    {
                        if (player.ps.PlayerNick.Equals(betData.PlayerStat.PlayerNick))
                        {
                            win -= betData.BetValue;
                            print(string.Format("Player {0} Lose {1}", betData.PlayerStat.PlayerNick, betData.BetValue));
                        }
                    }
                }
            }

            player.ps.AllMoney += win;
        }


    }
 

    public void OnEvent(ROULETTE_EVENT Event_type, Component Sender, params object[] Param)
    {
        switch (Event_type)
        {
            case ROULETTE_EVENT.CHECK_WINNERS:
               
                CheckAndNotifyAllCells((WheelCellData)Param[0]);
                Debug.Log("ROULETTE_GAME_END");
                rouletteEventManager.PostNotification(ROULETTE_EVENT.ROULETTE_GAME_END, this);
                break;
        }
    }
}
