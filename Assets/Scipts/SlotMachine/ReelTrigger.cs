using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class ReelTrigger : MonoBehaviour
    {
        public int ReelNumber;
        EventManager<SLOT_MACHINE_EVENT> em;

        private void Start()
        {
            em = gameObject.transform.parent.GetComponent<SlotMachineManager>().em;
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log(ReelNumber + " " + other.gameObject.tag);
            em.PostNotification(SLOT_MACHINE_EVENT.REEL_SYMBOL_TRIGGERED, this, ReelNumber, other.gameObject.tag);
        }

    }

}