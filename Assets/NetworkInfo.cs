using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInfo : MonoBehaviour
{
    public bool isMine = false;

    private void Start()
    {
        isMine = GetComponent<PhotonView>().IsMine;
    }
}
