using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DepthOfFieldController : MonoBehaviour
{

    RaycastHit hit;
    Ray raycast;
    bool isHit;
    float hitDistance;
    public LayerMask ignore;

    public PostProcessVolume volume;

    DepthOfField depthOfField;

    
    public float focusSpeed = 6;
    public float maxFocusDistance = 100f;
    private void Start()
    {
        volume.profile.TryGetSettings(out depthOfField);
    }
    void Update()
    {
        raycast = new Ray(transform.position, transform.forward * maxFocusDistance);

        isHit = false;

        if (Physics.Raycast(raycast, out hit, maxFocusDistance, ignore))
        {
            isHit = true;
            hitDistance = Vector3.Distance(transform.position, hit.point);

        }
        else {
            if (hitDistance < maxFocusDistance)
                hitDistance++;
        }

        SetFocus();
    }

    private void SetFocus()
    {
        depthOfField.focusDistance.value = Mathf.Lerp(depthOfField.focusDistance.value, hitDistance, Time.deltaTime * focusSpeed);
    }

    private void OnDrawGizmos()
    {

        if (isHit)
        {
            Gizmos.DrawSphere(hit.point, 0.1f);

            Debug.DrawRay(transform.position, transform.forward * Vector3.Distance(transform.position, hit.point));
        }
        else {
            Debug.DrawRay(transform.position, transform.forward * 100f);
        }
    }
}
