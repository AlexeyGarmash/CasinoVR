using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnPlayer : MonoBehaviourPun
{
    [SerializeField] private string HeadPlayerPrefabPath_Remote;
    [SerializeField] private string HeadPlayerPrefabPath_Local;

    private void Start()
    {
        if (HeadPlayerPrefabPath_Remote != null && HeadPlayerPrefabPath_Remote != string.Empty)
        {
            if(photonView.IsMine)
                PhotonNetwork.Instantiate(HeadPlayerPrefabPath_Local, new Vector3(0, 0, 0), Quaternion.identity);
            else
                PhotonNetwork.Instantiate(HeadPlayerPrefabPath_Remote, new Vector3(0, 0, 0), Quaternion.identity);

        }
        else
        {
            print("Cannt instatiate prefab. Name is null");
        }
    }
}
