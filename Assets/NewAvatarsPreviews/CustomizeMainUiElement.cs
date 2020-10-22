using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CustomizeAvatarPartV2;

public class CustomizeMainUiElement : MonoBehaviour
{
    public Action<CustomizeMainUiElement> OnBodyPartsToActivateChoosen;
    public CustomizePartsChooser CustomizePartChooser;
    [SerializeField] private Button BtnChooseBodyParts;


    private void Start()
    {
        BtnChooseBodyParts.onClick.AddListener(OnBtnChoosePartsClicked);
    }

    private void OnBtnChoosePartsClicked()
    {
        OnBodyPartsToActivateChoosen.Invoke(this);
    }


    public void Activate()
    {
        CustomizePartChooser.Activate();
    }

    public void Deactivate()
    {
        CustomizePartChooser.Deactivate();
    }
}
