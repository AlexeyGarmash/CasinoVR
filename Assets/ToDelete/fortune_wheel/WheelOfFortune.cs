using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelOfFortune : MonoBehaviour
{
    private WheelMotor wheelMotor;
    private FortuneWheelPointer fortuneWheelPointer;

    [SerializeField] private bool stopWheel;
    [SerializeField] private bool startWheel;
    [SerializeField] private float spinTime;

    [SerializeField] private bool spinNow = false;

    [SerializeField] private bool slowRotate = false;
    [SerializeField] private int CostToWin = 300;

    private void Start()
    {
        wheelMotor = GetComponentInChildren<WheelMotor>();
        fortuneWheelPointer = GetComponentInChildren<FortuneWheelPointer>();
        wheelMotor.MotorStateChanged += OnMotorStateChanged;
    }

    

    private void Update()
    {
        if(startWheel)
        {
            SpinWheel();
            startWheel = false;
        }
        if(spinNow && slowRotate)
        {
            print(string.Format("Slow rotate => Sector => {0}", fortuneWheelPointer.LastSector));
            if (fortuneWheelPointer.LastSector.Cost == CostToWin)
            {
                print("FORCE STOP");
                ForceStopWheel();
            }
        }
    }

    private void ForceStopWheel()
    {
        slowRotate = false;
        wheelMotor.ForceStopMotor();
    }

    private void SpinWheel()
    {
        if(!spinNow)
        {
            spinNow = true;
            slowRotate = false;
            StartCoroutine(SpinWheelWithTime());
        }
    }


    private IEnumerator SpinWheelWithTime()
    {
        wheelMotor.StartMotor();
        yield return new WaitForSeconds(spinTime);
        wheelMotor.StopMotor();
    }

    private void OnMotorStateChanged(WheelMotor.FortuneWheelMotorState stateMotor)
    {
        switch(stateMotor)
        {
            case WheelMotor.FortuneWheelMotorState.INVOKE_STOP:
                print("INVOKED STOP MOTOR");
                break;
            case WheelMotor.FortuneWheelMotorState.STARTED:
                print("INVOKED START MOTOR");
                break;
            case WheelMotor.FortuneWheelMotorState.TOTAL_STOP:
                spinNow = false;
                print("TOTAL STOP MOTOR");
                print(fortuneWheelPointer.LastSector);
                break;
            case WheelMotor.FortuneWheelMotorState.SLOW_ROTATE:
                slowRotate = true;
                print("SLOW ROTATE MOTOR");
                break;
        }
    }
}
