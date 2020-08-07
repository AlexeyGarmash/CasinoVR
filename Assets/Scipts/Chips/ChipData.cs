using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ChipData : MonoBehaviourPun
{
    public Chips Cost;
    public string Owner;
    public bool InAnimation;
    public bool isGrabbed;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetOwner_Photon(string owner)
    {
        photonView.RPC("SetOwner_RPC", RpcTarget.All, owner);
    }
    [PunRPC]
    public void SetOwner_RPC(string owner)
    {
        Owner = owner;
    }
    public void IsGragged_Photon(bool grabbed)
    {
        photonView.RPC("IsGragged_RPC", RpcTarget.All, grabbed);
       
    }
    [PunRPC]
    public void IsGragged_RPC(bool grabbed)
    {
        isGrabbed = grabbed;
        rb.isKinematic = grabbed;
    }


}

