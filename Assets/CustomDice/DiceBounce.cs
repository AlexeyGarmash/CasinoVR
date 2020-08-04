using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBounce : MonoBehaviour
{
    [SerializeField] private DiceSide[] _diceSides;
    [SerializeField] private float _forceSpeed;
    

    private Rigidbody _rigidbody;
    private bool thrown;
    private bool hasLanded;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_rigidbody.useGravity = false;
    }

    private void FixedUpdate()
    {
        if(HandleInput())
        {
            RoleDice(true, Vector3.up);
        }
        if(thrown && _rigidbody.IsSleeping())
        {
            CheckSide();
            thrown = false;
            hasLanded = true;
        }
    }

    private void CheckSide()
    {
        foreach (var side in _diceSides)
        {
            if(side.OnGround)
            {
                print("Dice side = " + side.DiceValue);
            }
        }
    }

    public void RoleDice(bool useForce, Vector3 forceDir)
    {
        thrown = true;
        hasLanded = false;
        _rigidbody.useGravity = true;
        if (useForce)
        {
            _rigidbody.AddForce(forceDir * _forceSpeed);
        }
        _rigidbody.AddTorque(UnityEngine.Random.Range(0, 500), UnityEngine.Random.Range(0, 500), UnityEngine.Random.Range(0, 500));
    }

    private bool HandleInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
