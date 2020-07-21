using System;
using System.Collections;
using System.Collections.Generic;
using TalesFromTheRift;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyboardHandler : MonoBehaviour
{
    [SerializeField] private Transform SpawnTransform;
    [SerializeField] private TMP_InputField Input;

    private void Awake()
    {
        Input.onSelect.AddListener(OnSelect);
        //Input.onDeselect.AddListener(OnDeselect);
    }

    private void OnSelect(string s)
    {
        CanvasKeyboard.Open(SpawnTransform, Input.gameObject);
    }

    /*private void OnDeselect(string s)
    {
        CanvasKeyboard.Close();
    }*/


}
