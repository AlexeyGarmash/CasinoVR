using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VibrationItem : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("this object pointer enter");
        VibrationManager.Instance.VibrateController(1, 2, 150, OVRInput.Controller.RTouch);
    }
}
