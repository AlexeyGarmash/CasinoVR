using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class TakePlaceUnityEvent : UnityEvent<PlayerStats> { }
public class LongClickProgress : MonoBehaviourPun, IListener<ROULETTE_EVENT>
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
    private bool canLeave = true;

    private EventManager<ROULETTE_EVENT> eventMang;
    private void Start()
    {
        _imageReady.texture = _notReadyTexture;
        _progressImage.fillAmount = 0f;
        p_place = GetComponentInParent<PlayerPlace>();

        eventMang = GetComponentInParent<TableBetsManager>().rouletteEventManager;
        eventMang.AddListener(ROULETTE_EVENT.ROULETTE_GAME_START,this);
        eventMang.AddListener(ROULETTE_EVENT.ROULETTE_GAME_END, this);
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

        photonView?.RPC("LeaveTable_RPC", RpcTarget.All);
       
    }

    private void InvokeClickTakePlace()
    {
        photonView?.RequestOwnership();
        _onLongClickTalePlace?.Invoke(playerStats);
        inGame = true;
        _imageReady.texture = _readyTexture;
        _textReady.text = PhotonNetwork.LocalPlayer.NickName;             
        photonView?.RPC("JoinTable_RPC", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName);
        
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
        if (other.gameObject.GetComponent<LongClickHand>() != null && inProgress == false && canLeave)
        {
            playerStats = other.GetComponentInParent<PlayerStats>();
            if(p_place.ps == null || p_place.ps == playerStats) 
            {
                if(p_place.ps != null && GetComponentInParent<TableBetsManager>().checkPlaceTakenYet(p_place.ps)) return;
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
        eventMang.PostNotification(ROULETTE_EVENT.PLAYER_LEAVE, this, playerStats.PlayerNick);
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

    public void OnEvent(ROULETTE_EVENT Event_type, Component Sender, params object[] Param)
    {
        switch (Event_type)
        {
            case ROULETTE_EVENT.ROULETTE_GAME_START:
                canLeave = false;
                break;
            case ROULETTE_EVENT.ROULETTE_GAME_END:
                canLeave = true;
                break;
        }
    }
}
