using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        print("Try connect to master...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedLobby()
    {
        print("Joined to lobby");
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to master"); 
        print("Try connect to lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnect from master " + cause);
    }

    public void CreateRoom(string roomName)
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.InRoom)
        {
            PhotonNetwork.CreateRoom(roomName, MasterManager.GameSettings.DefaultRoomOptions, TypedLobby.Default);
        }
        else
        {
            print("Photon not connected");
        }
    }

    public void JoinRoom(string roomName)
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            print("Photon not connected!");
        }
    }

    public void ChangePlayerNick(string newNick)
    {
        PhotonNetwork.LocalPlayer.NickName = newNick;
    }
}
