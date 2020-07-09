using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteWheelManager : MonoBehaviour
{
    [SerializeField] private RouletteSpin RouletteSpin;
    [SerializeField] private RouletteBallSpawner RouletteBallSpawner;
    [SerializeField] private Transform BallCenterRotateTransform;


    private BallSpin BallSpin;
    private BallTrigger BallTrigger;
    private RouletteWheelLogic RouletteWheelLogic;


    private void Awake()
    {
        RouletteWheelLogic = new RouletteWheelLogic();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartSpin();
        }
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
            BallTrigger.OnBallInCell += BallTrigger_OnBallInCell;
            BallSpin.SetCenterMass(BallCenterRotateTransform);
            BallSpin.StartSpin();
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
        print(obj);
        RouletteWheelLogic.StopWheel();
    }
}
