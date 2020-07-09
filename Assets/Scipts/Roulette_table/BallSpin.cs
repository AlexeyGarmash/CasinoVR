using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallSpin : MonoBehaviour, ISpinableTransform
{
    


    [SerializeField] private float ForceStrength;
    [SerializeField] private ForceMode ForceForceMode = ForceMode.Force;
    [SerializeField] private Transform CenterRoulette;
    [SerializeField] private float ImpulseStrength;
    [SerializeField] private float TimeToSpin;
    [SerializeField] private float DecreaseSpeedSpin;

    

    private bool isSpinnNow = true;
    private bool isSpinnStop = false;
    private bool isImpulse = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //StartBallSpin();
    }

    public void StartSpin()
    {
         StartBallSpin();
    }

    public void SetCenterMass(Transform center)
    {
        CenterRoulette = center;
    }


    public void StartBallSpin()
    {
        StartCoroutine(BallSpinnable());
    }

    private void AddBallForce(Vector3 force, ForceMode mode)
    {
        rb.AddForce(force, mode);
    }

    void FixedUpdate()
    {
        if (isSpinnNow)
        {
            SpinBall();
        } 
        else
        {
            if (!isSpinnStop)
            {
                if(isImpulse)
                    ImpulseToCenterBall();
                
                SmoothStopSpinBall();
                SpinBall();
            }
            else
            {
                ImpulseStrength = 0f;
            }
        }
    }

    private void ImpulseToCenterBall()
    {
        Vector3 vectorDir = CenterRoulette.position - transform.position;
        vectorDir.Normalize();
        AddBallForce(vectorDir * ImpulseStrength, ForceMode.Impulse);
        isImpulse = false;
    }

    private void SpinBall()
    {
        Vector3 vectorDir = CenterRoulette.position - transform.position;
        Vector3 vectorForce = Vector3.Cross(vectorDir, Vector3.up);
        vectorForce.Normalize();
        AddBallForce(vectorForce * ForceStrength, ForceForceMode);
    }

    private void SmoothStopSpinBall()
    {
        if(ForceStrength > 0)
        {
            ForceStrength -= Time.deltaTime * DecreaseSpeedSpin;
            if(!isImpulse && ForceStrength < 25)
            {
                isImpulse = true;
            }
        } 
        else
        {
            isSpinnStop = true;
        }
    }

    private IEnumerator BallSpinnable()
    {
        isSpinnNow = true;
        isSpinnStop = false;
        yield return new WaitForSeconds(TimeToSpin);
        isSpinnNow = false;
    }

   
}

    
