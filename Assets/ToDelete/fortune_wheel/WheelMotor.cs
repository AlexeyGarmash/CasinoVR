using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMotor : MonoBehaviour
{
    public enum FortuneWheelMotorState
    {
        STARTED,
        INVOKE_STOP,
        TOTAL_STOP,
        SLOW_ROTATE
    }

    public Action<FortuneWheelMotorState> MotorStateChanged;

    [SerializeField] private float smoothForceMult;
    [SerializeField] private float smoothMult;
    [SerializeField] private float motorTargetVelocity = 300;
    [SerializeField] private float motorForce = 50;

    private Rigidbody _rigidbody;
    private HingeJoint hingeJoint;
    private float startSmoothMult;

    [SerializeField] private bool invokeStopMotor = false;
    [SerializeField] private bool invokeForceStopMotor = false;
    [SerializeField] private bool slowRotation = false;

    private bool motorTotalStoped;

    private FortuneWheelMotorState motorState;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        hingeJoint = GetComponent<HingeJoint>();
        startSmoothMult = smoothMult;
    }

    public void StartMotor()
    {
        if(hingeJoint != null && hingeJoint.useMotor)
        {
            smoothMult = startSmoothMult;
            motorTotalStoped = false;
            slowRotation = false;
            motorState = FortuneWheelMotorState.STARTED;
            MotorStateChanged.Invoke(motorState);
            var motor = hingeJoint.motor;
            motor.force = motorForce;
            motor.targetVelocity = motorTargetVelocity;
            hingeJoint.motor = motor;
            hingeJoint.useMotor = true;
        }
    }

    internal void ForceStopMotor()
    {
        /*var motor = hingeJoint.motor;
        motorTotalStoped = true;
        motor.targetVelocity = 0;
        hingeJoint.motor = motor;*/
        invokeForceStopMotor = true;
    }

    public void StopMotor()
    {
        if (hingeJoint != null && hingeJoint.useMotor)
        {
            invokeStopMotor = true;
            motorState = FortuneWheelMotorState.INVOKE_STOP;
            MotorStateChanged.Invoke(motorState);
        }
    }

    private void FixedUpdate()
    {
        var motor = hingeJoint.motor;
        if(motor.targetVelocity >= 0 && motor.targetVelocity <= 25 && invokeStopMotor && !slowRotation)
        {
            motorState = FortuneWheelMotorState.SLOW_ROTATE;
            MotorStateChanged.Invoke(motorState);
            slowRotation = true;
            smoothMult /= 3;
        }
        if (motor.targetVelocity <= 0 && motor.targetVelocity <= 0.3 && invokeStopMotor)
        {
            motorTotalStoped = true;
            motorState = FortuneWheelMotorState.TOTAL_STOP;
            MotorStateChanged.Invoke(motorState);
            invokeStopMotor = false;
            invokeForceStopMotor = false;
            motor.targetVelocity = 0;
        }
        if(invokeStopMotor && !invokeForceStopMotor)
        {

            float calcTargetVelocity = motor.targetVelocity - (smoothMult * Time.fixedDeltaTime);

            if (calcTargetVelocity <= 0)
            {
                motor.targetVelocity = 0;
            }
            {
                motor.targetVelocity = calcTargetVelocity;
            }
            hingeJoint.motor = motor;
        }
        if(invokeForceStopMotor)
        {
            float calcTargetVelocity = motor.targetVelocity - (smoothForceMult * Time.fixedDeltaTime);
            if (calcTargetVelocity <= 0)
            {
                motor.targetVelocity = 0;
            }
            else
            {
                motor.targetVelocity = calcTargetVelocity;
            }
            hingeJoint.motor = motor;
        }
    }

    
}
