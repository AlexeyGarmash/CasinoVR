using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class ReelTrigger2 : MonoBehaviour,IListener<EVENT_TYPE>
    {
        public int ReelNumber;
        EventManager em;

        public void OnEvent(EVENT_TYPE Event_type, Component Sender, params object[] Param)
        {
            switch (Event_type)
            {
                case EVENT_TYPE.REELSTOP1:
                    GetComponent<Collider>().enabled = true;
                    break;
            }
        }
        private void Start()
        {
            em = gameObject.transform.parent.GetComponent<EventManager>();
            GetComponent<Collider>().isTrigger = false;
            em.AddListener(EVENT_TYPE.REELSTOP1, this);
        }
        void OnTriggerEnter(Collider other)
        {

            Debug.Log(ReelNumber + " " + other.gameObject.tag);
            em.PostNotification(EVENT_TYPE.REEL_SYMBOL_TRIGGERED, this, ReelNumber, other.gameObject.tag);
        }

    }

}