﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class TakePlaceUnityEvent : UnityEvent<PlayerStats> { }
public class LongClickProgress : MonoBehaviourPun
{
    [SerializeField] private TakePlaceUnityEvent _onLongClickTalePlace;
    [SerializeField] private UnityEvent _onLongClickLeavePlace;
    [SerializeField] private float _holdTime;
    [SerializeField] private Image _progressImage;
    [SerializeField] private TMP_Text _textReady;
    [SerializeField] private RawImage _imageReady;

    [SerializeField] private Texture _readyTexture;
    [SerializeField] private Texture _notReadyTexture;

    [SerializeField] private PlayerPlace p_place;
    private bool inProgress = false;
    private float currentHoldTime = 0f;
    private bool inGame = false;
    private PlayerStats playerStats;
    private int enterCount = 0;
    private int exitCount = 0;
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
            if (!inGame)
            {
                InvokeClickTakePlace();
            }
            else
            {
                InvokeClickGoOutFromPlace();
            }
            ResetProgress();
        }
        FillImageProgress(currentHoldTime / _holdTime);
    }

    private void InvokeClickGoOutFromPlace()
    {
        photonView?.RequestOwnership();
        _onLongClickLeavePlace?.Invoke();
        inGame = false;
        _imageReady.texture = _notReadyTexture;
        _textReady.text = "Not ready";
       
        print("FUCKING GO OUT <ESSAGE");
        photonView?.RPC("LeaveTable_RPC", RpcTarget.Others);
       
    }

    private void InvokeClickTakePlace()
    {
        photonView?.RequestOwnership();
        _onLongClickTalePlace?.Invoke(playerStats);
        inGame = true;
        _imageReady.texture = _readyTexture;
        _textReady.text = PhotonNetwork.LocalPlayer.NickName;
        //if (photonView.IsMine)
        //{
            print("FUCKING TAKE PLACE <ESSAGE");
            photonView?.RPC("JoinTable_RPC", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName);
        //}
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
            if(p_place.ps == null || p_place.ps == playerStats) 
            {
                if(!inGame && GetComponentInParent<TableBetsManager>().checkPlaceTakenYet(playerStats)) return;
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
    public void LeaveTable_RPC()
    {
        inGame = false;
        _imageReady.texture = _notReadyTexture;
        _textReady.text = "Hold to join";
    }


    [PunRPC]
    public void JoinTable_RPC(string nickname)
    {
        inGame = true;
        _imageReady.texture = _readyTexture;
        _textReady.text = nickname;
    }


}
