using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNickNameResolver : MonoBehaviour
{
    [SerializeField] private TMP_Text _textNickName;

    private void Update()
    {
        SetTextNickname();
    }

    private void SetTextNickname()
    {
        if (PhotonNetwork.IsConnected)
        {
            _textNickName.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            _textNickName.text = "No connection...";
        }
    }
}
