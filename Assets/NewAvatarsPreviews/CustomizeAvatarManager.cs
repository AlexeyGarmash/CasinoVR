using HSVPicker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeAvatarManager : MonoBehaviour
{
    public enum CustomizePart
    {
        Skin,
        Dress,
        Hair,
        Iris,
        Lips,
        Other
    }
    public static CustomizeAvatarManager Instance;

    [SerializeField] GameObject SettingField;
    [SerializeField] ColorPicker colorPicker;
    [SerializeField] Color currentColor;
    [SerializeField] Button BtnChangeSkin;
    [SerializeField] Button BtnChangeDress;
    [SerializeField] Button BtnChangeHair;
    [SerializeField] Button BtnChangeIris;
    [SerializeField] Button BtnChangeOther;
    [SerializeField] Button BtnChangeLips;

    [SerializeField] TshirtTextureItem[] ListToChangeTshirt;
    [Header("Man1")]
    [SerializeField] Texture[] TexsMan1;
    [Header("Woman1")]
    [SerializeField] Texture[] TexsWoman1;


    public CustomizePart CurrentCustomizePart;

    public PlayerAvatar AvatarInfo;
    public OvrAvatar CurrentAvatar;
    public OvrAvatarBody AvatarBody;
    public Material CurrentMaterial;
    public Material DressMaterial;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        foreach (var item in ListToChangeTshirt)
        {
            item.OnTextureChanged += OnChooseTexture;
        }
        BtnChangeSkin.onClick.AddListener(() => OnButtonChange_Clicked(CustomizePart.Skin));
        BtnChangeDress.onClick.AddListener(() => OnButtonChange_Clicked(CustomizePart.Dress));
        BtnChangeHair.onClick.AddListener(() => OnButtonChange_Clicked(CustomizePart.Hair));
        BtnChangeIris.onClick.AddListener(() => OnButtonChange_Clicked(CustomizePart.Iris));
        BtnChangeLips.onClick.AddListener(() => OnButtonChange_Clicked(CustomizePart.Lips));
        BtnChangeOther.onClick.AddListener(() => OnButtonChange_Clicked(CustomizePart.Other));

        colorPicker.onValueChanged.AddListener(OnColorPickerChangedColor);
        colorPicker.gameObject.SetActive(false);
        SettingField.SetActive(false);
    }

    private void OnChooseTexture(Texture texture)
    {
        DressMaterial.SetTexture("_MainTex", texture);
    }

    private void OnButtonChange_Clicked(CustomizePart customizePart)
    {
        CurrentCustomizePart = customizePart;
        switch (CurrentCustomizePart)
        {
            case CustomizePart.Skin:
                CurrentMaterial = AvatarBody.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
                SaveMaterialTexture(CurrentMaterial, "m_face_");
                break;
            case CustomizePart.Dress:
                CurrentMaterial = AvatarBody.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
                SaveMaterialTexture(CurrentMaterial, "t_shirt_");

                break;
            case CustomizePart.Hair:
                CurrentMaterial = AvatarBody.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
                break;
            case CustomizePart.Iris:
                CurrentMaterial = AvatarBody.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
                break;
            case CustomizePart.Lips:
                CurrentMaterial = AvatarBody.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
                break;
            case CustomizePart.Other:
                CurrentMaterial = AvatarBody.transform.GetChild(3).GetComponent<SkinnedMeshRenderer>().material;
                break;

        }
    }

    private void OnColorPickerChangedColor(Color color)
    {
        currentColor = color;

        switch (CurrentCustomizePart)
        {
            case CustomizePart.Skin:
                PhotonPlayerSettings.Instance.SkinColor = currentColor;
                break;
            case CustomizePart.Dress:
                PhotonPlayerSettings.Instance.DressColor = currentColor;
                break;
            case CustomizePart.Hair:
                PhotonPlayerSettings.Instance.HairColor = currentColor;
                break;
            case CustomizePart.Iris:
                PhotonPlayerSettings.Instance.IrisColor = currentColor;
                break;
            case CustomizePart.Lips:
                PhotonPlayerSettings.Instance.LipsColor = currentColor;
                break;
            case CustomizePart.Other:
                //PhotonPlayerSettings.Instance.SkinColor = currentColor;
                break;

        }

        if (CurrentCustomizePart == CustomizePart.Iris)
        {
            CurrentMaterial.SetColor("_MaskColorIris", color);
        }
        else if(CurrentCustomizePart == CustomizePart.Lips)
        {
            CurrentMaterial.SetColor("_MaskColorLips", color);
        }
        else
        {
            CurrentMaterial.SetColor("_BaseColor", color);
        }

        
    }

    public void SetAvatarGameObject(PlayerAvatar avatarInfo, OvrAvatar avatar)
    {
        AvatarInfo = avatarInfo;
        CurrentAvatar = avatar;
        CurrentAvatar.OnAvatarLoaded += OnAvatarLoaded;
    }

    private void OnAvatarLoaded()
    {
        StartCoroutine(WaitUntilAvatarLoaded());
    }

    private IEnumerator WaitUntilAvatarLoaded()
    {
        yield return new WaitUntil(() => CurrentAvatar.transform.childCount > 0);
        
        colorPicker.gameObject.SetActive(true);
        SettingField.SetActive(true);
        ChangeTexturesForAvatar();
        AvatarBody = CurrentAvatar.transform.GetComponentInChildren<OvrAvatarBody>();
        CurrentAvatar.OnAvatarLoaded -= OnAvatarLoaded;
        CurrentMaterial = AvatarBody.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        DressMaterial = AvatarBody.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
    }

    private void ChangeTexturesForAvatar()
    {
        switch(AvatarInfo.AvatarType)
        {
            case PlayerAvatarType.Man1:
                for (int i = 0; i < ListToChangeTshirt.Length; i++)
                {
                    ListToChangeTshirt[i].SetUiTextures(TexsMan1[i]);
                }
                break;
            case PlayerAvatarType.Woman1:
                for (int i = 0; i < ListToChangeTshirt.Length; i++)
                {
                    ListToChangeTshirt[i].SetUiTextures(TexsWoman1[i]);
                }
                break;
        }
    }

    public void SaveMaterialTexture(Texture texture3D)
    {
        Texture2D tex = texture3D as Texture2D;
        byte[] data;
        if (tex == null)
        {
            Texture source = texture3D;
            if (source == null)
                return;
            tex = new Texture2D(source.width, source.height, TextureFormat.ARGB32, false);
            Graphics.CopyTexture(source, tex);
            data = tex.EncodeToPNG();
            DestroyImmediate(tex);
        }
        else
        {
            data = tex.EncodeToPNG();
        }
        File.WriteAllBytes(Application.dataPath + "/../Assets/Materials/Saved/SavedScreen" + System.DateTime.Now.ToString("yyyymmdd") + ".png", data);
    }

    public void SaveMaterialTexture(Material mat, string filePrefix)
    {
        Texture2D savedTexture = mat.mainTexture as Texture2D;
        print($"TEXTURE FORMAT IS {savedTexture.format}");
        Texture2D newTexture = new Texture2D(savedTexture.width, savedTexture.height, TextureFormat.ARGB32, false);
        

        newTexture.SetPixels(0, 0, savedTexture.width, savedTexture.height, savedTexture.GetPixels());
        //newTexture = FillInClear(newTexture);
        //newTexture.Compress(true);
        newTexture.Apply();
        byte[] bytes = newTexture.EncodeToPNG();
        File.WriteAllBytes(@"C:\Users\TopPC\Desktop\man_t_shirts\" + filePrefix + UnityEngine.Random.Range(1, 10000) + ".png", bytes);
    }

}
