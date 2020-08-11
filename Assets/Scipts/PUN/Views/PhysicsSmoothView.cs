using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsSmoothView : MonoBehaviourPun, IPunObservable
{
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
          
    }
    Vector3 position;
    Quaternion rotation;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {                  

            stream.SendNext(_rigidbody.position);
            stream.SendNext(_rigidbody.rotation);
            //stream.SendNext(_rigidbody.velocity);
        }
        else
        {
                           

            var obj = stream.ReceiveNext();
            position = (Vector3)obj;
            rotation = (Quaternion)stream.ReceiveNext();
            
            //networkRotation = (Quaternion)stream.ReceiveNext();
            //_rigidbody.velocity = (Vector3)stream.ReceiveNext();

            //float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            //networkPosition += (_rigidbody.velocity * lag);
        }
    }

    public void Update()
    {      

        if (photonView.Synchronization != ViewSynchronization.Off)
            if (!photonView.IsMine)
            {
                transform.SetPositionAndRotation(position, rotation);
                //_rigidbody.position = Vector3.MoveTowards(_rigidbody.position, networkPosition, Time.fixedDeltaTime);
                //_rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
            }
    }


    
}
