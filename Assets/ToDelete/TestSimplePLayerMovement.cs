using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestSimplePLayerMovement : MonoBehaviourPunCallbacks
{

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (photonView.IsMine)
            {
                transform.Rotate(Vector3.up, Space.Self);
            }
        }
    }
}
