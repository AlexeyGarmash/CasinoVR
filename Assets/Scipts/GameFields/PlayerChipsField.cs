using OVR.OpenVR;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerChipsField : ChipsField
{
  
    [SerializeField]
    private Transform SpawnPos;




    public void InstantiateToStackWithColor(Chips chipsCost, ref int money, string playerNick)
    {
        var chip = PhotonNetwork.Instantiate(ChipUtils.Instance.GetPathToChip(chipsCost), SpawnPos.position, SpawnPos.rotation);
        chip.GetComponent<ChipData>().Owner = playerNick;
        chip.GetComponent<PhotonView>().RequestOwnership();
        chip.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();
        money -= (int)chipsCost;
    }


}
