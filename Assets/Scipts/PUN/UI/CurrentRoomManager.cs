﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class CurrentRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform Content;
    [SerializeField] private PlayerItem PlayerItem;
    [SerializeField] private Button ButtonReady;
    [SerializeField] private TMP_Text ButtonReadyText;
    [SerializeField] private Button ButtonStartGame;

    private List<PlayerItem> _playerItems = new List<PlayerItem>();
    private bool _isReady = false;
    private ExitGames.Client.Photon.Hashtable customProps;

    private void Awake()
    {
        customProps = new ExitGames.Client.Photon.Hashtable();
        ButtonReady.onClick.AddListener(OnButtonReady_Clicked);
        ButtonStartGame.onClick.AddListener(OnButtonStartGame_Clicked);
    }

    private void OnButtonReady_Clicked()
    {
        _isReady = !_isReady;
        UpdateCustomProps();
    }

    private void UpdateCustomProps()
    {
        customProps[PlayerItem.KEY_PLAYER_READY] = _isReady;
        PhotonNetwork.SetPlayerCustomProperties(customProps);
        ButtonReadyText.text = _isReady ? "Ready" : "Not ready";
    }

    public void LeaveRoom()
    {
        _isReady = false; 
        UpdateCustomProps();
        PhotonNetwork.LeaveRoom();
    }

    private void InstatiatePlayer(Player playerInfo)
    {
        PlayerItem newPlayerItem = Instantiate(PlayerItem, Content);
        if (newPlayerItem != null)
        {
            newPlayerItem.SetPlayerInfo(playerInfo);
            _playerItems.Add(newPlayerItem);
        }
    }

    private void InstantiateStartPlayers()
    {
        foreach (var punPlayer in PhotonNetwork.CurrentRoom.Players)
        {
            InstatiatePlayer(punPlayer.Value);
        }
    }

    private void RemoveAllPLayers()
    {
        foreach (var player in _playerItems)
        {
            Destroy(player.gameObject);
        }
        _playerItems.Clear();
    }

    public override void OnJoinedRoom()
    {
        print("Hi local player");
        //InstatiatePlayer(PhotonNetwork.LocalPlayer);
        InstantiateStartPlayers();
    }

    public override void OnLeftRoom()
    {
        print("Master client left room, remove all players");
        RemoveAllPLayers();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("New player ENTER ROOM");
        InstatiatePlayer(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int indexPlayer = _playerItems.FindIndex(p => p.PlayerInfo.NickName.Equals(otherPlayer.NickName));
        if(indexPlayer != -1)
        {
            Destroy(_playerItems[indexPlayer].gameObject);
            _playerItems.RemoveAt(indexPlayer);
        }
    }

    private void OnButtonStartGame_Clicked()
    {
        StartGame();
    }

    public void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (RoomPlayersReady())
            {
                PhotonNetwork.LoadLevel(1);
            }
            else
            {
                print("Not all users are ready");
            }
        }
    }

    private bool RoomPlayersReady()
    {
        foreach (var playerKV in PhotonNetwork.CurrentRoom.Players)
        {
            Player player = playerKV.Value;
            if(!player.IsReady())
            {
                return false;
            }
        }
        return true;
    }

}
