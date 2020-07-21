using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateJoinRoomManager : BaseMenuPanel
{
    [SerializeField] private TMP_InputField InputFieldRoomName;
    [SerializeField] private TMP_Text TextCurrentNickName;

    [SerializeField] private Transform SrollViewTransformContent;
    [SerializeField] private RoomItem RoomItem;

    private List<RoomItem> _roomsList = new List<RoomItem>();

    public void CreateNewRoom()
    {
        string roomName = InputFieldRoomName.text;
        if (roomName.Length != 0)
        {
            NetworkManager.Instance.CreateRoom(roomName);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("Room list updated");
        UpdateRoomsList(roomList);
    }

    private void UpdateRoomsList(List<RoomInfo> roomList)
    {
        foreach (var roomInfo in roomList)
        {
            int roomIndex = _roomsList.FindIndex(room => room.RoomInfo.Name.Equals(roomInfo.Name));
            if (roomInfo.RemovedFromList)
            {
                if (roomIndex != -1)
                {
                    Destroy(_roomsList[roomIndex].gameObject);
                    _roomsList.RemoveAt(roomIndex);
                }
            }
            else
            {
                if(roomIndex != -1)
                {
                    continue;
                }
                RoomItem newRoomItem = Instantiate(RoomItem, SrollViewTransformContent);
                if (newRoomItem != null)
                {
                    newRoomItem.SetRoomInfo(roomInfo);
                    _roomsList.Add(newRoomItem);
                }
                else
                {
                    OnCreateRoomFailed(404, "Can not instatiate room item! Null");
                }
            }
        }
    }
}
