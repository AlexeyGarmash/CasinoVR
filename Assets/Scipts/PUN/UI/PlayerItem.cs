using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using System;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public const string KEY_PLAYER_READY = "player_ready";
    public const string KEY_PLAYER_AVATAR = "player_avatar";

    [SerializeField] private TMP_Text TextPlayer;
    [SerializeField] private RawImage _imageReady;
    [SerializeField] private Texture _readySprite;
    [SerializeField] private Texture _notReadySprite;
    [SerializeField] private RawImage _imageAvatar;
    [SerializeField] private VoicePrefabSetup _voiceItem;

    public Player PlayerInfo { get; set; }

    public void SetPlayerInfo(Player player)
    {
        PlayerInfo = player;
        SetPLayerText(player);
        SetPlayerInfoReady(player);
        SetPlayerAvatar(player);
        if (_voiceItem != null)
        {
            _voiceItem.SetupPhotonVoice(player);
        }
    }

    

    private void SetPLayerText(Player playerChangesInfo)
    {
        string resultPLayerText = string.Format("{0}", PlayerInfo.NickName);
        TextPlayer.text = resultPLayerText;
    }

    private void SetPlayerInfoReady(Player playerChangesInfo)
    {
        bool playerReady = false;
        if (playerChangesInfo.CustomProperties.ContainsKey(KEY_PLAYER_READY))
        {
            playerReady = (bool)playerChangesInfo.CustomProperties[KEY_PLAYER_READY];
        }
        _imageReady.texture = playerReady ? _readySprite : _notReadySprite;
    }

    private void SetPlayerAvatar(Player playerChangesInfo)
    {
        if(playerChangesInfo.CustomProperties.ContainsKey(KEY_PLAYER_AVATAR))
        {
            string avatarNameRes = (string)playerChangesInfo.CustomProperties[KEY_PLAYER_AVATAR];
            if (avatarNameRes != null)
            {
                _imageAvatar.texture = Resources.Load(avatarNameRes) as Texture;
            }
            else
            {
                print("Cannot set avatar image");
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        print("Props changes at player -> " + targetPlayer.NickName);
        if(targetPlayer != null && PlayerInfo != null && targetPlayer == PlayerInfo)
        {
            if(changedProps.ContainsKey(KEY_PLAYER_READY))
            {
                print("Props player ready confirmed!");
                SetPlayerInfoReady(targetPlayer);
            }

            if(changedProps.ContainsKey(KEY_PLAYER_AVATAR))
            {
                print("Props avatar confirmed!");
                SetPlayerAvatar(targetPlayer);
            }
        }
    }
}
