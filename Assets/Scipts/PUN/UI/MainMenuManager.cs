using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    public static MainMenuManager Instance;

    [SerializeField] private GameObject LoadingServerPanel;
    [SerializeField] private GameObject CreatePlayerPanel;
    [SerializeField] private GameObject JoinOrCreateRoomPanel;
    [SerializeField] private GameObject CurrentRoomPanel;

    private Vector3 fullScale = new Vector3(1, 1, 1);
    private Vector3 removeScale = new Vector3(0, 0, 0);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadingServerPanel.SetScale(fullScale);

        CreatePlayerPanel.SetScale(removeScale);
        JoinOrCreateRoomPanel.SetScale(removeScale);
        CurrentRoomPanel.SetScale(removeScale);
    }

    public override void OnConnectedToMaster()
    {
        LoadingServerPanel.SetScale(removeScale);
        CreatePlayerPanel.SetScale(fullScale);
    }

    public void OnPlayerEnterNickName()
    {
        CreatePlayerPanel.SetScale(removeScale);
        JoinOrCreateRoomPanel.SetScale(fullScale);
        CurrentRoomPanel.SetScale(removeScale);
    }

    public void OnPlayerJoinRoom()
    {
        JoinOrCreateRoomPanel.SetScale(removeScale);
        CurrentRoomPanel.SetScale(fullScale);
    }

    public void OnPlayerLeftRoom()
    {
        CurrentRoomPanel.SetScale(removeScale);
        JoinOrCreateRoomPanel.SetScale(fullScale);
    }

    public override void OnCreatedRoom()
    {
        print("New room is created now");
    }

    public override void OnJoinedRoom()
    {
        print("Player join room");
        OnPlayerJoinRoom();
    }

    public override void OnLeftRoom()
    {
        OnPlayerLeftRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print(string.Format("Create room failed. Cause: {0}", message));
    }
}
