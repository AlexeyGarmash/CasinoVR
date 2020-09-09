using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class TakePlaceUnityEvent : UnityEvent<PlayerStats> { }
public abstract class LongClickProgerssBase : MonoBehaviourPun
{
    [SerializeField] protected TakePlaceUnityEvent _onLongClickIn;
    [SerializeField] protected UnityEvent _onLongClickOut;

    [SerializeField] protected float _holdTime;
    [SerializeField] protected Image _progressImage;
    [SerializeField] protected TMP_Text _textReady;
    [SerializeField] protected RawImage _imageReady;

    [SerializeField] protected Texture _readyTexture;
    [SerializeField] protected Texture _notReadyTexture;

    [SerializeField] protected PlayerPlace p_place;

    protected bool inProgress = false;
    protected float currentHoldTime = 0f;
    protected bool inGame = false;
    protected PlayerStats playerStats;
    protected int enterCount = 0;
    protected int exitCount = 0;

    protected void Start()
    {
        _imageReady.texture = _notReadyTexture;
        _progressImage.fillAmount = 0f;
        p_place = GetComponentInParent<PlayerPlace>();
    }
    protected void Update()
    {
        if (inProgress)
        {
            ShowProgress();
        }
    }
    protected void ResetProgress()
    {
        inProgress = false;
        currentHoldTime = 0f;
        FillImageProgress(currentHoldTime);
    }
    protected void FillImageProgress(float progress)
    {
        _progressImage.fillAmount = progress;
    }

    protected void ShowProgress()
    {
        currentHoldTime += Time.deltaTime;
        if (currentHoldTime >= _holdTime)
        {
            //
            if (!inGame)
            {
                InvokeClickIn();
            }
            else
            {
                InvokeClickOut();
            }
            ResetProgress();
        }
        FillImageProgress(currentHoldTime / _holdTime);
    }

    [PunRPC]
    public virtual void InvokeClickOut_RPC()
    {
        //eventMang.PostNotification(ROULETTE_EVENT.PLAYER_LEAVE, this, playerStats.PlayerNick);
        inGame = false;
        _imageReady.texture = _notReadyTexture;
        _textReady.text = "Not ready";
    }


    [PunRPC]
    public virtual void InvokeClickIn_RPC(string nickname)
    {
        inGame = true;
        _imageReady.texture = _readyTexture;
        _textReady.text = nickname;
    }
    protected virtual void InvokeClickOut()
    {
        photonView?.RequestOwnership();
        _onLongClickOut?.Invoke();
        photonView?.RPC("InvokeClickOut_RPC", RpcTarget.All);

    }

    protected virtual void InvokeClickIn()
    {
        photonView?.RequestOwnership();
        p_place.ps = lastCollider.GetComponentInParent<PlayerStats>();
        p_place.handMenu = lastCollider.GetComponentInChildren<RadialMenuHandV2>();
        _onLongClickIn?.Invoke(p_place.ps);
        inGame = true;
        _imageReady.texture = _readyTexture;
        _textReady.text = PhotonNetwork.LocalPlayer.NickName;
        photonView?.RPC("InvokeClickIn_RPC", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName);

    }

    protected Collider lastCollider;
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LongClickHand>() != null && inProgress == false)
        {
            lastCollider = other;
            playerStats = other.GetComponentInParent<PlayerStats>();
            if (!p_place.PlayerOnPlace || p_place.ps.PlayerNick == playerStats.PlayerNick)
                inProgress = true;

        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LongClickHand>() != null)
        {
            //if (!photonView.IsMine) return;
            ResetProgress();
        }
    }
}