using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ChipsField : AbstractField
{

    public override bool MagnetizeObject(GameObject Object, StackData Stack)
    {
        var photonView = Object.GetComponent<PhotonView>();
        //photonView.ObservedComponents.Clear();
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

            //Debug.Break();

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

        if (list.Count == 0)
            for (var i = 0; i < Stacks.Length; i++)
                if (Stacks[i].Objects.Count == 0)
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

    private (GameObject chip, StackData stack) GetChipAndHisStack(int viewID)
    {
        var stack = Stacks.ToList().Find(s => s.Objects.Find(c => c.GetComponent<PhotonView>().ViewID == viewID));
        var chip = stack.Objects.Find(s => s.GetComponent<PhotonView>().ViewID == viewID);

        return (chip, stack);

    }
    #region RPC 
    public void ExtranctChipOnAll(int viewID)
    {
        var data = GetChipAndHisStack(viewID);
        if (data.chip == null)
            Debug.Log("chip not found! viewID = "  + viewID);

        data.chip.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Unreliable;
        data.stack.Objects.Remove(data.chip);

        data.stack.UpdateStackInstantly();
    }

    //protected bool ExtranctChipOnAll(int viewID)
    //{     
        
    //    photonView.RPC("ExtranctChipOnAll_RPC", RpcTarget.All, viewID);

    //    return true;
    //}
    #endregion


    #region Unity Callbacks
    protected void OnTriggerEnter(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        var gc = other.gameObject.GetComponent<GrabbableChip>();
        var rb = other.GetComponent<Rigidbody>();
        var view = gameObj.GetComponent<PhotonView>();

        if (chip != null && gc != null && !gc.isGrabbed  && !rb.isKinematic && view != null)
        {
            chip.GetComponent<NetworkInfo>().Synchronization = ViewSynchronization.Off;

            var clossest = FindClossestField(chip.transform, FindPossibleFields(chip));
            MagnetizeObject(gameObj, clossest);

        }

    }

    protected void OnTriggerStay(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        var gc = other.gameObject.GetComponent<GrabbableChip>();
        var rb = other.GetComponent<Rigidbody>();
        var view = gameObj.GetComponent<PhotonView>();
    
        if (chip != null && gc != null && gc.isGrabbed && rb.isKinematic && view != null && Contain(gameObj))
        {

            ExtranctChipOnAll(view.ViewID);

        }

    }

    #endregion



}


