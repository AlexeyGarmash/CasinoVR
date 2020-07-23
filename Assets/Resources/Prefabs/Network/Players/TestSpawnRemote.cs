using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnRemote : MonoBehaviour
{
    private static byte InstantiateVrAvatarEventCode = 170;
    private void Start()
    {
        
    }

    private void InstantiateLocalAvatar()
    {
        GameObject local = Instantiate(Resources.Load("Prefabs/Network/Players/LocalAvatar")) as GameObject;
        PhotonView photonView = local.GetComponent<PhotonView>();

        if (PhotonNetwork.AllocateViewID(photonView))
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache,
                Receivers = ReceiverGroup.Others
            };

            object[] content = new object[] {
                photonView.ViewID
            };

            PhotonNetwork.RaiseEvent(InstantiateVrAvatarEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
