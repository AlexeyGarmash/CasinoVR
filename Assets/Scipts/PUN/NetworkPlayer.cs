using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using TMPro.Examples;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{
    private Transform CenterEye;
    [SerializeField] private GameObject Avatar;
    [SerializeField] private TextMeshPro TextNickName;
    
    void Start()
    {
        if(photonView != null && photonView.IsMine)
        {
            GameObject globalVRController = GameObject.Find("OVRPlayerControllerMain");
            if(globalVRController != null)
            {
                Transform ovrControllerTransform = globalVRController.transform;
                CenterEye = ovrControllerTransform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform;
                transform.SetParent(CenterEye);
                transform.localPosition = Vector3.zero;
                TextNickName.text = PhotonNetwork.LocalPlayer.NickName;
            }
        }
    }

    
}
