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
    class BlackJackPlayerCardField : AbstractField
    {
        public override bool MagnetizeObject(GameObject Object, StackData Stack)
        {
            var photonView = Object.GetComponent<PhotonView>();
            //photonView.ObservedComponents.Clear();
            var rb = Object.GetComponent<Rigidbody>();
            var chip = Object.GetComponent<CardData>();


            var stackData = Stack;
            var transform = stackData.gameObject.transform;
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

        private void OnTriggerEnter(Collider other)
        {
            var cardData = other.GetComponent<CardData>();

            if (cardData)
            {
                
            }
        }
        
    }
}
