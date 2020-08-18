using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum ROULETTE_EVENT {   
    BET_WIN,
    BET_LOST,
    ROULETTE_GAME_START,
    CHECK_WINNERS,
    ROULETTE_SPIN_END,
    ROULETTE_GAME_END,
    PLAYER_LEAVE,
    PLAYER_ATTACHED
}
public class RouletteRandom {

    public int RndNext()
    {
        return UnityEngine.Random.Range(0, 36);
    }
}
public class TableBetsManager : MonoBehaviour, IListener<ROULETTE_EVENT>
{
    [SerializeField] private TableCell[] TableCells;
    [SerializeField] private RouletteWheelManager RouletteWheelManager;
    [SerializeField] private PlayerPlace[] plyers;
    [SerializeField] private PlayerInformer _playerInformer;
    [SerializeField] private CroupierNPC _croupierNPC;

    private RouletteRandom random = new RouletteRandom();
    public EventManager<ROULETTE_EVENT> rouletteEventManager = new EventManager<ROULETTE_EVENT>();
    
    private void Awake()
    {
        rouletteEventManager.AddListener(ROULETTE_EVENT.CHECK_WINNERS, this);
    }

    private void Start() {
        _croupierNPC.onCroupierPausedOff += startAnimationRoulette;
        foreach (var place in plyers)
        {
            place.actionReadyOrNot += playerClickReady;
        }

    }

    private void startAnimationRoulette()
    {
        StartSpinAllParts();
    }

    private void playerClickReady(bool isReady, PlayerStats ps) 
    {

        bool allReady = plyers.Where( player => player.ps != null).All(joinedPlayer => joinedPlayer.IsReady);
        if(allReady) {
            print("START SPIN ROULETTE!!!");
            StartAllAnimations();
        } else
        {
            print("not all players are ready");
        }
    }

    public bool checkPlaceTakenYet(PlayerStats pss) {
        PlayerPlace findPlace = null;
        foreach (var place in plyers)
        {
            //print(string.Format("Player {0} joined => {1}", place.ps.name, place.IsPlaceTaken));

            if(place.ps != null && place.ps.PlayerNick == pss.PlayerNick)
            {
                findPlace = place;
                print(string.Format("Player {0} joined => {1}", place.ps.PlayerNick, place.IsPlaceTaken));
                break;
            }
        }

        //PlayerPlace findPlace = plyers.FirstOrDefault(pl => pl.ps != null && pl.ps.name == pss.name);
        
        if(findPlace == null) {
            return false;
        } else {
            return true;
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            
        }
    }

    public void StartAllAnimations()
    {
        _croupierNPC.StartSpinAnimation();
    }

    public void StartSpinAllParts()
    { 
        RouletteWheelManager.StartSpin(random.RndNext());
        Debug.Log("ROULETTE_GAME_START");
        rouletteEventManager.PostNotification(ROULETTE_EVENT.ROULETTE_GAME_START, this, null);
        _playerInformer.HideMessage();
    }

    public void LeaveTable()
    {
        _playerInformer.HideMessage();
        foreach (var tCell in TableCells)
        {
            tCell.ResetBets(PhotonNetwork.LocalPlayer.NickName);
        }
    }

    private void CheckAndNotifyAllCells(WheelCellData obj)
    {
        var player = plyers.ToList().Find(x => x.ps.PlayerNick == PhotonNetwork.LocalPlayer.NickName);
        if (player == null) return;
            int win = 0;
        print(string.Format("Table cells count = {0}", TableCells.Length));
        
        foreach (var tCell in TableCells)
            {
            print(string.Format("Bets count at table cell {0} = {1}", tCell.name, tCell.BetsData.Count));
                if (tCell.CheckIsWinCell(obj))
                {
                    foreach (var betData in tCell.BetsData)
                    {
                        print(string.Format("Check WINN table cell {0} bet of player {1}", tCell.name, betData.PlayerStat));
                        if (player.ps.PlayerNick.Equals(betData.PlayerStat))
                        {

                            win += (tCell.WinKoeff * betData.BetValue);
                            print(string.Format("Player {0} win {1}", betData.PlayerStat, betData.BetValue * tCell.WinKoeff));

                            //var localWin = (tCell.WinKoeff * betData.BetValue) - betData.BetValue;
                            //win += localWin;
                            print(string.Format("Player {0} win {1}", betData.PlayerStat, betData.BetValue * tCell.WinKoeff));
                            tCell.BetsData.Remove(betData);
                            break;
                        }

                    }
                }
                else
                {
                    foreach (var betData in tCell.BetsData)
                    {
                    print(string.Format("Check LOSE table cell {0} bet of player {1}", tCell.name, betData.PlayerStat));

                    if (player.ps.PlayerNick.Equals(betData.PlayerStat))
                        {
                            win -= betData.BetValue;
                            print(string.Format("Player {0} Lose {1}", betData.PlayerStat, betData.BetValue));
                        }
                    }
            }
            }


            player.ps.AllMoney += win;

            //player.ps.AllMoney += win;
            if (win > 0)
            {
                Debug.Log("StartAnim win:" + win);
                player.StartWinAnimation(win, player.ps.PlayerNick);
            }
        



        if(win < 0)
        {
            _playerInformer?.SetMessage(string.Format("You lose {0}$ !", Math.Abs(win)), WIN_STATUS.LOSE);
        }
        else if (win > 0)
        {
            _playerInformer?.SetMessage(string.Format("You win {0}$ !", Math.Abs(win)), WIN_STATUS.WIN);
        }
        else
        {
            _playerInformer?.SetMessage("You win nothing !", WIN_STATUS.NOTHING);
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
