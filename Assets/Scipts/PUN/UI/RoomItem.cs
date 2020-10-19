using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TMP_Text TextRoomName;
    [SerializeField] private TMP_Text TextGameType;
    [SerializeField] private Button ButtonJoinRoom;

    public RoomInfo RoomInfo { get; set; }

    private void Start()
    {
        ButtonJoinRoom.onClick.AddListener(OnButtonJoinRoom_Clicked);
    }
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        TextRoomName.text = roomInfo.Name;
        string gameTypeName = RoomInfo.CustomProperties[NetworkManager.TYPE_GAME] as string;
        TextGameType.text = gameTypeName;
    }

    private void OnButtonJoinRoom_Clicked()
    {
        try
        {
            NetworkManager.Instance.JoinRoom(RoomInfo?.Name);
        }
        catch(Exception ex)
        {
            print(ex.Message);
        }
    }
}
