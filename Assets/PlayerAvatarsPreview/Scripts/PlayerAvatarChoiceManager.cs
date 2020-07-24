using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarChoiceManager : MonoBehaviour
{
    [SerializeField] private PlayerAvatar[] _playerAvatars;
    [SerializeField] private Transform _avatarPreviewSpawnPoint;
    
    private void Start()
    {
        foreach (var avatar in _playerAvatars)
        {
            avatar.OnAvatarChosen += OnAvatarChosen;
        }
    }

    private void OnAvatarChosen(PlayerAvatar avatar)
    {
        ChooseUiAvatar(avatar);
        ClearNotSelectedSelection(avatar);
        SpawnPreviewAvatar(avatar);
        SetPlayerAvatarResourcePath(avatar);
    }

    private void SetPlayerAvatarResourcePath(PlayerAvatar avatar)
    {
        if(PhotonPlayerSettings.Instance != null)
        {
            PhotonPlayerSettings.Instance.PrefabResourceName = avatar.AvatarResoucePathName;
        }
    }

    private void ChooseUiAvatar(PlayerAvatar avatar)
    {
        avatar.ChooseAvatar();
    }

    private void ClearNotSelectedSelection(PlayerAvatar avatar)
    {
        foreach (var _avatar in _playerAvatars)
        {
            if (avatar != _avatar)
            {
                _avatar.UncheckSelected();
            }
        }
    }

    private void SpawnPreviewAvatar(PlayerAvatar avatar)
    {
        ClearSpawnChildren();
        SpawnPreview(avatar);
    }

    private void SpawnPreview(PlayerAvatar avatar)
    {
        if (_avatarPreviewSpawnPoint != null)
        {
            GameObject go = Instantiate(avatar.AvatarPrefab, _avatarPreviewSpawnPoint, true);
            go.transform.localPosition = Vector3.zero;
        }
    }

    private void ClearSpawnChildren()
    {
        if(_avatarPreviewSpawnPoint != null)
        {
            for (int i = 0; i < _avatarPreviewSpawnPoint.childCount; i++)
            {
                Destroy(_avatarPreviewSpawnPoint.GetChild(i).gameObject);
            }
        }
    }
}
