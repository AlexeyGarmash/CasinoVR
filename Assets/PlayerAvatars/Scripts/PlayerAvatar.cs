using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatar : MonoBehaviour
{
    public Action<PlayerAvatar> OnAvatarChosen;

    [Header("Prefab info")]
    [SerializeField] private string _avatarResoucePathName;
    [SerializeField] private GameObject _avatarPrefab;
    [SerializeField] private Texture _avatarUiTexture;
    [SerializeField] private string _avatarDisplayName;

    [Header("UI elements")]
    [SerializeField] private RawImage _avatarUiImage;
    [SerializeField] private TMP_Text _avatarNameText;
    [SerializeField] private Button _buttonChoiceAvatar;
    [SerializeField] private RawImage _frameChosenAvatar;


    [SerializeField] private bool _avatarIsChosen = false;

    public GameObject AvatarPrefab { get => _avatarPrefab; }
    public string AvatarResoucePathName { get => _avatarResoucePathName; }
    public string AvatarDisplayName { get => _avatarDisplayName; }

    private void Start()
    {
        SetAvatarUi();
        SetAvatarDisplayName();
        if(_avatarIsChosen)
        {
            ChooseAvatar();
        }
        else
        {
            UncheckSelected();
        }
    }

    private void Awake()
    {
        SetupListeners();
    }

    private void SetupListeners()
    {
        _buttonChoiceAvatar.onClick.AddListener(OnButtonChooseAvatar_Click);
    }

    private void OnButtonChooseAvatar_Click()
    {
        OnAvatarChosen.Invoke(this);
    }

    private void SetAvatarDisplayName()
    {
        if(_avatarDisplayName != null && _avatarDisplayName != string.Empty)
        {
            _avatarNameText.text = _avatarDisplayName;
        }
        else
        {
            _avatarNameText.text = "Default name";
        }
    }

    private void SetAvatarUi()
    {
        if(_avatarUiTexture != null)
        {
            _avatarUiImage.texture = _avatarUiTexture;
        }
    }

    public void ChooseAvatar()
    {
        _frameChosenAvatar.enabled = true;
        _avatarIsChosen = true;
    }

    public void UncheckSelected()
    {
        _frameChosenAvatar.enabled = false;
        _avatarIsChosen = false;
    }



}
