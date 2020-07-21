using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteAvatarNetworkPlayer : MonoBehaviourPun
{
    [SerializeField] private string _ovrControllerName;

    void Start()
    {
        if (photonView != null && photonView.IsMine)
        {
            GameObject globalVRController = GameObject.Find(_ovrControllerName);
            if (globalVRController != null)
            {
                Transform ovrControllerTransform = globalVRController.transform;
                Transform CameraRig = ovrControllerTransform.Find("OVRCameraRig").transform;
                transform.SetParent(CameraRig, false);
            }
        }
    }
}
