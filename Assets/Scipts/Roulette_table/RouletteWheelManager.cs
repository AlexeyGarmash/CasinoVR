﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteWheelManager : MonoBehaviourPun, IListener<ROULETTE_EVENT>
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
        eventManager = GetComponentInParent<TableBetsManager>().rouletteEventManager;
        eventManager.AddListener(ROULETTE_EVENT.ROULETTE_SPIN_END, this);
    }


    int winNumber;
    public void StartSpin(int winNumber)
    {
        photonView.RequestOwnership();
        if (RouletteWheelLogic.IsPossibleStartGame)
        {
            //
            photonView.RPC("StartSpin_RPC", RpcTarget.All, winNumber);
            StartSpinAll();
        }
        else
        {
            print("Wheel spin now, impossible spin now!!");
        }
    }

    [PunRPC]
    private void StartSpin_RPC(int winNumber)
    {
        Debug.Log("Wining numebr" + winNumber);
        this.winNumber = winNumber;
        RouletteWheelLogic.StartWheel();
        
    }

    private void StartSpinAll()
    {
        if(RouletteBallSpawner != null)
        {
            RouletteBallSpawner.SpawnBall(winNumber);
            BallSpin = RouletteBallSpawner.CreatedBallPrefab.GetComponent<BallSpin>();
            BallTrigger = RouletteBallSpawner.CreatedBallPrefab.GetComponent<BallTrigger>();
        }

        if(BallSpin != null && RouletteSpin != null && BallTrigger != null)
        {
            BallTrigger.OnBallInCell -= BallTrigger_OnBallInCell;
            BallTrigger.OnBallInCell += BallTrigger_OnBallInCell;
            BallSpin.SetCenterMass(BallCenterRotateTransform);
            BallSpin.StartSpin();
            
            RouletteSpin.StartSpin();
        }
    }

    private void BallTrigger_OnBallInCell(WheelCellData obj)
    {
       

        if(obj.Number == winNumber)
            RouletteWheelLogic.ReceiveWheelCellData(obj);
    }

    //private void RouletteSpin_OnRouletteSpinEnd(string obj)
    //{
    //    eventManager.PostNotification(ROULETTE_EVENT.ROULETTE_GAME_END, this, RouletteWheelLogic.WheelCellData);
    //    print(obj);
    //    RouletteWheelLogic.StopWheel();
    //    OnRouletteWheelFinish.Invoke(RouletteWheelLogic.WheelCellData);
    //}

    public void OnEvent(ROULETTE_EVENT Event_type, Component Sender, params object[] Param)
    {
        switch (Event_type)
        {
            case ROULETTE_EVENT.ROULETTE_SPIN_END:
               
                RouletteWheelLogic.StopWheel();
                Debug.Log("CHECK_WINNERS");
                eventManager.PostNotification(ROULETTE_EVENT.CHECK_WINNERS, this, RouletteWheelLogic.WheelCellData);
                 break;
        }
    }
}
