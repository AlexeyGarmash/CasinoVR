using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using TMPro;
using Photon.Pun;

public class CreatePlayerManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField InputFieldNickName;

    public void ChangeNickName()
    {
        if(InputFieldNickName.text.Length != 0)
        {
            NetworkManager.Instance.ChangePlayerNick(InputFieldNickName.text);
            InputFieldNickName.text = "";
            MainMenuManager.Instance.OnPlayerEnterNickName();
        }
        else
        {
            print("Nick must be not empty!");
        }
    }

}
