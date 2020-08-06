using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTransformView : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    
    void Update()
    {
        if(!photonView.IsMine)
        {
            transform.position = Vector3.MoveTowards(transform.position, networkPosition, Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, networkRotation, Time.deltaTime * 100.0f);
        }
    }

}
