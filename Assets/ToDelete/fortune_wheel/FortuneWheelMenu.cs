using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FortuneWheelMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text textInfo;

    public void SetInfo(string text, bool useTimer = false, float seconds = 5f)
    {
        textInfo.text = text;
        if(useTimer)
        {
            StartCoroutine(_timer(seconds));
        }
    }

    private IEnumerator _timer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        textInfo.text = "Spin wheel!";
    }
}
