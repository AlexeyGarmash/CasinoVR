using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private string _gameVersion = "0.1";
    [SerializeField] private string _nickName;
    [SerializeField] private byte _maxPlayersPerRoom = 2;
    public string NickName
    {
        get => string.Format("{0}#{1}", _nickName, Random.Range(1, 1000));
    }

    public string GameVersion
    {
        get => _gameVersion;
    }

    public byte MaxPlayersPerRoom { get => _maxPlayersPerRoom; }

    public RoomOptions DefaultRoomOptions
    {
        get
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = _maxPlayersPerRoom;
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            return roomOptions;
        }
    }
}
