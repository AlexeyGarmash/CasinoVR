using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using System;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public const string KEY_PLAYER_READY = "player_ready";

    [SerializeField] private TMP_Text TextPlayer;

    public Player PlayerInfo { get; set; }

    public void SetPlayerInfo(Player player)
    {
        PlayerInfo = player;
        SetPLayerText(player);
    }

    private void SetPLayerText(Player playerChangesInfo)
    {
        bool playerReady = false;
        if (playerChangesInfo.CustomProperties.ContainsKey(KEY_PLAYER_READY))
        {
            playerReady = (bool)playerChangesInfo.CustomProperties[KEY_PLAYER_READY];
        }
        string resultPLayerText = string.Format("{0} - {1}", PlayerInfo.NickName, playerReady ? "R" : "N");
        TextPlayer.text = resultPLayerText;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        print("Props changes at player -> " + targetPlayer.NickName);
        if(targetPlayer != null && PlayerInfo != null && targetPlayer == PlayerInfo)
        {
            if(changedProps.ContainsKey(KEY_PLAYER_READY))
            {
                print("Props confirmed!");
                SetPLayerText(targetPlayer);
            }
        }
    }
}
