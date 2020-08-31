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
            var card = other.gameObject.GetComponent<CardData>();
            var gc = other.gameObject.GetComponent<OVRGrabbableCustom>();
            var rb = other.GetComponent<Rigidbody>();
            var view = gameObj.GetComponent<PhotonView>();

            if (card != null && gc != null && !gc.isGrabbed && !rb.isKinematic && view != null)
            {

                Debug.Log("MagnetizeObject viewID=" + view.ViewID);
                var clossest = FindClossestField(card.transform, FindPossibleFields(card));
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
