using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using System;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    public static MainMenuManager Instance;



    [SerializeField] private GameObject LoadingServerPanel;
    [SerializeField] private GameObject CreatePlayerPanel;
    [SerializeField] private GameObject JoinOrCreateRoomPanel;
    [SerializeField] private GameObject CurrentRoomPanel;
    [SerializeField] private LoadingWithProgressManager LoadingWithProgressManager;

    

    private Vector3 fullScale = new Vector3(1, 1, 1);
    private Vector3 removeScale = new Vector3(0, 0, 0);

    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        LoadingServerPanel.transform.SetAsLastSibling();
        //LoadingServerPanel.SetScale(fullScale);
        /*LoadingWithProgressManager.gameObject.SetScale(removeScale);
        

        CreatePlayerPanel.SetScale(removeScale);//
        JoinOrCreateRoomPanel.SetScale(removeScale);
        CurrentRoomPanel.SetScale(removeScale);*/
    }

    

    public override void OnConnectedToMaster()
    {
        /*LoadingServerPanel.SetScale(removeScale);
        CreatePlayerPanel.SetScale(fullScale);*/
        CreatePlayerPanel.transform.SetAsLastSibling();
    }

    public void OnPlayerEnterNickName()
    {
        /*CreatePlayerPanel.SetScale(removeScale);
        JoinOrCreateRoomPanel.SetScale(fullScale);
        CurrentRoomPanel.SetScale(removeScale);*/

        JoinOrCreateRoomPanel.transform.SetAsLastSibling();
    }

    public void OnPlayerJoinRoom()
    {
        /*JoinOrCreateRoomPanel.SetScale(removeScale);
        CurrentRoomPanel.SetScale(fullScale);*/

        CurrentRoomPanel.transform.SetAsLastSibling();
    }

    public void OnPlayerLeftRoom()
    {
        /*CurrentRoomPanel.SetScale(removeScale);
        JoinOrCreateRoomPanel.SetScale(fullScale);*/

        JoinOrCreateRoomPanel.transform.SetAsLastSibling();
    }

    public void PlayerGoBackToChooseAvatar()
    {
        /*JoinOrCreateRoomPanel.SetScale(removeScale);
        CreatePlayerPanel.SetScale(fullScale);*/

        CreatePlayerPanel.transform.SetAsLastSibling();
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

    public override void OnDisconnected(DisconnectCause cause)
    {
        /*LoadingServerPanel.SetScale(fullScale);
        CreatePlayerPanel.SetScale(removeScale);
        JoinOrCreateRoomPanel.SetScale(removeScale);
        CurrentRoomPanel.SetScale(removeScale);*/

        LoadingServerPanel.transform.SetAsLastSibling();
        NetworkManager.Instance.ConnectToServer();
    }

    public void StartGame()
    {
        //LoadingWithProgressManager.gameObject.SetScale(fullScale);
        LoadingWithProgressManager.transform.SetAsLastSibling();
        StartCoroutine(_startGame());
    }

    private IEnumerator _startGame()
    {
        PhotonNetwork.LoadLevel(1);
        while(PhotonNetwork.LevelLoadingProgress < 1)
        {
            LoadingWithProgressManager.SetProgress(PhotonNetwork.LevelLoadingProgress);
            yield return new WaitForEndOfFrame();
        }
    }
}
