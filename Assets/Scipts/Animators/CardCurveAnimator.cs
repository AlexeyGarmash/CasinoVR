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
        public void StartAnimCardToPlayer(int curveNumber, string owner, CardData card)
        {
            
            var item = PhotonNetwork.Instantiate(CardUtils.Instance.GetPathToCard(card), transform.position, transform.rotation);
            item.GetComponent<CardData>().SetOwner_Photon(owner);
            photonView.RPC("StartAnimation_RPC", RpcTarget.All, curveNumber);
        }
    }
}
