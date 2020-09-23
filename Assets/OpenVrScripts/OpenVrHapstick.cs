using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class OpenVrHapstick
{
    public static void DoVibration(float vibroStrength)
    {
        OVRInput.Controller controller = OVRInput.GetActiveController();
        InputDevice XRcontroller;
        if(controller == OVRInput.Controller.RTouch)
        {
            XRcontroller = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            Debug.Log("RIGHT HAND");
        } 
        else
        {
            XRcontroller = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            Debug.Log("LEFT HAND");
        }
        //XRcontroller =InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (XRcontroller.isValid)
        {
            HapticCapabilities hapCap = new HapticCapabilities();
            XRcontroller.TryGetHapticCapabilities(out hapCap);
            Debug.Log("DOES XR CONTROLLER SUPPORT HAPTIC BUFFER: " + hapCap.supportsBuffer);
            Debug.Log("DOES XR CONTROLLER SUPPORT HAPTIC IMPULSE: " + hapCap.supportsImpulse);
            Debug.Log("DOES XR CONTROLLER SUPPORT HAPTIC CHANNELS: " + hapCap.numChannels);
            Debug.Log("DOES XR CONTROLLER SUPPORT HAPTIC BUFFER FREQUENCY HZ: " + hapCap.bufferFrequencyHz);
            if (hapCap.supportsImpulse)
            {
                XRcontroller.SendHapticImpulse(0, vibroStrength, 0.1f);
            }
        } else
        {
            Debug.Log("INVALID XR CONTROLLER");
        }
    }
}
