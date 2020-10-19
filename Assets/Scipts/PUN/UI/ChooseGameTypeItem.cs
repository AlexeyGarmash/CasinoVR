using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGameTypeItem : MonoBehaviour
{
    public Action<ChooseGameTypeItem> OnGameTypeChoosen;

    public TMP_Text TextGameType;
    public RawImage ChoosenFrame;
    public string GameTypeName;
    public NetworkManager.CasinoGameTypes GameType;
    public Button BtnChoose;

    public bool TypeChoosen;


    private void Start()
    {
        TextGameType.text = GameTypeName;
        ChoosenFrame.enabled = TypeChoosen;
        BtnChoose = GetComponent<Button>();
        BtnChoose.onClick.AddListener(OnButtonChooseClicked);
    }

    private void OnButtonChooseClicked()
    {
        PhotonPlayerSettings.Instance.CurrentGameType = GameType;
        OnGameTypeChoosen.Invoke(this);
    }

    public void ChooseGameType(bool choose)
    {
        TypeChoosen = choose;
        ChoosenFrame.enabled = TypeChoosen;
    }
}
