// ----------------------------------------------------------------------------
// <copyright file="PhotonTransformView.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
//   Component to synchronize Transforms via PUN PhotonView.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------


namespace Photon.Pun
{
    using UnityEngine;

    
   
    [AddComponentMenu("Photon Networking/Photon Transform View")]
    [HelpURL("https://doc.photonengine.com/en-us/pun/v2/gameplay/synchronization-and-state")]
    public class PhotonTransformView : MonoBehaviourPun, IPunObservable
    {
        private float m_Distance;
        private float m_Angle;

        private Vector3 m_Direction;
        private Vector3 m_NetworkPosition;
        private Vector3 m_StoredPosition;

        private Quaternion m_NetworkRotation;

        public bool m_SynchronizeGlobalPosition = true;
        public bool m_SynchronizeGlobalRotation = true;
        public bool m_SynchronizeScale = false;

        bool m_firstTake = false;

        public void Awake()
        {
            m_StoredPosition = transform.localPosition;
            m_NetworkPosition = Vector3.zero;
            m_NetworkRotation = Quaternion.identity;
        }

        void OnEnable()
        {
            m_firstTake = true;
        }

        public void Update()
        {
            if (!this.photonView.IsMine)
            {
                if(!m_SynchronizeGlobalPosition)
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, this.m_NetworkPosition, this.m_Distance * (1.0f / PhotonNetwork.SerializationRate));
                else if (m_SynchronizeGlobalPosition)
                    transform.position = Vector3.MoveTowards(transform.position, this.m_NetworkPosition, this.m_Distance * (1.0f / PhotonNetwork.SerializationRate));

                if (!m_SynchronizeGlobalRotation)
                    transform.localRotation = Quaternion.RotateTowards(transform.localRotation, this.m_NetworkRotation, this.m_Angle * (1.0f / PhotonNetwork.SerializationRate));
                if (m_SynchronizeGlobalRotation)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, this.m_NetworkRotation, this.m_Angle * (1.0f / PhotonNetwork.SerializationRate));
            }
        }


        void WritingPosition(PhotonStream stream, PhotonMessageInfo info)
        {
            if (!this.m_SynchronizeGlobalPosition)
            {
                this.m_Direction = transform.localPosition - this.m_StoredPosition;
                this.m_StoredPosition = transform.localPosition;

                stream.SendNext(transform.localPosition);
                stream.SendNext(this.m_Direction);
            }
            else if (this.m_SynchronizeGlobalPosition )
            {
                this.m_Direction = transform.position - this.m_StoredPosition;
                this.m_StoredPosition = transform.position;

                stream.SendNext(transform.position);
                stream.SendNext(this.m_Direction);
            }
        }
        void WritingRotation(PhotonStream stream, PhotonMessageInfo info)
        {
            if (!this.m_SynchronizeGlobalRotation)
            {
                stream.SendNext(transform.localRotation);
            }
            else if (this.m_SynchronizeGlobalRotation)
            {
                stream.SendNext(transform.rotation);
            }
        }
        void WritingScale(PhotonStream stream, PhotonMessageInfo info)
        {
            if (this.m_SynchronizeScale)
            {
                stream.SendNext(transform.localScale);
            }
        }

        void ReadingPosition(PhotonStream stream, PhotonMessageInfo info)
        {
            if (!this.m_SynchronizeGlobalPosition)
            {
                this.m_NetworkPosition = (Vector3)stream.ReceiveNext();
                this.m_Direction = (Vector3)stream.ReceiveNext();

                if (m_firstTake)
                {
                    transform.localPosition = this.m_NetworkPosition;
                    this.m_Distance = 0f;
                }
                else
                {
                    float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                    this.m_NetworkPosition += this.m_Direction * lag;
                    this.m_Distance = Vector3.Distance(transform.localPosition, this.m_NetworkPosition);
                }

            }
            else if (this.m_SynchronizeGlobalPosition)
            {
                this.m_NetworkPosition = (Vector3)stream.ReceiveNext();
                this.m_Direction = (Vector3)stream.ReceiveNext();

                if (m_firstTake)
                {
                    transform.position = this.m_NetworkPosition;
                    this.m_Distance = 0f;
                }
                else
                {
                    float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                    this.m_NetworkPosition += this.m_Direction * lag;
                    this.m_Distance = Vector3.Distance(transform.position, this.m_NetworkPosition);
                }

            }
        }
        void ReadingRotation(PhotonStream stream, PhotonMessageInfo info)
        {
            if (!this.m_SynchronizeGlobalRotation )
            {
                this.m_NetworkRotation = (Quaternion)stream.ReceiveNext();

                if (m_firstTake)
                {
                    this.m_Angle = 0f;
                    transform.localRotation = this.m_NetworkRotation;
                }
                else
                {
                    this.m_Angle = Quaternion.Angle(transform.localRotation, this.m_NetworkRotation);
                }
            }
            else if (this.m_SynchronizeGlobalRotation)
            {
                this.m_NetworkRotation = (Quaternion)stream.ReceiveNext();

                if (m_firstTake)
                {
                    this.m_Angle = 0f;
                    transform.rotation = this.m_NetworkRotation;
                }
                else
                {
                    this.m_Angle = Quaternion.Angle(transform.rotation, this.m_NetworkRotation);
                }
            }
        }
        void ReadingScale(PhotonStream stream, PhotonMessageInfo info)
        {
            if (this.m_SynchronizeScale)
            {
                transform.localScale = (Vector3)stream.ReceiveNext();
            }
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                WritingPosition(stream, info);

                WritingRotation(stream, info);

                WritingScale(stream, info);
            }
            else
            {

                ReadingPosition(stream, info);

                ReadingRotation(stream, info);

                ReadingScale(stream, info);

                if (m_firstTake)
                {
                    m_firstTake = false;
                }
            }
        }
    }
}