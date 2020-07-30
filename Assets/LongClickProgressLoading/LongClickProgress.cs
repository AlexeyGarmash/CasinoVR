using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LongClickProgress : MonoBehaviourPun
{
    [SerializeField] private UnityEvent _onLongClickTalePlace;
    [SerializeField] private UnityEvent _onLongClickLeavePlace;
    [SerializeField] private float _holdTime;
    [SerializeField] private Image _progressImage;
    [SerializeField] private TMP_Text _textReady;
    [SerializeField] private RawImage _imageReady;

    [SerializeField] private Texture _readyTexture;
    [SerializeField] private Texture _notReadyTexture;

    private bool inProgress = false;
    private float currentHoldTime = 0f;
    private bool inGame = false;

    private void Start()
    {
        _imageReady.texture = _notReadyTexture;
        _progressImage.fillAmount = 0f;
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
        //photonView?.RequestOwnership();
        //_onLongClickLeavePlace?.Invoke();
        inGame = false;
        _imageReady.texture = _notReadyTexture;
        _textReady.text = "Not ready";
        //if (photonView.IsMine)
        //{
            print("FUCKING GO OUT <ESSAGE");
            photonView?.RPC("LeaveTable_RPC", RpcTarget.Others);
        //}
    }

    private void InvokeClickTakePlace()
    {
        //photonView?.RequestOwnership();
        //_onLongClickTalePlace?.Invoke();
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
        if (other.gameObject.GetComponent<LongClickHand>() != null)
        {
            //if (inGame && !photonView.IsMine) return;
            inProgress = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LongClickHand>() != null)
        {
            //if (inGame && !photonView.IsMine) return;
            ResetProgress();
        }
    }

    [PunRPC]
    public void LeaveTable_RPC()
    {
        inGame = false;
        _imageReady.texture = _notReadyTexture;
        _textReady.text = "Not ready";
    }


    [PunRPC]
    public void JoinTable_RPC(string nickname)
    {
        inGame = true;
        _imageReady.texture = _readyTexture;
        _textReady.text = nickname;
    }


}
