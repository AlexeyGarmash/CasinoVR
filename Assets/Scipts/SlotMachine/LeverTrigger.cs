using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class LeverTrigger : MonoBehaviour
    {
        public SlotMachineManager slotMachineManager;
        private string correctTag = "lever";

        void OnTriggerEnter(Collider other)
        {

            Debug.LogWarning("you see me?");
           if(other.gameObject.tag == correctTag)
                slotMachineManager.StartSlotGame();
        }

        
    }

}