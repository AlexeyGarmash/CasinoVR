﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using TMPro.Examples;
using System;
using static PhotonPlayerSettings;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{
    private Transform TrackingSpace;
    private Transform CenterEye;
    private Transform OvrCameraRigTransform;
    private Transform RightHandAnchor;
    private Transform LeftHandAnchor;
    [SerializeField] private GameObject Avatar;
    [SerializeField] private GameObject RightHand;
    [SerializeField] private GameObject LeftHand;
    [SerializeField] private TextMeshPro TextNickName;
    [SerializeField] private Component[] ComponentsToDisable;





    //New
    public OvrAvatarBody CurrentBody;
    public Material skinMaterial;
    public Material hairMaterial;
    public Material dressMaterial;




    [PunRPC]
    private void SetPlayerStatrs(int money, string nickname)
    {
        var stats = GetComponent<PlayerStats>();        
        stats.AllMoney = money;
        stats.PlayerNick = nickname;
    }

    /*void Awake()
    {
        var VRRig = GetComponentInChildren<VRRig>();

        if (photonView != null && photonView.IsMine)
        {
            Debug.Log("playernetwork create");
            GameObject globalVRController = GameObject.Find("OVRPlayerControllerMain");
            if(globalVRController != null)
            {              

                Transform ovrControllerTransform = globalVRController.transform;
                
                OvrCameraRigTransform = ovrControllerTransform.Find("OVRCameraRig").transform;

                Transform controllerLeft, controllerRight;

                if (OVRManager.XRDevice.OpenVR == OVRManager.loadedXRDevice)
                {
                    ovrControllerTransform.Find("ControllerLeftOculus").gameObject.SetActive(false);
                    ovrControllerTransform.Find("ControllerRightOculus").gameObject.SetActive(false);
                    

                    controllerLeft = ovrControllerTransform.Find("ControllerLeftVive").transform;
                    controllerRight = ovrControllerTransform.Find("ControllerRightVive").transform;
                }
                else {

                    ovrControllerTransform.Find("ControllerLeftVive").gameObject.SetActive(false);
                    ovrControllerTransform.Find("ControllerRightVive").gameObject.SetActive(false);                 

                    controllerLeft = ovrControllerTransform.Find("ControllerLeftOculus").transform;
                    controllerRight = ovrControllerTransform.Find("ControllerRightOculus").transform;
                }

                var leftControllerHolder = transform.Find("Robot Kyle/VRConstraints/Left Arm IK/TargetLeft").transform;
                controllerLeft.parent = leftControllerHolder;
                controllerLeft.transform.localPosition = Vector3.zero;
                controllerLeft.transform.localRotation = Quaternion.identity;

                var rightControllerHolder = transform.Find("Robot Kyle/VRConstraints/Right Arm IK/TargetRight").transform;
                controllerRight.parent = rightControllerHolder;
                controllerRight.transform.localPosition = Vector3.zero;
                controllerRight.transform.localRotation = Quaternion.identity;

                controllerLeft.GetComponent<TransparentByDistance>().target = LeftHand.transform;
                controllerRight.GetComponent<TransparentByDistance>().target = RightHand.transform;

                photonView.RPC("SetPlayerStatrs", RpcTarget.All, 1000, PhotonNetwork.LocalPlayer.NickName);

                CenterEye = ovrControllerTransform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform;
                RightHandAnchor = ovrControllerTransform.Find("OVRCameraRig/TrackingSpace/RightHandAnchor").transform;
                LeftHandAnchor = ovrControllerTransform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").transform;


                VRRig.head.vrTarget = CenterEye;
                VRRig.RightHand.vrTarget = RightHandAnchor;
                VRRig.leftHand.vrTarget = LeftHandAnchor;             

                OvrCameraRigTransform.parent = transform;
                OvrCameraRigTransform.localPosition = Vector3.zero;
                transform.parent = globalVRController.transform;
                transform.localPosition = Vector3.zero;

               

                
            }
        }

        if (!photonView.IsMine)
        {
            Debug.Log("Network player not is mine" + PhotonNetwork.LocalPlayer.NickName);
            Debug.Log("Owner" + photonView.Owner.NickName);
            TextNickName.text = photonView.Owner.NickName;

            foreach (var component in ComponentsToDisable)
            {
                Destroy(component);
            }

            Avatar.transform.parent = transform;
            RightHand.transform.parent = transform;
            LeftHand.transform.parent = transform;

            Avatar.layer = 0;

            VRRig.head.vrTarget = Avatar.transform;
            VRRig.RightHand.vrTarget = RightHand.transform;
            VRRig.leftHand.vrTarget = LeftHand.transform;
        }
    }*/

    private void Start()
    {
        if (photonView != null && photonView.IsMine)
        {
            RestoreMaterialData(true);
            OvrCameraRigTransform.localPosition = Vector3.zero;
        }
    }

    void Awake()
    {
        if (photonView && photonView.IsMine)
        {
            GameObject globalVRController = GameObject.Find("OVRPlayerControllerMain");

            if (globalVRController)
            {
                Transform ovrControllerTransform = globalVRController.transform;

                OvrCameraRigTransform = ovrControllerTransform.Find("OVRCameraRig").transform;

                TrackingSpace = ovrControllerTransform.Find("OVRCameraRig/TrackingSpace").transform;
                CenterEye = ovrControllerTransform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor").transform;
                RightHandAnchor = ovrControllerTransform.Find("OVRCameraRig/TrackingSpace/RightHandAnchor").transform;
                LeftHandAnchor = ovrControllerTransform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").transform;

                SetHeadAvatar();
                SetHand(LeftHand, LeftHandAnchor, 180f, 90f);
                SetHand(RightHand, RightHandAnchor, -180f, -90f);

                OvrCameraRigTransform.parent = transform;
                OvrCameraRigTransform.localPosition = Vector3.zero;

                transform.parent = globalVRController.transform;
                transform.localPosition = Vector3.zero;

                Debug.LogError("PhotonNetwork.LocalPlayer.NickName-> " + PhotonNetwork.LocalPlayer.NickName);
                photonView.RPC("SetPlayerStatrs", RpcTarget.All, 1000, PhotonNetwork.LocalPlayer.NickName);

                TextNickName.gameObject.SetActive(false);
                //SetParentForControllers(ovrControllerTransform);


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

    private void SetParentForControllers(Transform ovrControllerTransform)
    {
        Transform controllerLeft, controllerRight;

        
        
        if (OVRManager.XRDevice.OpenVR == OVRManager.loadedXRDevice)
        {
            ovrControllerTransform.Find("ControllerLeftOculus").gameObject.SetActive(false);
            ovrControllerTransform.Find("ControllerRightOculus").gameObject.SetActive(false);


            controllerLeft = ovrControllerTransform.Find("ControllerLeftVive").transform;
            controllerRight = ovrControllerTransform.Find("ControllerRightVive").transform;
        }
        else
        {

            ovrControllerTransform.Find("ControllerLeftVive").gameObject.SetActive(false);
            ovrControllerTransform.Find("ControllerRightVive").gameObject.SetActive(false);

            controllerLeft = ovrControllerTransform.Find("ControllerLeftOculus").transform;
            controllerRight = ovrControllerTransform.Find("ControllerRightOculus").transform;
        }

      
        controllerLeft.parent = LeftHandAnchor;
        controllerLeft.transform.localPosition = Vector3.zero;
        controllerLeft.transform.localRotation = Quaternion.identity;

       
        controllerRight.parent = RightHandAnchor;
        controllerRight.transform.localPosition = Vector3.zero;
        controllerRight.transform.localRotation = Quaternion.identity;

        controllerLeft.GetComponent<TransparentByDistance>().target = LeftHand.transform;
        controllerRight.GetComponent<TransparentByDistance>().target = RightHand.transform;
    }

    private void RestoreMaterialData(bool sendToOthers)
    {
        StartCoroutine(WaitUntilLoadAvatar(sendToOthers));
    }

    private IEnumerator WaitUntilLoadAvatar(bool sendToOthers)
    {
        yield return new WaitUntil(() => Avatar.GetComponentInChildren<OvrAvatarBody>() != null);
        yield return null;
        CurrentBody = GetComponentInChildren<OvrAvatarBody>();
        skinMaterial = CurrentBody.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        dressMaterial = CurrentBody.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
        hairMaterial = CurrentBody.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
        skinMaterial.SetColor(OvrAvatarMaterialManager.AVATAR_SHADER_COLOR, PhotonPlayerSettings.Instance.SkinColor);//_BaseColor
        dressMaterial.SetColor(OvrAvatarMaterialManager.AVATAR_SHADER_COLOR, PhotonPlayerSettings.Instance.DressColor);//_BaseColor
        hairMaterial.SetColor(OvrAvatarMaterialManager.AVATAR_SHADER_COLOR, PhotonPlayerSettings.Instance.HairColor);//_BaseColor
        skinMaterial.SetColor(OvrAvatarMaterialManager.AVATAR_SHADER_IRIS_COLOR, PhotonPlayerSettings.Instance.IrisColor);//_MaskColorIris
        skinMaterial.SetColor(OvrAvatarMaterialManager.AVATAR_SHADER_LIP_COLOR, PhotonPlayerSettings.Instance.LipsColor);//_MaskColorIris
        dressMaterial.SetTexture(OvrAvatarMaterialManager.AVATAR_SHADER_MAINTEX, PhotonPlayerSettings.Instance.DressTexture);//_MainTex t-shirt

        if (sendToOthers)
        {
            string jsonSkinData = PhotonPlayerSettings.Instance.GetJsonSkinData();
            print($"SKIN DATA TO SEND {jsonSkinData}");
            photonView.RPC("SendSkinsToOther_RPC", RpcTarget.Others, jsonSkinData);
        }
        
    }

    [PunRPC]
    public void SendSkinsToOther_RPC(string jsonSkinData)
    {
        print($"RECIEVED DATA TO SEND {jsonSkinData}");
        SkinData skinData = PhotonPlayerSettings.Instance.GetSkinData(jsonSkinData);
        var SkinColor = ColorExtensions.FromStringColor(skinData.skinColor);
        var HairColor = ColorExtensions.FromStringColor(skinData.hairColor);
        var IrisColor = ColorExtensions.FromStringColor(skinData.irisColor);
        var DressColor = ColorExtensions.FromStringColor(skinData.dressColor);
        var LipsColor = ColorExtensions.FromStringColor(skinData.lipsColor);
        RestoreRemoteMatarialData(SkinColor, HairColor, IrisColor, DressColor, LipsColor);
    }


    private void RestoreRemoteMatarialData(Color skinColor, Color hairColor, Color irisColor, Color dressColor, Color lipsColor)
    {
        StartCoroutine(WaitUntilLoadAvatarRemote(skinColor, hairColor, irisColor, dressColor, lipsColor));
    }

    private IEnumerator WaitUntilLoadAvatarRemote(Color skinColor, Color hairColor, Color irisColor, Color dressColor, Color lipsColor)
    {
        yield return new WaitUntil(() => Avatar.GetComponentInChildren<OvrAvatarBody>() != null);
        yield return null;
        CurrentBody = GetComponentInChildren<OvrAvatarBody>();
        skinMaterial = CurrentBody.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        dressMaterial = CurrentBody.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
        hairMaterial = CurrentBody.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
        skinMaterial.SetColor(OvrAvatarMaterialManager.AVATAR_SHADER_COLOR, skinColor);//_BaseColor
        dressMaterial.SetColor(OvrAvatarMaterialManager.AVATAR_SHADER_COLOR, dressColor);//_BaseColor
        hairMaterial.SetColor(OvrAvatarMaterialManager.AVATAR_SHADER_COLOR, hairColor);//_BaseColor
        skinMaterial.SetColor(OvrAvatarMaterialManager.AVATAR_SHADER_IRIS_COLOR, irisColor);//_MaskColorIris
        skinMaterial.SetColor(OvrAvatarMaterialManager.AVATAR_SHADER_LIP_COLOR, lipsColor);//_MaskColorIris
        dressMaterial.SetTexture(OvrAvatarMaterialManager.AVATAR_SHADER_MAINTEX, PhotonPlayerSettings.Instance.DressTexture);//_MainTex t-shirt
    }

    private void SetHeadAvatar()
    {
        Avatar.transform.SetParent(TrackingSpace);//CenterEye
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
