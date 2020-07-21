using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnPlayer : MonoBehaviour
{
    [SerializeField] private string HeadPlayerPrefabPath;

    private void Start()
    {
        string prefabName = HeadPlayerPrefabPath;
        if (prefabName != null && prefabName != string.Empty)
        {
            PhotonNetwork.Instantiate(prefabName, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            print("Cannt instatiate prefab. Name is null");
        }
    }
}
