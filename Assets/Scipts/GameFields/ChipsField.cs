using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public enum ChipFieldEvents{ StackAnimationEnded, StackAnimationStarted }
public class ChipsField : AbstractField, IListener<ChipFieldEvents>
{
    public EventManager<ChipFieldEvents> FieldEventManager = new EventManager<ChipFieldEvents>();

    protected void Start()
    {
        FieldEventManager.AddListener(ChipFieldEvents.StackAnimationEnded, this);
        FieldEventManager.AddListener(ChipFieldEvents.StackAnimationStarted, this);
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

        data.stack.animator.UpdateStackInstantly();
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

        if (chip != null && gc != null && !gc.isGrabbed && !rb.isKinematic && view != null)
        {

            Debug.Log("MagnetizeObject viewID=" + view.ViewID);
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
            ExtranctChip(view.ViewID);
            //SyncStacks();
        }

    }

    #endregion
   

    void BlockAllStacks()
    {       
        photonView.RPC("UpdateAllStacks", RpcTarget.All, false, false);

        foreach (var stack in Stacks)
            foreach (var chip in stack.Objects)
                chip.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();
    }

    void UnblockAllStacks()
    {
        photonView.RPC("UpdateAllStacks", RpcTarget.All, true, true);

        for (var i = 0; i < Stacks.Length; i++)
        {
            for (var j = 0; j < Stacks[i].Objects.Count; j++)
            {
                var position = Stacks[i].Objects[j].transform.position;
                var viewID = Stacks[i].Objects[j].GetComponent<PhotonView>().ViewID;

                photonView.RPC("SyncGameObjects", RpcTarget.Others, viewID, position, i, j);
            }
        }

       

        foreach (var stack in Stacks)
            foreach (var chip in stack.Objects)
                chip.GetComponent<PhotonSyncCrontroller>().SyncOn_Photon();


       

    }   

    [PunRPC]
    public void UpdateAllStacks(bool col, bool updateInstantly)
    {
        foreach (var stack in Stacks)
        {
            if(updateInstantly)
             stack.UpdateStackInstantly();
            stack.animator.ChangeStateOfItem(col);
        }
    }

    [PunRPC]
    public void SyncGameObjects(int viewID, Vector3 position, int stackIndex, int chipsIndex)
    {
        Stacks[stackIndex].Objects[chipsIndex].GetComponent<PhotonView>().ViewID = viewID;
        Stacks[stackIndex].Objects[chipsIndex].transform.position = position;       
    }

    int StackAnimEndedCounter = 0;
    int StackAnimStartedCounter = 0;
    public void OnEvent(ChipFieldEvents Event_type, Component Sender, params object[] Param)
    {
        if (photonView.IsMine)
        {
            switch (Event_type)
            {
                case ChipFieldEvents.StackAnimationEnded:

                    StackAnimEndedCounter++;
                    Debug.Log("StackAnimStartedCounter =" + StackAnimEndedCounter);
                    if (StackAnimStartedCounter == StackAnimEndedCounter)
                    {
                        Debug.Log("UnblockAllStacks");
                        UnblockAllStacks();
                        StackAnimEndedCounter = 0;
                        StackAnimStartedCounter = 0;
                    }

                    break;
                case ChipFieldEvents.StackAnimationStarted:
                    StackAnimStartedCounter++;
                    Debug.Log("StackAnimStartedCounter =" + StackAnimStartedCounter);
                    if (StackAnimStartedCounter == 1)
                    {
                        BlockAllStacks();
                        Debug.Log("BlockAllStacks");
                    }
                    break;
            }
        }
    }
}


