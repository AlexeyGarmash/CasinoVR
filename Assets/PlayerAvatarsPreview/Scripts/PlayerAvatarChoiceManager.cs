using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarChoiceManager : MonoBehaviour
{
    [SerializeField] private PlayerAvatar[] _playerAvatars;
    [SerializeField] private Transform _avatarPreviewSpawnPoint;

    private ExitGames.Client.Photon.Hashtable customProps;

    private void Start()
    {
        customProps = new ExitGames.Client.Photon.Hashtable();
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
        SetPlayerCustomPropsAvatar(avatar);
        
    }

    private void SetPlayerCustomPropsAvatar(PlayerAvatar avatar)
    {
        customProps[PlayerItem.KEY_PLAYER_AVATAR] = avatar.AvatarResourceUI;
        PhotonNetwork.SetPlayerCustomProperties(customProps);
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
            Renderer rend = go.GetComponent<Renderer>();
            if(rend == null)
            {
                rend = go.GetComponentInChildren<Renderer>();
            }
            AvatarSkinChooser.Instance.SetupAvatartToChoose(rend);
            CustomizeAvatarManager.Instance.SetAvatarGameObject(avatar, go.GetComponent<OvrAvatar>());
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
