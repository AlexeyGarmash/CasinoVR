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
