using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TakePlaceLongClickProgress : LongClickProgerssBase, IListener<AbstractFieldEvents>
{

    [SerializeField]
    PlayerChipsField field;

    [PunRPC]
    public override void InvokeClickOut_RPC()
    {        
        inGame = false;
        _imageReady.texture = _notReadyTexture;
        _textReady.text = "Not ready";

        if (field)
        {
            field.FieldEventManager.AddListener(AbstractFieldEvents.FieldAnimationEnded, this);
            field.FieldEventManager.AddListener(AbstractFieldEvents.FieldAnimationStarted, this);
        }

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
        photonView.RequestOwnership();
        
      
        photonView.RPC("InvokeClickOut_RPC", RpcTarget.All);

        

        _onLongClickOut.Invoke(p_place.ps);

        p_place.handMenu.gameObject.SetActive(true);
        p_place.handMenu = null;
    }

    protected override void InvokeClickIn()
    {
        photonView.RequestOwnership();
        p_place.photonView.RequestOwnership();     

        p_place.ps = lastCollider.GetComponentInParent<PlayerStats>(); 
        p_place.handMenu = lastCollider.GetComponentInChildren<RadialMenuHandV2>();

        Debug.Log("TakePlaceInvoked");

        inGame = true;
        _imageReady.texture = _readyTexture;
        _textReady.text = PhotonNetwork.LocalPlayer.NickName;
        photonView.RPC("InvokeClickIn_RPC", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName);

        _onLongClickIn.Invoke(p_place.ps);

    }

    public void OnEvent(AbstractFieldEvents Event_type, Component Sender, params object[] Param)
    {
        switch (Event_type)
        {
            case AbstractFieldEvents.FieldAnimationEnded:
                gameObject.SetActive(true);
                break;
            case AbstractFieldEvents.FieldAnimationStarted:
                gameObject.SetActive(false);
                break;
        }
    }
}

