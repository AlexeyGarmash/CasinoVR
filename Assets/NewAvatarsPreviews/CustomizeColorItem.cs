using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeColorItem : MonoBehaviour
{
    public Action<CustomizeColorItem> OnColorChanged;
    [SerializeField] private Button _btnChooseColor;
    [SerializeField] private Image _imgColor;
    public Color CustomColor;

    private void Start()
    {
        _btnChooseColor.onClick.AddListener(OnBtnChooseColorClicked);
        _imgColor.color = CustomColor;
    }

    private void OnBtnChooseColorClicked()
    {
        OnColorChanged.Invoke(this);
    }
}
