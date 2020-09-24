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
    public Transform SpawnPos;

    protected override List<StackData> FindPossibleFields(OwnerData data)
    {
        var chip = (ChipData)data;
        var list = new List<StackData>();

        list = FindStackByType(ChipUtils.Instance.GetStringOfType(chip.Cost));
        

        return list;

    }


   


}
