using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsSmoothView : MonoBehaviourPun, IPunObservable
{
    private Rigidbody _rigidbody;
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    //private NetworkInfo _networkInfo;
    //private Collider _collider;
    //private OVRGrabbableCustom _ovrGrabbableCustom;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_collider = GetComponent<Collider>();
        //_networkInfo = GetComponent<NetworkInfo>();
        //_ovrGrabbableCustom = GetComponent<OVRGrabbableCustom>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            
            
           
            //stream.SendNext(_networkInfo.isGrabbed);

            //if (!_networkInfo.isGrabbed)
            stream.SendNext(_rigidbody.isKinematic);
            stream.SendNext(_rigidbody.position);
            stream.SendNext(_rigidbody.rotation);
            stream.SendNext(_rigidbody.velocity);
        }
        else
        {
           
            //_networkInfo.isGrabbed = ((bool)stream.ReceiveNext());

            _rigidbody.isKinematic = (bool)stream.ReceiveNext();


            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            _rigidbody.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            networkPosition += (_rigidbody.velocity * lag);
        }
    }

    public void FixedUpdate()
    {      
            if (!photonView.IsMine )
            {
                _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, networkPosition, Time.fixedDeltaTime);
                _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
            }
    }
}
