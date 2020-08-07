using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scipts.Player
{
    class PlayerData
    {
        public string playerNickname { get => PhotonNetwork.LocalPlayer.NickName;  }
        public int money;


        
    }
}
