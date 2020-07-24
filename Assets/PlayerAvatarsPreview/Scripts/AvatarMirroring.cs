using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMirroring : MonoBehaviour
{
    [SerializeField] Transform CenterEyeCamera;


    private void Update()
    {
        if (CenterEyeCamera != null)
        {
            transform.rotation = Quaternion.Inverse(CenterEyeCamera.transform.rotation);
        }
    }
}
