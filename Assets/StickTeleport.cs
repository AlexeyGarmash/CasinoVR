using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickTeleport : MonoBehaviour
{
    private Vector3 StartPos;
    private Quaternion StartRot;

    OVRGrabbableCustom grabberCustom;

    bool isNeedTeleport = false;
    void Start()
    {
        StartPos = transform.position;
        StartRot = transform.rotation;

        grabberCustom = GetComponent<OVRGrabbableCustom>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabberCustom.isGrabbed)
        {
            StopAllCoroutines();
            isNeedTeleport = true;
        }

        if (isNeedTeleport && !grabberCustom.isGrabbed)
            StartCoroutine(WaitForTeleport());
        
    }

    IEnumerator WaitForTeleport()
    {
        yield return new WaitForSeconds(5f);
        transform.position = StartPos;
        transform.rotation = StartRot;

        isNeedTeleport = false;
    }
}
