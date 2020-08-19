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
        public override bool MagnetizeObject(GameObject Object, StackData Stack)
        {
            var photonView = Object.GetComponent<PhotonView>();
            //photonView.ObservedComponents.Clear();
            var rb = Object.GetComponent<Rigidbody>();
            var chip = Object.GetComponent<CardData>();


            var stackData = Stack;
            
            if (stackData.playerName.Equals(chip.Owner) || stackData.playerName == "")
            {
                if (stackData.playerName == "")
                    stackData.playerName = chip.Owner;

                rb.isKinematic = true;
                chip.transform.parent = stackData.transform;

                //Debug.Break();

                stackData.Objects.Add(Object);
                stackData.animator.StartAnim(Object);


                return true;


            }

            return false;
        }

        private new void OnTriggerEnter(Collider other)
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
                MagnetizeObject(gameObj, clossest);

            }

            
        }
        
    }
}
