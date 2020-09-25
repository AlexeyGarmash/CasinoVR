using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectControllerMenu : MonoBehaviour
{
    public GameObject ViveController;
    public GameObject OculusController;
    void Start()
    {
        if (OVRManager.loadedXRDevice == OVRManager.XRDevice.Oculus)
        {
            OculusController.SetActive(true);
        }
        else {
            ViveController.SetActive(true);
}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
