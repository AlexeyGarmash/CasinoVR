using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TshirtTextureItem : MonoBehaviour
{
    public Action<Texture> OnTextureChanged;
    public Texture TshirtTexture;
    public Button BtnTexture;
    public RawImage ImageTexture;

    private void Start()
    {
        
        BtnTexture.onClick.AddListener(OnBtnClicked);
    }

    private void OnBtnClicked()
    {
        OnTextureChanged.Invoke(TshirtTexture);
        PhotonPlayerSettings.Instance.DressTexture = TshirtTexture;
    }

    public void SetUiTextures(Texture tex)
    {
        ImageTexture = GetComponent<RawImage>();
        ImageTexture.texture = tex;
        TshirtTexture = tex;
    }
}
