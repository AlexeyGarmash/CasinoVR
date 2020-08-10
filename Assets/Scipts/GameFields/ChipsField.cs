using Photon.Pun;
using System;
using System.Collections;
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
        if (stackData.playerName.Equals(chip.Owner) || stackData.playerName == "")
        {
            if (stackData.playerName == "")
                stackData.playerName = chip.Owner;

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
        if(stack == null)
            return (null, null);
        var chip = stack.Objects.Find(s => s.GetComponent<PhotonView>().ViewID == viewID);

        return (chip, stack);
       

    }
    #region RPC 
    [PunRPC]
    public void ExtranctChipOnAll_RPC(int viewID)
    {
        var data = GetChipAndHisStack(viewID);
        if (data.chip == null)
        {
            Debug.Log("chip not found! viewID = " + viewID);
            return;
        }
       
        data.stack.Objects.Remove(data.chip);

        data.stack.UpdateStackInstantly();
    }
    public void ExtranctChip(int viewID)
    {
        var data = GetChipAndHisStack(viewID);
        if (data.chip == null)
        {
            Debug.Log("chip not found! viewID = " + viewID);
            return;
        }
     
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
        var networkProps = gameObj.GetComponent<ChipData>();
        if (chip != null && gc != null && !networkProps.isGrabbed && !rb.isKinematic && view != null)
        {
            
            Debug.Log("MagnetizeObject viewID=" + view.ViewID);          
            var clossest = FindClossestField(chip.transform, FindPossibleFields(chip));
            MagnetizeObject(gameObj, clossest);
            SyncStacks();
        }

    }

    protected void OnTriggerStay(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        var gc = other.gameObject.GetComponent<GrabbableChip>();
        var rb = other.GetComponent<Rigidbody>();
        var view = gameObj.GetComponent<PhotonView>();
        var networkProps = gameObj.GetComponent<ChipData>();

        if (chip != null && gc != null && networkProps.isGrabbed && rb.isKinematic && view != null && Contain(gameObj))
        {
            ExtranctChip(view.ViewID);
            //SyncStacks();
        }

    }

    #endregion
    bool syncStarted = false;

    public void SyncStacks()
    {
        if (!syncStarted)
            StartCoroutine(SynchronizeStacks());
    }
    IEnumerator SynchronizeStacks()
    {
        syncStarted = true;

        photonView.RPC("UpdateAllStacks", RpcTarget.All, false, true);

        foreach (var stack in Stacks)
            foreach (var chip in stack.Objects)
                chip.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();

        yield return new WaitForSeconds(2f);
        while (Stacks.ToList().Sum(s => s.animator.AnimationFlag) != Stacks.Length)
        {

            Debug.Log("Wait Anim to sync");
            yield return new WaitForSeconds(0.2f);

        }


        for (var i = 0; i < Stacks.Length; i++)
        {
            for (var j = 0; j < Stacks[i].Objects.Count; j++)
            {
                var position = Stacks[i].Objects[j].transform.position;
                var viewID = Stacks[i].Objects[j].GetComponent<PhotonView>().ViewID;

                photonView.RPC("SyncGameObjects", RpcTarget.All, viewID, position, i, j);
            }
        }


        foreach (var stack in Stacks)
            foreach (var chip in stack.Objects)
                chip.GetComponent<PhotonSyncCrontroller>().SyncOn_Photon();


        photonView.RPC("UpdateAllStacks", RpcTarget.All, true, false);

        syncStarted = false;

    }

    [PunRPC]
    public void UpdateAllStacks(bool col, bool isAnim)
    {
        foreach (var stack in Stacks)
        {
            //stack.UpdateStackInstantly();
            stack.animator.ChangeStateOfItem(col, isAnim);
        }
    }

    [PunRPC]
    public void SyncGameObjects(int viewID, Vector3 position, int stackIndex, int chipsIndex)
    {
        Stacks[stackIndex].Objects[chipsIndex].GetComponent<PhotonView>().ViewID = viewID;
        Stacks[stackIndex].Objects[chipsIndex].transform.position = position;       
        //chip.GetComponent<PhotonSyncCrontroller>().SyncOn_Photon();
    }


}


