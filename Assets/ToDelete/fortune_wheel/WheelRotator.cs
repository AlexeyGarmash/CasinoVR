using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    public FortuneHandler[] fortuneHandlers;


    public void DisableCrazy(FortuneHandler noAction)
    {
        foreach (var handler in fortuneHandlers)
        {
            if(handler != noAction)
            {
                handler.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
