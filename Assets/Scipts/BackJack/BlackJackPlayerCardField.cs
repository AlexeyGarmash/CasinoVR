using Cards;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scipts.BackJack
{
    public class BlackJackPlayerCardField : AbstractField
    {
        

        private void OnTriggerEnter(Collider other)
        {    

            var gameObj = other.gameObject;
            var card = gameObj.GetComponent<CardData>();
            var gc = gameObj.GetComponent<OVRGrabbableCustom>();
            var rb = gameObj.GetComponent<Rigidbody>();
            var view = gameObj.GetComponent<PhotonView>();

            
            if (card.IsNotNull() && gc.IsNotNull() && !gc.isGrabbed && !rb.isKinematic && view.IsNotNull())
            {
                var clossest = FindClossestField(card.transform, FindPossibleFields(card));

                if (TriggerLocal)                                                  
                    MagnetizeObject(gameObj, clossest, "CardField", true);
                else if(photonView.IsMine)
                    MagnetizeObject(gameObj, clossest, "CardField");               
            }
            

            
        }

        public GameObject ExtractObject(CardData cost)
        {
            StackData stack = null;
            GameObject Obj = null;
            Stacks.ToList().ForEach(s => s.Objects.ForEach(o => {
                CardData cd = o.GetComponent<CardData>();
                if (cd.Sign == cost.Sign && cd.Face == cost.Face)
                {
                    stack = s;
                    Obj = o;
                }

            }));

            stack?.ExtractOne(Obj);

            return Obj;
        }
        
    }
}
