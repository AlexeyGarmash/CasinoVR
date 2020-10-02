using ExitGames.Demos.DemoPunVoice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSkinChooser : MonoBehaviour
{
    public static AvatarSkinChooser Instance;

    [SerializeField] Material[] SkinsList;
    [SerializeField] Button ButtonPrev;
    [SerializeField] Button ButtonNext;
    [SerializeField] Renderer ObjectRenderer;

    private int currentChoosenIndex;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        ButtonPrev.onClick.AddListener(OnPrevButtonClicked);
        ButtonNext.onClick.AddListener(OnNextButtonClicked);
        ButtonPrev.gameObject.SetActive(false);
        ButtonNext.gameObject.SetActive(false);
    }

    public void SetupAvatartToChoose(Renderer rend)
    {
        if (rend != null)
        {
            ObjectRenderer = rend;
            ButtonPrev.gameObject.SetActive(true);
            ButtonNext.gameObject.SetActive(true);
        }
    }

    private void OnPrevButtonClicked()
    {
        SelectPrevMaterial();
    }

    private void OnNextButtonClicked()
    {
        SelectNextMaterial();
    }

    public void SelectNextMaterial()
    {
        currentChoosenIndex++;
        if(currentChoosenIndex >= SkinsList.Length)
        {
            currentChoosenIndex = 0;
        }
        SelectSkin();
    }

    public void SelectPrevMaterial()
    {
        currentChoosenIndex--;
        if (currentChoosenIndex < 0)
        {
            currentChoosenIndex = SkinsList.Length - 1;
        }
        SelectSkin();
    }

    private void SelectSkin()
    {
        if (ObjectRenderer != null)
        {
            ObjectRenderer.material = SkinsList[currentChoosenIndex];
        }
    }

}
