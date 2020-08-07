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
    private ItemNetworkInfo _networkInfo;
    private Collider _collider;
    private OVRGrabbableCustom _ovrGrabbableCustom;


    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _networkInfo = GetComponent<ItemNetworkInfo>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(_collider.enabled);
            //stream.SendNext(gameObject.activeSelf);
            
           //network info 
            stream.SendNext(_networkInfo.isGrabbed);
            stream.SendNext(_networkInfo.Owner);

            if (!_networkInfo.isGrabbed)
                stream.SendNext(_rigidbody.isKinematic);

            stream.SendNext(_rigidbody.position);
            stream.SendNext(_rigidbody.rotation);
            stream.SendNext(_rigidbody.velocity);
        }
        else
        {
            //_collider.enabled = ((bool)stream.ReceiveNext());
            //gameObject.SetActive((bool)stream.ReceiveNext());
            _networkInfo.isGrabbed = ((bool)stream.ReceiveNext());
            _networkInfo.Owner = ((string)stream.ReceiveNext());

            var obj = stream.ReceiveNext();

            if (!_networkInfo.isGrabbed && obj is bool)
            {
                _rigidbody.isKinematic = (bool)obj;
                obj = stream.ReceiveNext();
            }

            networkPosition = (Vector3)obj;
            networkRotation = (Quaternion)stream.ReceiveNext();
            _rigidbody.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            networkPosition += (_rigidbody.velocity * lag);
        }
    }

    public void FixedUpdate()
    {      

        if (photonView.Synchronization != ViewSynchronization.Off)
            if (!photonView.IsMine )
            {
                _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, networkPosition, Time.fixedDeltaTime);
                _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
            }
    }


    public bool IsPhotonSync { get => photonView.Synchronization != ViewSynchronization.Off; }
    public void SyncOff()
    {
        if(photonView.IsMine)
            photonView.RPC("SyncOff_RPC", RpcTarget.All);
    }
    public void SyncOn()
    {
        if (photonView.IsMine)
            photonView.RPC("SyncOn_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void SyncOff_RPC()
    {
        photonView.Synchronization = ViewSynchronization.Off;
    }
    public void SyncOn_RPC()
    {
        photonView.Synchronization = ViewSynchronization.Unreliable;
    }
}
