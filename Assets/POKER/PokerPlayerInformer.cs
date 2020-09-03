using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PokerPlayerInformer : MonoBehaviour
{
    [SerializeField] private TMP_Text textInfo;

    private void Start()
    {
        if (textInfo == null) textInfo = GetComponentInChildren<TMP_Text>();
    }

    public void SetInfo(string text)
    {
        textInfo.text = text;
    }
}
