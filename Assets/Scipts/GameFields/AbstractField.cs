using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using Assets.Scipts.Chips;

public enum AbstractFieldEvents { StackAnimationEnded, StackAnimationStarted, FieldAnimationEnded, FieldAnimationStarted, UpdateUI, ExtractObject, FieldBloked, FieldUnbloked, ObjectInside, ObjectOutdore }

public abstract class AbstractField : MonoBehaviourPun, IMagnetize, IListener<AbstractFieldEvents>
{
    protected int StackAnimEndedCounter = 0;
    protected int StackAnimStartedCounter = 0;

    public bool TriggerLocal = false;

    [SerializeField]
    public List<StackData> Stacks;
   
    [SerializeField]
    protected int maxObjectsOnField = 20;
    protected EventManager<AbstractFieldEvents> _fieldEventManager;
    public EventManager<AbstractFieldEvents> FieldEventManager
    {
        get
        {
            if (_fieldEventManager == null)
                _fieldEventManager = new EventManager<AbstractFieldEvents>();
            return _fieldEventManager;
        }
    }

    protected virtual void ClearObjectDataFromField(OwnerData data) { }
    protected List<StackData> FindStackByType(string type, List<StackData> Stacks)
    {
        var stacks = Stacks.FindAll(s => s.stackType == type && s.Objects.Count < maxObjectsOnField);
       
        if (stacks.Count == 0)
            return Stacks.FindAll(s => s.stackType == "");
        else return stacks;

    }
    protected virtual List<StackData> FindPossibleFields(OwnerData data)
    {     
        var list = new List<StackData>();
     
        for (var i = 0; i < Stacks.Count; i++)
            if (Stacks[i].Objects.Count < maxObjectsOnField)
                list.Add(Stacks[i]);
        return list;

    }

    public void BlockField(bool blockUnblock)
    {
        BlockItems(blockUnblock);
        var collider = GetComponent<Collider>();
        if (collider)
            collider.enabled = !blockUnblock;

        if(blockUnblock)
            _fieldEventManager.PostNotification(AbstractFieldEvents.FieldBloked, this);

        else
            _fieldEventManager.PostNotification(AbstractFieldEvents.FieldUnbloked, this);
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
        Stacks = GetComponentsInChildren<StackData>().ToList();
        FieldEventManager.AddListener(AbstractFieldEvents.StackAnimationEnded, this);
        FieldEventManager.AddListener(AbstractFieldEvents.StackAnimationStarted, this);
    }

    public void ClearStacks()
    {
        for (var i = 0; i < Stacks.Count; i++)                               
            Stacks[i].ClearData();
        FieldEventManager.PostNotification(AbstractFieldEvents.UpdateUI, this, 0, 0);

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

    protected void GetChipAndHisStack(int viewID, out GameObject chip, out StackData stack)
    {
        chip = null;
        stack = Stacks.ToList().Find(s => s.Objects.Find(c => c.GetComponent<PhotonView>().ViewID == viewID));
        if (stack == null)
            return;
        chip = stack.Objects.Find(s => s.GetComponent<PhotonView>().ViewID == viewID);        


    }
   
    public virtual void OnEvent(AbstractFieldEvents Event_type, Component Sender, params object[] Param)
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
                        _fieldEventManager.PostNotification(AbstractFieldEvents.FieldAnimationEnded, this);
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
                        _fieldEventManager.PostNotification(AbstractFieldEvents.FieldAnimationStarted, this);
                        BlockAllStacks();
                        Debug.Log("BlockAllStacks");
                    }
                    break;
            }
        }
    }

    protected void BlockAllStacks()
    {       
        photonView.RPC("UpdateAllStacks", RpcTarget.All, false, false);

        foreach (var stack in Stacks)
            foreach (var chip in stack.Objects)
                chip.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();
    }

    protected void UnblockAllStacks()
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

    protected virtual void OnTriggerEnter(Collider other)
    {

        //var gameObj = other.gameObject;
        //var chip = other.gameObject.GetComponent<OwnerData>();
        //var gc = other.gameObject.GetComponent<OVRGrabbableCustom>();
        //var rb = other.GetComponent<Rigidbody>();
        //var view = gameObj.GetComponent<PhotonView>();

        //if (chip != null && gc != null && !gc.isGrabbed && !rb.isKinematic && view != null)
        //{

        //    Debug.Log("MagnetizeObject viewID=" + view.ViewID);
        //    var clossest = FindClossestField(chip.transform, FindPossibleFields(chip));
        //    MagnetizeObject(gameObj, clossest, );

        //}

    }

    protected virtual void OnTriggerStay(Collider other)
    {

        //var gameObj = other.gameObject;
        //var chip = other.gameObject.GetComponent<OwnerData>();
        //var gc = other.gameObject.GetComponent<OVRGrabbableCustom>();
        //var rb = other.GetComponent<Rigidbody>();
        //var view = gameObj.GetComponent<PhotonView>();

        //if (chip != null && gc != null && gc.isGrabbed && rb.isKinematic && view != null && Contain(gameObj))
        //{
        //    //ExtranctObject(view.ViewID);
        //    //SyncStacks();
        //}

    }
    [PunRPC]
    public void ExtranctChipOnAll_RPC(int viewID)
    {
        StackData stack;
        GameObject chip;

        GetChipAndHisStack(viewID, out chip, out stack);

        if (chip == null)
        {
            Debug.Log("chip not found! viewID = " + viewID);
            return;
        }

        stack.Objects.Remove(chip);

        stack.UpdateStackInstantly();
    }
    public virtual GameObject ExtranctObject(int viewID)
    {
        StackData stack;
        GameObject chip;

        GetChipAndHisStack(viewID, out chip, out stack);

        if (chip == null)
        {
            Debug.Log("chip not found! viewID = " + viewID);
            return null;
        }

        stack.ExtractOne(chip);
        ClearObjectDataFromField(chip.GetComponent<OwnerData>());

        return chip;
    }

    public void ExtractAllObjects()
    {
        foreach (var stack in Stacks)
            stack.ExtractAll();

    }


    [PunRPC]
    public void MagnetizeObject_RPC(int viewId,int stackIndex, Vector3 ObjPosition)
    {
        try
        {
           

            var magnetizedObject = Physics.OverlapSphere(ObjPosition, 10f).FirstOrDefault(g => g.GetComponent<PhotonView>()?.ViewID == viewId).gameObject;
            var rb = magnetizedObject.GetComponent<Rigidbody>();
            var stackData = Stacks[stackIndex];

            var ownerdata = GetComponent<OwnerData>();
            if (stackData.playerName == "")
                stackData.playerName = ownerdata.Owner;

            ownerdata.field = this;

            rb.isKinematic = true;
            magnetizedObject.transform.parent = stackData.transform;

            stackData.Objects.Add(magnetizedObject);
            stackData.animator.StartAnim(magnetizedObject);
        }
        catch (Exception e)
        {
            Debug.LogError("MagnetizeObject_RPC exaption");
        }
    }
    public virtual bool MagnetizeObject(GameObject Object, StackData Stack, string StackType = "", bool magnetizeLocal = false)
    {
        if (!Stack)
            return false;

        var rb = Object.GetComponent<Rigidbody>();
        var chip = Object.GetComponent<OwnerData>();
        chip.field = this;

        if (Stack.stackType == "")
            Stack.stackType = StackType;

        var stackData = Stack;

        if (stackData.playerName.Equals(chip.Owner) || stackData.playerName == "")
        {
            if (stackData.playerName == "")
                stackData.playerName = chip.Owner;

            if(!magnetizeLocal)
            photonView.RPC("MagnetizeObject_RPC", RpcTarget.OthersBuffered, chip.photonView.ViewID, Stacks.ToList().IndexOf(Stack), chip.transform.position);

            rb.isKinematic = true;
            chip.transform.parent = stackData.transform;

            //Debug.Break();


            stackData.Objects.Add(Object);
            stackData.animator.StartAnim(Object);

            return true;


        }

        return false;
    }

}


