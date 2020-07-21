using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteSpin : MonoBehaviour, ISpinableTransform
{
    #region Serialize Fields

    [SerializeField] private float IncreaseRotateSpeed = 100f;
    [SerializeField] private float RotateTime = 10f;
    [SerializeField] private float DecreaseRotateSpeed = 10f;

    #endregion

    #region Private Fields

    private bool _startRotate = false;
    private bool _isRotation = false;
    private float _increaseSpeed;

    #endregion


    #region Public Props

   

    EventManager<ROULETTE_EVENT> eventManager;

    #endregion

    private void Awake()
    {
        _increaseSpeed = IncreaseRotateSpeed;
    }

    private void Start()
    {
        //StartSpin();
        eventManager = GetComponentInParent<TableBetsManager>().rouletteEventManager;
    }

    private void Update()
    {
        if((!_startRotate && _isRotation) || (_startRotate && _isRotation))
        {
            ResolveRotateSpeed();
            transform.Rotate(-Vector3.forward * Time.deltaTime * IncreaseRotateSpeed);
        }
        
    }


    public void StartSpin()
    {
        StartCoroutine(_RotateRouletteBase());
    }

    private void ResolveRotateSpeed()
    {
        if(IncreaseRotateSpeed > 0)
        {
            IncreaseRotateSpeed -= Time.deltaTime * DecreaseRotateSpeed;
        } else
        {
            IncreaseRotateSpeed = 0;
            _isRotation = false;
            //OnRouletteSpinEnd.Invoke("Roulette spin end!");
            Debug.Log("ROULETTE_SPIN_END");
            eventManager.PostNotification(ROULETTE_EVENT.ROULETTE_SPIN_END, this);
        }
    }

    private IEnumerator _RotateRouletteBase()
    {
        StartRotate();
        yield return new WaitForSeconds(RotateTime);
        EndRotate();
    }
    private void StartRotate()
    {
        IncreaseRotateSpeed = _increaseSpeed;
        _startRotate = true;
        _isRotation = true;  
    }

    private void EndRotate()
    {
        _startRotate = false;
    }

    
}
