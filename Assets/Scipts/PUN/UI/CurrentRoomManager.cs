using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class CurrentRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform Content;
    [SerializeField] private PlayerItem PlayerItem;
    private List<PlayerItem> _playerItems = new List<PlayerItem>();

    public void LeaveRoom()
    {
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
        InstatiatePlayer(PhotonNetwork.LocalPlayer);
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
            Destroy(_playerItems[indexPlayer]);
            _playerItems.RemoveAt(indexPlayer);
        }
    }

}
