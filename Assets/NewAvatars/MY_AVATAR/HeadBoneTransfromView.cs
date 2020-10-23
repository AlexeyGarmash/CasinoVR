using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBoneTransfromView : MonoBehaviourPun, IPunObservable
{
    private float m_Angle;
    private Quaternion m_NetworkRotation;
    bool m_firstTake = false;
    public void Awake()
    {
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, this.m_NetworkRotation, this.m_Angle * (1.0f / PhotonNetwork.SerializationRate));
        }
    }

    void WritingRotation(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SendNext(transform.rotation);
    }


    void ReadingRotation(PhotonStream stream, PhotonMessageInfo info)
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            WritingRotation(stream, info);
        }
        else
        {
            ReadingRotation(stream, info);
            if (m_firstTake)
            {
                m_firstTake = false;
            }
        }
    }
}
