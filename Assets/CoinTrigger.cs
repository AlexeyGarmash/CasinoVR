using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class CoinTrigger : MonoBehaviour
    {
        public static string COIN_TAG = "game_coin";
        EventManager em;
        private void Start()
        {
            em = gameObject.transform.parent.GetComponent<EventManager>();
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == COIN_TAG)
            {
                Destroy(other.gameObject);
                //TODO: Play music

                em.PostNotification(EVENT_TYPE.COIN_INSERTED, this, null);
            }
        }
    }

}