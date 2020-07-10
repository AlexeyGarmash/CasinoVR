using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlase : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                
            }
        }
    }
}
