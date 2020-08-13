using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public enum AbstractFieldEvents{ StackAnimationEnded, StackAnimationStarted }
public class ChipsField : AbstractField
{
    protected List<StackData> FindPossibleFields(ChipData data)
    {
        var list = new List<StackData>();

        for (var i = 0; i < Stacks.Length; i++)
            if (Stacks[i].Objects.Count != 0 && Stacks[i].Objects[0].GetComponent<ChipData>().Cost == data.Cost && maxObjectsOnField != Stacks[i].Objects.Count)
                list.Add(Stacks[i]);

        if (list.Count == 0)
            for (var i = 0; i < Stacks.Length; i++)
                if (Stacks[i].Objects.Count == 0)
                    list.Add(Stacks[i]);

        if (list.Count == 0)
        {
            maxObjectsOnField += 1;

            for (var i = 0; i < Stacks.Length; i++)
                if (Stacks[i].Objects.Count != 0 && Stacks[i].Objects[0].GetComponent<ChipData>().Cost == data.Cost && maxObjectsOnField != Stacks[i].Objects.Count)
                    list.Add(Stacks[i]);

            if (list.Count == 0)
                for (var i = 0; i < Stacks.Length; i++)
                    if (Stacks[i].Objects.Count == 0)
                        list.Add(Stacks[i]);
        }

        return list;

    }
    public override bool MagnetizeObject(GameObject Object, StackData Stack)
    {
        var photonView = Object.GetComponent<PhotonView>();
        //photonView.ObservedComponents.Clear();
        var rb = Object.GetComponent<Rigidbody>();
        var chip = Object.GetComponent<ChipData>();


        var stackData = Stack;
        var transform = stackData.gameObject.transform;
        if (stackData.playerName.Equals(chip.Owner) || stackData.playerName == "")
        {
            if (stackData.playerName == "")
                stackData.playerName = chip.Owner;

            rb.isKinematic = true;
            chip.transform.parent = stackData.transform;

            //Debug.Break();

            stackData.Objects.Add(Object);
            stackData.animator.StartAnim(Object);
        

            return true;


        }

        return false;
    }


    protected new void OnTriggerEnter(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        var gc = other.gameObject.GetComponent<GrabbableChip>();
        var rb = other.GetComponent<Rigidbody>();
        var view = gameObj.GetComponent<PhotonView>();

        if (chip != null && gc != null && !gc.isGrabbed && !rb.isKinematic && view != null)
        {

            //Debug.Log("MagnetizeObject viewID=" + view.ViewID);
            var clossest = FindClossestField(chip.transform, FindPossibleFields(chip));
            MagnetizeObject(gameObj, clossest);

        }

    }
    protected new void OnTriggerStay(Collider other)
    {

        var gameObj = other.gameObject;
        ChipData chip = other.gameObject.GetComponent<ChipData>();
        var gc = other.gameObject.GetComponent<GrabbableChip>();
        var rb = other.GetComponent<Rigidbody>();
        var view = gameObj.GetComponent<PhotonView>();

        if (chip != null && gc != null && gc.isGrabbed && rb.isKinematic && view != null && Contain(gameObj))
        {
            ExtranctChip(view.ViewID);
            //SyncStacks();
        }

    }

    #region Unity Callbacks



    #endregion



}


