using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMirroring : MonoBehaviour
{
    [SerializeField] Transform CenterEyeCamera;
    [SerializeField] Transform Leader;

    Vector3 _followOffset;

    private void Start()
    {
        _followOffset = transform.position - Leader.position;
    }
    private void Update()
    {
        Vector3 targetPosition = Leader.position + _followOffset;
        transform.position += (targetPosition - transform.position);
        if (CenterEyeCamera != null)
        {
            transform.rotation = CenterEyeCamera.transform.rotation;//Quaternion.Inverse(CenterEyeCamera.transform.rotation);
            //transform.LookAt(CenterEyeCamera.position);
        }
    }
}
