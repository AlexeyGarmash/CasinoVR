using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using TMPro.Examples;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{
    private Transform CenterEye;
    private Transform RightHandAnchor;
    private Transform LeftHandAnchor;
    [SerializeField] private GameObject Avatar;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject LeftHand;
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
                RightHandAnchor = ovrControllerTransform.Find("OVRCameraRig/TrackingSpace/RightHandAnchor").transform;
                LeftHandAnchor = ovrControllerTransform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").transform;
                SetHeadAvatar();
                SetHand(LeftHand, LeftHandAnchor);
                SetHand(RightHand, RightHandAnchor);
                TextNickName.text = PhotonNetwork.LocalPlayer.NickName;
            }
        }
    }

    private void SetHeadAvatar()
    {
        transform.SetParent(CenterEye);
        transform.localPosition = Vector3.zero;
    }

    private void SetHand(GameObject hand, Transform anchor)
    {
        hand.transform.SetParent(anchor);
        hand.transform.localPosition = Vector3.zero;
        hand.transform.localRotation = anchor.localRotation;
    }
}
