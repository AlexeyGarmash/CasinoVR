using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private TMP_Text TextPlayer;

    public Player PlayerInfo { get; set; }

    public void SetPlayerInfo(Player player)
    {
        PlayerInfo = player;
        TextPlayer.text = PlayerInfo.NickName;
    }
}
