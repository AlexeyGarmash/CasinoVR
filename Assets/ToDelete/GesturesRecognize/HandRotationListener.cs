using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRotationListener : MonoBehaviour
{

    [SerializeField] private Transform _centerEye;

    public float dot = 0f;
    public float upDot = 0f;
    // Update is called once per frame
    void Update()
    {
        Vector3 forwardCenterEye = _centerEye.TransformDirection(Vector3.forward);
        Vector3 upCenterEye = _centerEye.TransformDirection(Vector3.up);
        Vector3 toHand = transform.position - _centerEye.position;

        dot = Vector3.Dot(forwardCenterEye, toHand);
        upDot = Vector3.Dot(upCenterEye, toHand);
        if (dot > 0.29f)
        {
            if(transform.eulerAngles.z >= 250 && transform.eulerAngles.z <= 300)
            {
                print("hand look up");
            }
        }
    }
}
