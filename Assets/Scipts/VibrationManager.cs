using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void VibrateController(int iteration, int frequency, int strength, OVRInput.Controller controller)
    {
        OVRHapticsClip vibroClip = new OVRHapticsClip();

        for (int i = 0; i < iteration; i++)
        {
            vibroClip.WriteSample(i % frequency == 0 ? (byte)strength : (byte)0);
        }


        if(controller == OVRInput.Controller.LTouch)
        {
            OVRHaptics.LeftChannel.Preempt(vibroClip);
        }

        if (controller == OVRInput.Controller.RTouch)
        {
            OVRHaptics.RightChannel.Preempt(vibroClip);
        }

    }
}
