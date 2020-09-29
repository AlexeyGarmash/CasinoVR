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
    }

  
}
