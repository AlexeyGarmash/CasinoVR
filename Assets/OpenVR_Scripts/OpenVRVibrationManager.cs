using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class OpenVRVibrationManager
{
    public static void DoVibration(float strength, float duration) 
    {
        var xrControllerRight = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        var xrControllerLeft = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        MakeVibro(xrControllerRight, strength, duration);
        MakeVibro(xrControllerLeft, strength, duration);
    }

    private static void MakeVibro(InputDevice xrController, float strength, float duration)
    {
        if (xrController.isValid)
        {
            HapticCapabilities hapticCapabilities;
            xrController.TryGetHapticCapabilities(out hapticCapabilities);

            if(hapticCapabilities.supportsImpulse)
            {
                uint defaultChannel = 0;
                xrController.SendHapticImpulse(defaultChannel, strength, duration);
            }
        }
    }
}
