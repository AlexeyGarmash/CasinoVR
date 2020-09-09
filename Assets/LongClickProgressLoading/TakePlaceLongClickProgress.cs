using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TakePlaceLongClickProgress : LongClickProgerssBase
{
    [PunRPC]
    public override void InvokeClickOut_RPC()
    {        
        inGame = false;
        _imageReady.texture = _notReadyTexture;
        _textReady.text = "Not ready";

        Debug.LogWarning("player InvokeClickOut_RPC rpc");

    }


    [PunRPC]
    public override void InvokeClickIn_RPC(string nickname)
    {
        inGame = true;
        _imageReady.texture = _readyTexture;
        _textReady.text = nickname;
    }
    protected override void InvokeClickOut()
    {
        photonView?.RequestOwnership();
        p_place.handMenu = null;
        _onLongClickOut?.Invoke();
        photonView?.RPC("InvokeClickOut_RPC", RpcTarget.All);

    }

    protected override void InvokeClickIn()
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
    protected new void Start()
    {
        base.Start();      
    }     

}

