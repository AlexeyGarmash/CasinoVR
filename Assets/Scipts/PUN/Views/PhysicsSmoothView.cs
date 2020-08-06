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
    private NetworkInfo NetworkInfo;
    private Collider _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        NetworkInfo = GetComponent<NetworkInfo>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_collider.enabled);
            stream.SendNext(gameObject.activeSelf);
            stream.SendNext(_rigidbody.isKinematic);

            stream.SendNext(_rigidbody.position);
            stream.SendNext(_rigidbody.rotation);
            stream.SendNext(_rigidbody.velocity);
        }
        else
        {
            _collider.enabled = ((bool)stream.ReceiveNext());
            gameObject.SetActive((bool)stream.ReceiveNext());
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
        if (!photonView.IsMine && NetworkInfo.Synchronization != ViewSynchronization.Off )
        {
            _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, networkPosition, Time.fixedDeltaTime);
            _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
        }
    }
}
