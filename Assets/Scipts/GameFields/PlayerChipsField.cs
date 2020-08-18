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

       

        //if (list.Count == 0)
        //{
        //    maxObjectsOnField += 1;

        //    for (var i = 0; i < Stacks.Length; i++)
        //        if (Stacks[i].Objects.Count != 0 && Stacks[i].Objects[0].GetComponent<ChipData>().Cost == chip.Cost && maxObjectsOnField != Stacks[i].Objects.Count)
        //            list.Add(Stacks[i]);

        //    if (list.Count == 0)
        //        for (var i = 0; i < Stacks.Length; i++)
        //            if (Stacks[i].Objects.Count == 0)
        //                list.Add(Stacks[i]);
        //}

        return list;

    }


   


}
