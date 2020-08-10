using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LongClickReadyPlay : MonoBehaviourPun
{
    [SerializeField] private float _holdTime;
    [SerializeField] private Image _progressImage;
    [SerializeField] private TMP_Text _textReady;
    [SerializeField] private RawImage _imageReady;

    [SerializeField] private Texture _readyTexture;
    [SerializeField] private Texture _notReadyTexture;

    [SerializeField] private PlayerPlace p_place;
    private bool inProgress = false;
    private float currentHoldTime = 0f;
    private bool isReady = false;
    private PlayerStats playerStats;
    private void Start()
    {
        _imageReady.texture = _notReadyTexture;
        _progressImage.fillAmount = 0f;
        p_place = GetComponentInParent<PlayerPlace>();
    }

    private void Update()
    {
        if(inProgress)
        {
            ShowProgress();
        }
    }

    private void ShowProgress()
    {
        currentHoldTime += Time.deltaTime;
        if(currentHoldTime >= _holdTime)
        {
            //
            if (!isReady)
            {
                InvokeReadyBehaviour();
            }
            else
            {
                InvokeNotReadyBehaviour();
            }
            ResetProgress();
        }
        FillImageProgress(currentHoldTime / _holdTime);
    }

    private void InvokeNotReadyBehaviour()
    {
        photonView?.RequestOwnership();
        isReady = false;
        _imageReady.texture = _notReadyTexture;
        _textReady.text = "Not ready";
        p_place.NotReadyToPlay();
        photonView?.RPC("NotReady_RPC", RpcTarget.Others);
    }

    private void InvokeReadyBehaviour()
    {
        photonView?.RequestOwnership();
        isReady = true;
        _imageReady.texture = _readyTexture;
        _textReady.text = "Ready";
        p_place.ReadyToPlay();
        photonView?.RPC("Ready_RPC", RpcTarget.Others);
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
        if (other.gameObject.GetComponent<LongClickHand>() != null && inProgress == false)
        {
            playerStats = other.GetComponentInParent<PlayerStats>();
            if(p_place.ps != null || p_place.ps == playerStats) 
            {
                inProgress = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LongClickHand>() != null)
        {
            //if (!photonView.IsMine) return;
            ResetProgress();
        }
    }

    [PunRPC]
    public void NotReady_RPC()
    {
        isReady = false;
        _imageReady.texture = _notReadyTexture;
        _textReady.text = "Not ready";
    }


    [PunRPC]
    public void Ready_RPC()
    {
        isReady = true;
        _imageReady.texture = _readyTexture;
        _textReady.text = "Ready";
    }
}
