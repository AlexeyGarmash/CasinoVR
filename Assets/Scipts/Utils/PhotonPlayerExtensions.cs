using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhotonPlayerExtensions
{
    public static bool IsReady(this Player player)
    {
        if(player.CustomProperties.ContainsKey(PlayerItem.KEY_PLAYER_READY))
        {
            return (bool)player.CustomProperties[PlayerItem.KEY_PLAYER_READY];
        }
        else
        {
            return false;
        }
    }
}
