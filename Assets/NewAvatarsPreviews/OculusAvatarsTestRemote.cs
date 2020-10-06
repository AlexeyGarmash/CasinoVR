using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusAvatarsTestRemote : MonoBehaviour
{
    public void InstantiateAvatars()
    {
        GameObject avatar = PhotonNetwork.Instantiate("TestAvatar", Vector3.zero, Quaternion.identity);
    }
}
