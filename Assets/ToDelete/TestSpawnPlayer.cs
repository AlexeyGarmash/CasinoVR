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
        if (PhotonPlayerSettings.Instance != null)
        {
            if(PhotonPlayerSettings.Instance.PrefabResourceName != null)
                PhotonNetwork.Instantiate(PhotonPlayerSettings.Instance.PrefabResourceName, new Vector3(0, 0, 0), Quaternion.identity);
            

        }
        else
        {
            print("Cannt instatiate prefab. Name is null");
        }
    }
}
