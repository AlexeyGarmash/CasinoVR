using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class ReelTrigger3 : MonoBehaviour,IListener<SLOT_MACHINE_EVENT>
    {
        public int ReelNumber;
        EventManager<SLOT_MACHINE_EVENT> em;
        public void OnEvent(SLOT_MACHINE_EVENT Event_type, Component Sender, params object[] Param)
        {
            switch (Event_type)
            {
                case SLOT_MACHINE_EVENT.REELSTOP2:
                    GetComponent<Collider>().enabled = true;
                   
                    break;
            }
        }
        private void Start()
        {
            em = gameObject.transform.parent.GetComponent<SlotMachineManager>().em;
            GetComponent<Collider>().isTrigger = false;
            em.AddListener(SLOT_MACHINE_EVENT.REELSTOP2, this);
        }
        void OnTriggerEnter(Collider other)
        {
            //Debug.Log(ReelNumber + " " + other.gameObject.tag);
            em.PostNotification(SLOT_MACHINE_EVENT.REEL_SYMBOL_TRIGGERED, this, ReelNumber, other.gameObject.tag);
        }

    }

}