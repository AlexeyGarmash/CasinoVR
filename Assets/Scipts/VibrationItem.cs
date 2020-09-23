using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VibrationItem : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private int _iteration = 40;
    [SerializeField] private int _frequency = 2;
    [SerializeField] private int _strength = 255;

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("this object pointer enter");
        //VibrationManager.Instance.VibrateController(_iteration, _frequency, _strength, OVRInput.Controller.RTouch);
        OpenVrHapstick.DoVibration(0.2f);
    }
}
