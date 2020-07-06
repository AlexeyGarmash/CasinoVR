using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class ReelTrigger : MonoBehaviour
    {
        public int ReelNumber;
        EventManager em;

        private void Start()
        {
            em = gameObject.transform.parent.GetComponent<EventManager>();
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log(ReelNumber + " " + other.gameObject.tag);
            em.PostNotification(EVENT_TYPE.REEL_SYMBOL_TRIGGERED, this, ReelNumber, other.gameObject.tag);
        }

    }

}