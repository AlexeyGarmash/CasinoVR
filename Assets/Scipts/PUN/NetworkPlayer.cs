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
    [SerializeField] private Component[] ComponentsToDisable;
    
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
                SetHand(LeftHand, LeftHandAnchor, 180f, 90f);
                SetHand(RightHand, RightHandAnchor, -180f, -90f);
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


    private void LoadAvatar()
    {
        if(PhotonPlayerSettings.Instance != null)
        {

        }
    }
    private void SetHeadAvatar()
    {
        transform.SetParent(CenterEye);
        transform.localPosition = Vector3.zero;
    }

    private void SetHand(GameObject hand, Transform anchor, float yRot, float zRotation)
    {
        hand.transform.SetParent(anchor);
        hand.transform.localPosition = Vector3.zero;
        hand.transform.rotation = Quaternion.Euler(0f, yRot, zRotation);
        OVRGrabberCustom grabberCustom = hand.GetComponent<OVRGrabberCustom>();
        if(grabberCustom != null)
        {
            Transform spawnPoint = anchor.Find("Spawn");
            if (spawnPoint != null)
            {
                grabberCustom.grabbleObjSpawnPoint = spawnPoint;
            }
        }
    }
}
