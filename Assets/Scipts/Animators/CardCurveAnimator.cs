using Cards;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scipts.Animators
{
    class CardCurveAnimator : CurveAnimator
    {

        public void StartAnimCardToPlayer(int playerPlaceNumber, string owner, CardData card)
        {
            var item = PhotonNetwork.Instantiate(CardUtils.Instance.GetPathToCard(card), transform.position, transform.rotation);
            item.GetComponent<CardData>().SetOwner_Photon(owner);
            photonView.RPC("StartAnimation_RPC", RpcTarget.All, playerPlaceNumber);
        }
    }
}
