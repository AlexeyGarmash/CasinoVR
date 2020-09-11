using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using TMPro.Examples;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{
    private Transform CenterEye;
    private Transform OvrCameraRigTransform;
    private Transform RightHandAnchor;
    private Transform LeftHandAnchor;
    [SerializeField] private GameObject Avatar;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private TextMeshPro TextNickName;
    [SerializeField] private Component[] ComponentsToDisable;

    [PunRPC]
    private void SetPlayerStatrs(int money, string nickname)
    {      
        var stats = GetComponent<PlayerStats>();
        stats.AllMoney = money;
        stats.PlayerNick = nickname;
    }
    void Awake()
    {
        if(photonView != null && photonView.IsMine)
        {
            GameObject globalVRController = GameObject.Find("OVRPlayerControllerMain");
            if(globalVRController != null)
            {
                Transform ovrControllerTransform = globalVRController.transform;
                
                OvrCameraRigTransform = ovrControllerTransform.Find("OVRCameraRig").transform;
                            

                photonView.RPC("SetPlayerStatrs", RpcTarget.All, 1000, PhotonNetwork.LocalPlayer.NickName);

                CenterEye = ovrControllerTransform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform;
                RightHandAnchor = ovrControllerTransform.Find("OVRCameraRig/Robot Kyle/VRConstraints/Right Arm IK/RightHandTarget").transform;
                LeftHandAnchor = ovrControllerTransform.Find("OVRCameraRig/Robot Kyle/VRConstraints/Left Arm IK/LeftHandTarget").transform;
                SetHeadAvatar();
                SetHand(LeftHand, LeftHandAnchor, 0, 0);
                SetHand(RightHand, RightHandAnchor, 0, 0);

                OvrCameraRigTransform.parent = transform;
                transform.parent = globalVRController.transform;
                transform.localPosition = Vector3.zero;
            }
        }

        if (!photonView.IsMine)
        {
            TextNickName.text = photonView.Owner.NickName;
            foreach (var component in ComponentsToDisable)
            {
                Destroy(component);
            }
        }
    }

    private void SetHeadAvatar()
    {
        Avatar.transform.SetParent(CenterEye);
        Avatar.transform.localPosition = Vector3.zero;
        Avatar.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void SetHand(GameObject hand, Transform anchor, float yRot, float zRotation)
    {
        hand.transform.SetParent(anchor);
        hand.transform.localPosition = Vector3.zero;
        hand.transform.rotation = Quaternion.Euler(0f, yRot, zRotation);
        
    }
}
