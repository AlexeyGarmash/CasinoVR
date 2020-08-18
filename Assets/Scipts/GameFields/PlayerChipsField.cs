using Assets.Scipts.Chips;
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

    protected override List<StackData> FindPossibleFields(OwnerData data)
    {
        var chip = (ChipData)data;
        var list = new List<StackData>();

        for (var i = 0; i < Stacks.Length; i++)
            if (Stacks[i].Objects.Count != 0 && Stacks[i].Objects[0].GetComponent<ChipData>().Cost == chip.Cost && maxObjectsOnField != Stacks[i].Objects.Count)
                list.Add(Stacks[i]);

        if (list.Count == 0)
            for (var i = 0; i < Stacks.Length; i++)
                if (Stacks[i].Objects.Count == 0)
                    list.Add(Stacks[i]);

        if (list.Count == 0)
        {
            maxObjectsOnField += 1;

            for (var i = 0; i < Stacks.Length; i++)
                if (Stacks[i].Objects.Count != 0 && Stacks[i].Objects[0].GetComponent<ChipData>().Cost == chip.Cost && maxObjectsOnField != Stacks[i].Objects.Count)
                    list.Add(Stacks[i]);

            if (list.Count == 0)
                for (var i = 0; i < Stacks.Length; i++)
                    if (Stacks[i].Objects.Count == 0)
                        list.Add(Stacks[i]);
        }

        return list;

    }


    public void InstantiateToStackWithColor(Chips chipsCost, ref int money, string playerNick)
    {
        var chip = PhotonNetwork.Instantiate(ChipUtils.Instance.GetPathToChip(chipsCost), SpawnPos.position, SpawnPos.rotation);
        chip.GetComponent<OwnerData>().SetOwner_Photon(playerNick);
        chip.GetComponent<PhotonView>().RequestOwnership();
        chip.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();
        money -= (int)chipsCost;
    }


}
