using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteWheelManager : MonoBehaviour
{
    public event Action<WheelCellData> OnRouletteWheelFinish;

    [SerializeField] private RouletteSpin RouletteSpin;
    [SerializeField] private RouletteBallSpawner RouletteBallSpawner;
    [SerializeField] private Transform BallCenterRotateTransform;


    private BallSpin BallSpin;
    private BallTrigger BallTrigger;
    private RouletteWheelLogic RouletteWheelLogic;

    EventManager<ROULETTE_EVENT> eventManager;
    private void Awake()
    {
        eventManager = GetComponent<TableBetsManager>().rouletteEventManager;
        RouletteWheelLogic = new RouletteWheelLogic();
    }

    private void Start()
    {
        
    }

    

    public void StartSpin()
    {
        if (RouletteWheelLogic.IsPossibleStartGame)
        {
            RouletteWheelLogic.StartWheel();
            StartSpinAll();
        }
        else
        {
            print("Wheel spin now, impossible spin now!!");
        }
    }

    private void StartSpinAll()
    {
        if(RouletteBallSpawner != null)
        {
            RouletteBallSpawner.SpawnBall();
            BallSpin = RouletteBallSpawner.CreatedBallPrefab.GetComponent<BallSpin>();
            BallTrigger = RouletteBallSpawner.CreatedBallPrefab.GetComponent<BallTrigger>();
        }

        if(BallSpin != null && RouletteSpin != null && BallTrigger != null)
        {
            BallTrigger.OnBallInCell -= BallTrigger_OnBallInCell;
            BallTrigger.OnBallInCell += BallTrigger_OnBallInCell;
            BallSpin.SetCenterMass(BallCenterRotateTransform);
            BallSpin.StartSpin();
            RouletteSpin.OnRouletteSpinEnd -= RouletteSpin_OnRouletteSpinEnd;
            RouletteSpin.OnRouletteSpinEnd += RouletteSpin_OnRouletteSpinEnd;
            RouletteSpin.StartSpin();
        }
    }

    private void BallTrigger_OnBallInCell(WheelCellData obj)
    {
        print(obj.Number);
        RouletteWheelLogic.ReceiveWheelCellData(obj);
    }

    private void RouletteSpin_OnRouletteSpinEnd(string obj)
    {
        eventManager.PostNotification(ROULETTE_EVENT.ROULETTE_GAME_END, this, RouletteWheelLogic.WheelCellData);
        print(obj);
        RouletteWheelLogic.StopWheel();
        OnRouletteWheelFinish.Invoke(RouletteWheelLogic.WheelCellData);
    }
}
