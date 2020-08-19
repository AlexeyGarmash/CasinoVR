using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using Assets.Scipts.Chips;

public abstract class AbstractField : MonoBehaviourPun, IMagnetize, IListener<AbstractFieldEvents>
{ 
    [SerializeField]
    public StackData[] Stacks;
   
    [SerializeField]
    protected int maxObjectsOnField = 20;
    EventManager<AbstractFieldEvents> _fieldEventManager;
    public EventManager<AbstractFieldEvents> FieldEventManager
    {
        get
        {
            if (_fieldEventManager == null)
                _fieldEventManager = new EventManager<AbstractFieldEvents>();
            return _fieldEventManager;
        }
    }

    protected List<StackData> FindStackByType(string type)
    {
        var stacks = Stacks.ToList().FindAll(s => s.stackType == type && s.Objects.Count < maxObjectsOnField);
       
        if (stacks.Count == 0)
            return Stacks.ToList().FindAll(s => s.stackType == "");
        else return stacks;

    }
    protected virtual List<StackData> FindPossibleFields(OwnerData data)
    {     
        var list = new List<StackData>();
     
        for (var i = 0; i < Stacks.Length; i++)
            if (Stacks[i].Objects.Count < maxObjectsOnField)
                list.Add(Stacks[i]);
        return list;

    }

    public void BlockField(bool blockUnblock)
    {
        BlockItems(blockUnblock);
        GetComponent<Collider>().isTrigger = !blockUnblock;
    }
    public void BlockItems(bool blockUnblock)
    {
        foreach (var stack in Stacks)
            foreach (var chip in stack.Objects)
            {
                chip.GetComponent<Collider>().enabled = !blockUnblock;

            }
    }

    protected void Awake()
    {
        Stacks = GetComponentsInChildren<StackData>();
        FieldEventManager.AddListener(AbstractFieldEvents.StackAnimationEnded, this);
        FieldEventManager.AddListener(AbstractFieldEvents.StackAnimationStarted, this);
    }

    public void ClearStacks()
    {
        for (var i = 0; i < Stacks.Length; i++)                               
            Stacks[i].ClearData();

        
    }
    public void ClearStack(StackData stack)
    {
        
        foreach (Transform child in transform)
            foreach (Transform child1 in child)
                Destroy(child1.gameObject);
        stack.ClearData();

        
    }
    public bool Contain(GameObject chip)
    {
        foreach (StackData stack in Stacks)
        {
            if (stack.Objects.Contains(chip.gameObject))
                return true;
        }

        return false;
    }
    protected StackData FindStackByName(Transform chip)
    {
        var list = Stacks.ToList();
        StackData stack = null;
        if (list.Exists(s => s.playerName == chip.GetComponent<ChipData>().Owner))
            stack = Stacks.ToList().Find(s => s.playerName == chip.GetComponent<ChipData>().Owner);

        if (stack == null)
            return Stacks.ToList().Find(s => s.playerName == "");

        return stack;
    }
    protected StackData FindClossestField(Transform chip, List<StackData> PossibleField)
    {
        if (PossibleField == null)
            PossibleField = Stacks.ToList();
        List<float> distances = new List<float>();
        for (var i = 0; i < PossibleField.Count; i++)
        {
            distances.Add(Vector3.Distance(PossibleField[i].transform.position, chip.position));
        }

        return PossibleField[distances.IndexOf(distances.Min())];
    }

    

    protected (GameObject chip, StackData stack) GetChipAndHisStack(int viewID)
    {
        var stack = Stacks.ToList().Find(s => s.Objects.Find(c => c.GetComponent<PhotonView>().ViewID == viewID));
        if (stack == null)
            return (null, null);
        var chip = stack.Objects.Find(s => s.GetComponent<PhotonView>().ViewID == viewID);

        return (chip, stack);


    }
    int StackAnimEndedCounter = 0;
    int StackAnimStartedCounter = 0;
    public void OnEvent(AbstractFieldEvents Event_type, Component Sender, params object[] Param)
    {
        if (photonView.IsMine)
        {
            switch (Event_type)
            {
                case AbstractFieldEvents.StackAnimationEnded:

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
                case AbstractFieldEvents.StackAnimationStarted:
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
             
    }   

    [PunRPC]
    public void UpdateAllStacks(bool col, bool updateInstantly)
    {


        foreach (var stack in Stacks)
        {
            if(updateInstantly)
             stack.UpdateStackInstantly();
            BlockItems(!col);
        }
        
    }

    protected void OnTriggerEnter(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<OwnerData>();
        var gc = other.gameObject.GetComponent<OVRGrabbableCustom>();
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
        var chip = other.gameObject.GetComponent<OwnerData>();
        var gc = other.gameObject.GetComponent<OVRGrabbableCustom>();
        var rb = other.GetComponent<Rigidbody>();
        var view = gameObj.GetComponent<PhotonView>();

        if (chip != null && gc != null && gc.isGrabbed && rb.isKinematic && view != null && Contain(gameObj))
        {
            ExtranctObject(view.ViewID);
            //SyncStacks();
        }

    }
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
    public void ExtranctObject(int viewID)
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

    public void ExtractAllObjects()
    {
        foreach (var stack in Stacks)
            stack.ExtractAll();

    }

    public abstract bool MagnetizeObject(GameObject Object, StackData Stack);

}


