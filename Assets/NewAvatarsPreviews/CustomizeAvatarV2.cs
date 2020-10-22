using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeAvatarV2 : MonoBehaviour
{
    public static CustomizeAvatarV2 Instance;

    public List<CustomizeAvatarPartV2> BodyPartsToCustomize;
    [SerializeField] private CustomizePanelUI UIPanel_Male;
    [SerializeField] private CustomizePanelUI UIPanel_Female;
    [SerializeField] private GameObject _avatarPreview;
    
    [SerializeField] private PlayerAvatar _avatarInfo;
    [SerializeField] private AvatarBodyHolder _currentAvatarBody;
    [SerializeField] private CustomizePartsChooser _customizePartsChooser;
    

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void SetupAvataraPreviewGameObject(GameObject avatarPreview, PlayerAvatar avatarInfo)
    {
        _avatarPreview = avatarPreview;
        _avatarInfo = avatarInfo;
        _currentAvatarBody = _avatarPreview.GetComponent<AvatarBodyHolder>();

        PhotonPlayerSettings.Instance.ResetBodyParts();
        if (_avatarInfo.AvatarMale)
        {
            UIPanel_Male.ActivateOnlyUI();
            UIPanel_Female.DeactivateAll();
            
        }
        else
        {
            UIPanel_Male.DeactivateAll();
            UIPanel_Female.ActivateOnlyUI();
        }
    }

    public void SetPartsToCustomize(CustomizePartsChooser customizeParts)
    {
        foreach (var part in BodyPartsToCustomize)
        {
            part.OnBodyPartChanged -= OnBodyPartChanged;
        }

        _customizePartsChooser = customizeParts;
        BodyPartsToCustomize = _customizePartsChooser.CustomParts;

        foreach (var part in BodyPartsToCustomize)
        {
            part.OnBodyPartChanged += OnBodyPartChanged;
        }

    }

    private void OnBodyPartChanged(CustomizeAvatarPartV2 avatarPart)
    {
        PhotonPlayerSettings.Instance.SetupBodyPart(
            _avatarInfo.AvatarMale,
            avatarPart.BodyPartType,
            avatarPart.MaterialData,
            avatarPart.MeshData);

        switch (avatarPart.BodyPartType)
        {
            case CustomizeAvatarPartV2.CA_Part.Beard:
                ChangeBeard(avatarPart);
                break;
            case CustomizeAvatarPartV2.CA_Part.Hair:
                ChangeHair(avatarPart);
                break;
            case CustomizeAvatarPartV2.CA_Part.Shirt:
                ChangeShirt(avatarPart);
                break;
            case CustomizeAvatarPartV2.CA_Part.Glasses:
                ChangeGlasses(avatarPart);
                break;
        }
    }

    private void ChangeHair(CustomizeAvatarPartV2 avatarPart)
    {
        _currentAvatarBody.ChangeHair(avatarPart.MaterialData, avatarPart.MeshData);
        
    }

    private void ChangeBeard(CustomizeAvatarPartV2 avatarPart)
    {
        _currentAvatarBody.ChangeBeard(avatarPart.MaterialData, avatarPart.MeshData);
    }

    private void ChangeShirt(CustomizeAvatarPartV2 avatarPart)
    {
        _currentAvatarBody.ChangeShirt(avatarPart.MaterialData, avatarPart.MeshData);
    }

    private void ChangeGlasses(CustomizeAvatarPartV2 avatarPart)
    {
        _currentAvatarBody.ChangeGlasses(avatarPart.MaterialData, avatarPart.MeshData);
    }
}
