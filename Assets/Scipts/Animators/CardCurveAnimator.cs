using Cards;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scipts.Animators
{
    class CardCurveAnimator : CurveAnimator
    {

        private new void Start()
        {
            ObjectToAnimation = new List<GameObject>();
        }
        public void StartAnimCardToPlayerWithInstantiate(int curveNumber, string owner, CardData card)
        {

            if (photonView.IsMine)
            {
                var item = PhotonNetwork.Instantiate(CardUtils.Instance.GetPathToCard(card), transform.position, transform.rotation);
                item.GetComponent<CardData>().SetOwner_Photon(owner);
                animStarted = true;
                photonView.RPC("StartAnimation_RPC", RpcTarget.All, curveNumber);
            }
        }
        public void StartAnimCardToPlayer(int curveNumber)
        {

            
            animStarted = true;
            StartAnimation_RPC(curveNumber);
            //photonView.RPC("StartAnimation_RPC", RpcTarget.All, curveNumber);
            
        }
    }
}
