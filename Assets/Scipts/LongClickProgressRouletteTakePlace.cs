using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class LongClickProgressRouletteTakePlace : TakePlaceLongClickProgress, IListener<ROULETTE_EVENT>
{
        

    private bool canLeave = true;

    private EventManager<ROULETTE_EVENT> eventMang;
    private new void Start()
    {
        base.Start();
        eventMang = GetComponentInParent<TableBetsManager>().rouletteEventManager;
        eventMang.AddListener(ROULETTE_EVENT.ROULETTE_GAME_START, this);
        eventMang.AddListener(ROULETTE_EVENT.ROULETTE_GAME_END, this);
    }

    protected new void OnTriggerEnter(Collider other)
    {
        if (canLeave)
        {
            base.OnTriggerEnter(other);
        }
        //if (other.gameObject.GetComponent<LongClickHand>() != null && inProgress == false && canLeave)
        //{
        //    playerStats = other.GetComponentInParent<PlayerStats>();
        //    if(p_place.ps == null || p_place.ps == playerStats)
        //    inProgress = true;

        //}
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
    [PunRPC]
    public override void InvokeClickOut_RPC()
    {
        eventMang.PostNotification(ROULETTE_EVENT.PLAYER_LEAVE, this, playerStats.PlayerNick);
        inGame = false;
        _imageReady.texture = _notReadyTexture;
        _textReady.text = "Not ready";
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
        _onLongClickOut?.Invoke(p_place.ps);
        photonView?.RPC("InvokeClickOut_RPC", RpcTarget.All);

    }

    protected override void InvokeClickIn()
    {
        photonView?.RequestOwnership();
        _onLongClickIn?.Invoke(playerStats);
        inGame = true;
        _imageReady.texture = _readyTexture;
        _textReady.text = PhotonNetwork.LocalPlayer.NickName;
        photonView?.RPC("InvokeClickIn_RPC", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName);

    }

}

