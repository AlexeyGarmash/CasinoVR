using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIK : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;
    

    bool CastRay(Vector3 direction, float distance, Color color)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, distance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(direction) * hit.distance, color);
            Debug.Log("forward Hit");

            if (Vector3.Distance(transform.parent.position, hit.point) < 0.4f)
                transform.position = hit.point;
            else transform.localPosition = Vector3.zero;
            return true;
        }
        return false;
    }
    void Update()
    {
        

        if (CastRay(Vector3.forward, 0.1f, Color.yellow))
            return;

        if (CastRay(Vector3.back, 0.1f, Color.yellow))
            return;

        if (CastRay(Vector3.up, 0.1f, Color.yellow))
            return;

        if (CastRay(Vector3.down, 0.1f, Color.yellow))
            return;

        if (CastRay(Vector3.left, 0.1f, Color.yellow))
            return;

        if (CastRay(Vector3.right, 0.1f, Color.yellow))
            return;

        transform.localPosition = Vector3.zero;
    }
}
