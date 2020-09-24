using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scipts.Chips
{
    public class OwnerData : MonoBehaviourPun
    {

        public AbstractField field;
        public CurveAnimator animator;

        public string Owner;
        public void SetOwner_Photon(string owner)
        {
            photonView.RPC("SetOwner_RPC", RpcTarget.All, owner);
        }
        [PunRPC]
        public void SetOwner_RPC(string owner)
        {
            Owner = owner;
        }


        public void ExtractObject()
        {
            if (field.IsNotNull())
            {
                
                field.ExtranctObject(photonView.ViewID);
                field = null;
            }
            if (animator.IsNotNull())
            {
                animator.StopObjectAnimation(photonView.ViewID);
                animator = null;
            }
        }


    }
}
