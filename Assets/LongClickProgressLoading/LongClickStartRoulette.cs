using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LongClickStartRoulette : MonoBehaviourPun
{
    [SerializeField] private UnityEvent _onLongClickStartRoulette;
    [SerializeField] private float _holdTime = 1.8f;
    [SerializeField] private Image _progressImage;

    private bool inProgress = false;
    private float currentHoldTime = 0f;

    private void Update()
    {
        if (inProgress)
        {
            ShowProgress();
        }
    }

    private void ShowProgress()
    {
        currentHoldTime += Time.deltaTime;
        if (currentHoldTime >= _holdTime)
        {
            //
            InvokeClickStartRoulette();
            ResetProgress();
        }
        FillImageProgress(currentHoldTime / _holdTime);
    }

    private void InvokeClickStartRoulette()
    {
        _onLongClickStartRoulette?.Invoke();
        //photonView?.RPC("StartRoulette_RPC", RpcTarget.Others);
    }

    

    private void ResetProgress()
    {
        inProgress = false;
        currentHoldTime = 0f;
        FillImageProgress(currentHoldTime);
    }

    private void FillImageProgress(float progress)
    {
        _progressImage.fillAmount = progress;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LongClickHand>() != null)
        {
            inProgress = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LongClickHand>() != null)
        {
            ResetProgress();
        }
    }

    
}
