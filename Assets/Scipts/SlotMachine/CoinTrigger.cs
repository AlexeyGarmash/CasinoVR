using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class CoinTrigger : MonoBehaviour
    {
        public static string COIN_TAG = "game_coin";
        EventManager<SLOT_MACHINE_EVENT> em;
        private void Start()
        {
            em = gameObject.transform.parent.GetComponent<SlotMachineManager>().em;
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == COIN_TAG)
            {
                Destroy(other.gameObject);
                //TODO: Play music

                em.PostNotification(SLOT_MACHINE_EVENT.COIN_INSERTED, this, null);
            }
        }
    }

}