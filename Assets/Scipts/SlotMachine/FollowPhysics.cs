using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPhysics : MonoBehaviour
{
    public Transform target;
    Rigidbody rb;
    public bool startGrab = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GetComponentInChildren<FortuneOfWheelGrabbable>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (startGrab && rb != null)
        {
            rb.MovePosition(target.transform.position);
        }
    }
}
