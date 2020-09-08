using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHandleRadialMenu : MonoBehaviour
{
    public RadialMenuHandV2 RadialMenuHand;
    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            RadialMenuHand.InvokeMenu();
        }
        else
        {
            RadialMenuHand.RevokeMenu();
        }
    }
}
