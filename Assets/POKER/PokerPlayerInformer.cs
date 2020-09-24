using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class PokerPlayerInformer : MonoBehaviour
{
    [SerializeField] private TMP_Text textInfo;
    [SerializeField] private Transform playerHeadTransform;

    private void Start()
    {
        if (textInfo == null) textInfo = GetComponentInChildren<TMP_Text>();
    }

    public void SetInfo(string text)
    {
        textInfo.text = text;
    }

    private void Update()
    {
        if(playerHeadTransform != null)
        {
            transform.LookAt(2 * transform.position - playerHeadTransform.position);
        }
    }
}
