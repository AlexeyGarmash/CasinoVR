using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using TMPro.Examples;
using System;
using static PhotonPlayerSettings;
using static CustomizeAvatarPartV2;

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
            //RestoreMaterialData(true);
            RestoreMaterialDataV2(true);
            OvrCameraRigTransform.localPosition = Vector3.zero;
        }
    }

    private void RestoreMaterialDataV2(bool sendToOthers)
    {
        var avatarBodyHolder = Avatar.GetComponent<AvatarBodyHolder>();
        var beard = PhotonPlayerSettings.Instance.Beard;
        var hair = PhotonPlayerSettings.Instance.Hair;
        var glasses = PhotonPlayerSettings.Instance.Glasses;
        var head = PhotonPlayerSettings.Instance.Head;
        var shirt = PhotonPlayerSettings.Instance.Shirt;

        if (head.IsChanged)
        {
            avatarBodyHolder.ChangeHead(head.Mat, head.Mesh);
        }
        if (beard.IsChanged)
        {
            avatarBodyHolder.ChangeBeard(beard.Mat, beard.Mesh);
        }
        if (hair.IsChanged)
        {
            avatarBodyHolder.ChangeHair(hair.Mat, hair.Mesh);
        }
        if (glasses.IsChanged)
        {
            avatarBodyHolder.ChangeGlasses(glasses.Mat, glasses.Mesh);
        }
        if (shirt.IsChanged)
        {
            avatarBodyHolder.ChangeShirt(shirt.Mat, shirt.Mesh);
        }

        if (sendToOthers)
        {
            string jsonCustAvatarData = PhotonPlayerSettings.Instance.GetCustomizeAvatarJsonData();
            print($"CUSTOMIZE DATA DATA TO SEND {jsonCustAvatarData}");
            photonView.RPC("SendSkinsToOther_RPC", RpcTarget.Others, jsonCustAvatarData);
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
        CustomizeJsonData customize = PhotonPlayerSettings.Instance.RestoreCustomizeDataFromJson(jsonSkinData);

        var avatarBodyHolder = Avatar.GetComponent<AvatarBodyHolder>();
        var beard = customize.Beard;
        var hair = customize.Hair;
        var glasses = customize.Glasses;
        var head = customize.Head;
        var shirt = customize.Shirt;

        if (head.IsChanged)
        {
            var matMesh = LoadMaterialMesh(head, CA_Part.Head);
            avatarBodyHolder.ChangeHead(matMesh.Item1, matMesh.Item2);
        }
        if (beard.IsChanged)
        {
            var matMesh = LoadMaterialMesh(beard, CA_Part.Beard);
            avatarBodyHolder.ChangeBeard(matMesh.Item1, matMesh.Item2);
        }
        if (hair.IsChanged)
        {
            var matMesh = LoadMaterialMesh(hair, CA_Part.Hair);
            avatarBodyHolder.ChangeHair(matMesh.Item1, matMesh.Item2);
        }
        if (glasses.IsChanged)
        {
            var matMesh = LoadMaterialMesh(glasses, CA_Part.Glasses);
            avatarBodyHolder.ChangeGlasses(matMesh.Item1, matMesh.Item2);
        }
        if (shirt.IsChanged)
        {
            var matMesh = LoadMaterialMesh(shirt, CA_Part.Shirt);
            avatarBodyHolder.ChangeShirt(matMesh.Item1, matMesh.Item2);
        }

        /*SkinData skinData = PhotonPlayerSettings.Instance.GetSkinData(jsonSkinData);
        var SkinColor = ColorExtensions.FromStringColor(skinData.skinColor);
        var HairColor = ColorExtensions.FromStringColor(skinData.hairColor);
        var IrisColor = ColorExtensions.FromStringColor(skinData.irisColor);
        var DressColor = ColorExtensions.FromStringColor(skinData.dressColor);
        var LipsColor = ColorExtensions.FromStringColor(skinData.lipsColor);
        var textureName = skinData.textureName;*/

        //RestoreRemoteMatarialData(SkinColor, HairColor, IrisColor, DressColor, LipsColor, textureName);
    }

    private (Material, Mesh) LoadMaterialMesh(C_BodyPart c_BodyPart, CA_Part bodyPartType)
    {
        Material mat = null;
        Mesh mesh = null;
        string gender = c_BodyPart.Gender;
        string matName = c_BodyPart.MaterialName;
        string meshName = c_BodyPart.MeshName;
        switch (bodyPartType)
        {
            case CA_Part.Beard:
                mat = Resources.Load<Material>(string.Format(BaseBeardMaterialsResourcePath, gender, matName));
                mesh = Resources.Load<Mesh>(string.Format(BaseBeardMeshesResourcePath, gender, meshName));
                return (mat, mesh);

            case CA_Part.Glasses:
                mat = Resources.Load<Material>(string.Format(BaseGlassesMaterialsResourcePath, gender, matName));
                mesh = Resources.Load<Mesh>(string.Format(BaseGlassesMeshesResourcePath, gender, meshName));
                return (mat, mesh);

            case CA_Part.Hair:
                mat = Resources.Load<Material>(string.Format(BaseHairMaterialsResourcePath, gender, matName));
                mesh = Resources.Load<Mesh>(string.Format(BaseHairMeshesResourcePath, gender, meshName));
                return (mat, mesh);

            case CA_Part.Head:
                mat = Resources.Load<Material>(string.Format(BaseFaceMaterialsResourcePath, gender, matName));
                mesh = Resources.Load<Mesh>(string.Format(BaseFaceMeshesResourcePath, gender, meshName));
                return (mat, mesh);

            case CA_Part.Shirt:
                mat = Resources.Load<Material>(string.Format(BaseShirtMaterialsResourcePath, gender, matName));
                mesh = Resources.Load<Mesh>(string.Format(BaseShirtMeshesResourcePath, gender, meshName));
                return (mat, mesh);
        }

        return (null, null);
    }


    private void RestoreRemoteMatarialData(Color skinColor, Color hairColor, Color irisColor, Color dressColor, Color lipsColor, string dressTextureName)
    {
        StartCoroutine(WaitUntilLoadAvatarRemote(skinColor, hairColor, irisColor, dressColor, lipsColor, dressTextureName));
    }

    private IEnumerator WaitUntilLoadAvatarRemote(Color skinColor, Color hairColor, Color irisColor, Color dressColor, Color lipsColor, string dressTextureName)
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
        if (dressTextureName.Length > 0)
        {
            Texture dressTexture = Resources.Load<Texture>(PhotonPlayerSettings.BaseTexturesResPath + dressTextureName);
            dressMaterial.SetTexture(OvrAvatarMaterialManager.AVATAR_SHADER_MAINTEX, dressTexture);
        }
        //_MainTex t-shirt
    }

    private void SetHeadAvatar()
    {
        Avatar.layer = 17;
        Avatar.transform.SetParent(TrackingSpace);//CenterEye
        Avatar.transform.localPosition = Vector3.zero;
        var avLocPos = Avatar.transform.localPosition;
        avLocPos.z = -0.2f;
        avLocPos.y = -1.3f;
        Avatar.transform.localPosition = avLocPos;
        Avatar.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void SetHand(GameObject hand, Transform anchor, float yRot, float zRotation)
    {
        hand.transform.SetParent(anchor);
        hand.transform.localPosition = Vector3.zero;
        hand.transform.rotation = Quaternion.Euler(0f, yRot, zRotation);

    }


}
