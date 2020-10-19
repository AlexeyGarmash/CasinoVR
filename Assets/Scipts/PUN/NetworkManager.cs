using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public const string TYPE_GAME = "type_game";
    public enum CasinoGameTypes
    {
        Slots,
        Roulette,
        Blackjack
    }
    public const string GAME_SLOTS = "Slots";
    public const string GAME_ROULETTE = "Roulette";
    public const string GAME_BLACKJACK = "Blackjack";
    public static NetworkManager Instance;
    [SerializeField] int MinPlayersCount = 1;
    [SerializeField] float fakeServerLoadTime = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    private void Start()
    {
        //ConnectToServer();
        StartCoroutine(StartServerWithPause(fakeServerLoadTime));
    }

    private IEnumerator StartServerWithPause(float pauseTime)
    {
        yield return new WaitForSeconds(pauseTime);
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.1";//MasterManager.GameSettings.GameVersion;
        PhotonNetwork.NickName = "rand";//MasterManager.GameSettings.NickName;
        print("Try connect to master...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public RoomOptions DefaultRoomOptions
    {
        get
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.BroadcastPropsChangeToAll = true;
            return roomOptions;
        }
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

    public void CreateRoom(string roomName, CasinoGameTypes gameType)
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.InRoom)
        {
            var roomSettings = new RoomOptions();
            roomSettings.MaxPlayers = 4;
            roomSettings.IsOpen = true;
            roomSettings.IsVisible = true;
            roomSettings.BroadcastPropsChangeToAll = true;
            roomSettings.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            switch (gameType)
            {
                case CasinoGameTypes.Slots:
                    roomSettings.CustomRoomProperties.Add(TYPE_GAME, GAME_SLOTS);
                    break;
                case CasinoGameTypes.Roulette:
                    roomSettings.CustomRoomProperties.Add(TYPE_GAME, GAME_ROULETTE);
                    break;
                case CasinoGameTypes.Blackjack:
                    roomSettings.CustomRoomProperties.Add(TYPE_GAME, GAME_BLACKJACK);
                    break;
            }
            
            PhotonNetwork.CreateRoom(roomName, roomSettings, TypedLobby.Default);
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


    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(!RoomCountPlayerAccepted(MinPlayersCount))
            {
                print("Not min count players");
                MainMenuInformer.Instance.ShowInfoWithExitTime($"Minimal players count is {MinPlayersCount}", MainMenuMessageType.Danger);
            } 
            else if (RoomPlayersReady())
            {
                StartGameLoading();
            }
            else
            {
                print("Not all users are ready");
                MainMenuInformer.Instance.ShowInfoWithExitTime("Not all players are ready", MainMenuMessageType.Warning);
            }
        }
        else
        {
            MainMenuInformer.Instance.ShowInfoWithExitTime("Only master can start the game", MainMenuMessageType.Warning);
        }
    }

    private bool RoomCountPlayerAccepted(int acceptedCount) => PhotonNetwork.CurrentRoom.Players.Count >= acceptedCount;
    

    private bool RoomPlayersReady()
    {
        foreach (var playerKV in PhotonNetwork.CurrentRoom.Players)
        {
            Player player = playerKV.Value;
            if (!player.IsReady())
            {
                return false;
            }
        }
        return true;
    }

    private void StartGameLoading()
    {
        MainMenuManager.Instance.StartGame();
    }

}
