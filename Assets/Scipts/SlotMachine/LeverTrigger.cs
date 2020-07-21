using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class LeverTrigger : MonoBehaviour
    {
        public SlotMachineManager slotMachineManager;
        
        void OnTriggerEnter(Collider other)
        {
            Debug.LogWarning("Trigger Enter");
           if(other.gameObject.tag == "lever")
                slotMachineManager.StartSlotGame();
        }

        
    }

}