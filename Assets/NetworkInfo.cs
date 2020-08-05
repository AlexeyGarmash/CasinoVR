using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInfo : MonoBehaviour
{
    public bool isMine = false;
    public int ViewId;
    private void Start()
    {
        var view = GetComponent<PhotonView>();
        isMine = view.IsMine;
        ViewId = view.ViewID;


    }
}
