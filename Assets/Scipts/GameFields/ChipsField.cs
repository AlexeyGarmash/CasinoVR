﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChipsField : AbstractField
{
   
    public override bool MagnetizeObject(GameObject Object, StackData Stack)
    {
        
        var rb = Object.GetComponent<Rigidbody>();
        var chip = Object.GetComponent<ChipData>();
       

        var stackData = Stack;
        var transform = stackData.gameObject.transform;
        if (stackData.playerName.Equals(chip.player) || stackData.playerName == "")
        {
            if (stackData.playerName == "")
                stackData.playerName = chip.player;

            rb.isKinematic = true;
            chip.transform.parent = stackData.transform;

            stackData.Objects.Add(Object);
            stackData.StartAnim(Object);

            return true;
            
        }

        return false;
    }
    private StackData FindClossestField(Transform chip, List<StackData> PossibleField)
    {
        List<float> distances = new List<float>();
        for (var i = 0; i < PossibleField.Count; i++)
        {
            distances.Add(Vector3.Distance(PossibleField[i].transform.position, chip.position));
        }

        return PossibleField[distances.IndexOf(distances.Min())];
    }

    private List<StackData> FindPossibleFields(ChipData data)
    {
        var list = new List<StackData>();

        for (var i = 0; i < Stacks.Length; i++)
            if (Stacks[i].Objects.Count != 0 && Stacks[i].Objects[0].GetComponent<ChipData>().Cost == data.Cost && maxChipsOnField != Stacks[i].Objects.Count)
                list.Add(Stacks[i]);

        if(list.Count == 0)
            for (var i = 0; i < Stacks.Length; i++)
                if(Stacks[i].Objects.Count == 0)
                    list.Add(Stacks[i]);

        if (list.Count == 0)
        {
            maxChipsOnField += 1;

            for (var i = 0; i < Stacks.Length; i++)
                if (Stacks[i].Objects.Count != 0 && Stacks[i].Objects[0].GetComponent<ChipData>().Cost == data.Cost && maxChipsOnField != Stacks[i].Objects.Count)
                    list.Add(Stacks[i]);

            if (list.Count == 0)
                for (var i = 0; i < Stacks.Length; i++)
                    if (Stacks[i].Objects.Count == 0)
                        list.Add(Stacks[i]);
        }

        return list;

    }

    private StackData FindStackOfChip(ChipData data)
    {
        for (var i = 0; i < Stacks.Length; i++)
            if (Stacks[i].Objects.Contains(data.gameObject))
                return Stacks[i];

        return null;
    }

    private void RemoveChip(ChipData data)
    {
        var stack = FindStackOfChip(data);
        if (stack != null)
            ExtractionObject(data.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        var gc = other.gameObject.GetComponent<GrabbableChip>();
        var rb = other.GetComponent<Rigidbody>();
        if (chip != null && gc != null && gc.grabbedBy == null && !rb.isKinematic)
        {

            var clossest = FindClossestField(chip.transform, FindPossibleFields(chip));
            MagnetizeObject(gameObj, clossest);

        }

    }

    private void OnTriggerStay(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        var gc = other.gameObject.GetComponent<GrabbableChip>();
        var rb = other.GetComponent<Rigidbody>();

        if (chip != null && gc != null && gc.grabbedBy != null && rb.isKinematic)
        {

            RemoveChip(chip);


        }

    }



}


