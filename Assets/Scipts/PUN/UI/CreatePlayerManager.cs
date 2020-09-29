using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using TMPro;
using Photon.Pun;
using UnityEngine.Events;
using System;
using TalesFromTheRift;

public class CreatePlayerManager : BaseMenuPanel
{
    const string PREF_NICKNAME_KEY = "nickname";

    [SerializeField] private TMP_InputField InputFieldNickName;

    private void Start()
    {
        GetNameFromPrefs();
    }

    public void ChangeNickName()
    {
        if(InputFieldNickName.text.Length != 0 )
        {
            if(PhotonPlayerSettings.Instance != null
            && PhotonPlayerSettings.Instance.PrefabResourceName != null)
            {
                SaveNickNameToPrefs();
                NetworkManager.Instance.ChangePlayerNick(InputFieldNickName.text);
                //InputFieldNickName.text = "";
                MainMenuManager.Instance.OnPlayerEnterNickName();
            }
            else
            {
                print("Choose avatar");
                MainMenuInformer.Instance.ShowInfoWithExitTime("Choose avatar", MainMenuMessageType.Warning);
            }
            
        }
        else
        {
            print("Nick must be not empty!");
            MainMenuInformer.Instance.ShowInfoWithExitTime("Empty nickname", MainMenuMessageType.Warning);
        }
    }

    private void GetNameFromPrefs()
    {
        string storedNickname = PlayerPrefs.GetString(PREF_NICKNAME_KEY, "");
        InputFieldNickName.text = storedNickname;
    }

    private void SaveNickNameToPrefs()
    {
        PlayerPrefs.SetString(PREF_NICKNAME_KEY, InputFieldNickName.text);
    }

}
