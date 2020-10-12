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
    private Transform redChipSpawnpoint;
    [SerializeField]
    private Transform greenChipSpawnpoint;
    [SerializeField]
    private Transform blueChipSpawnpoint;
    [SerializeField]
    private Transform blackChipSpawnpoint;
    [SerializeField]
    private Transform purpleChipSpawnpoint;

    public Transform npcCenter;

    public Dictionary<Chips, Transform> StacksByChipCost = new Dictionary<Chips, Transform>();

    [SerializeField]
    private Transform StackSpawnPoint;

    [SerializeField]
    private GameObject stackPrefab;

    [SerializeField]
    private float distanceToPossibleStack = 0.12f;

    GrabbableChip lastChip;

    List<ChipData> triggeredChips = new List<ChipData>();

    bool chekingChipsStarted = false;

    public Vector3 lastPlayerHandPosition;

    private void Start()
    {
        StacksByChipCost.Add(Chips.BLACK, blackChipSpawnpoint);
        StacksByChipCost.Add(Chips.PURPLE, purpleChipSpawnpoint);
        StacksByChipCost.Add(Chips.GREEN, greenChipSpawnpoint);
        StacksByChipCost.Add(Chips.RED, redChipSpawnpoint);
        StacksByChipCost.Add(Chips.BLUE, blueChipSpawnpoint);
    }
    protected override List<StackData> FindPossibleFields(OwnerData data)
    {

        var chip = data as ChipData;
        var list = new List<StackData>();



        foreach (var stack in base.FindPossibleFields(chip))
        {
            var vect1 = Vector3.ProjectOnPlane(stack.transform.position, Vector3.up);
            var vect2 = Vector3.ProjectOnPlane(chip.transform.position, Vector3.up);

            if (Vector3.Distance(vect1, vect2) < distanceToPossibleStack)
            {
                list.Add(stack);
            }
        }

        return list;

    }

    protected override void ClearObjectDataFromField(OwnerData data)
    {
        if (triggeredChips.Contains(data as ChipData))
            triggeredChips.Remove(data as ChipData);
    }


    [PunRPC]
    private void CreateStack_RPC(Vector3 position, int[] chipsViewId, int stackViewID)
    {
        var grabbableCollidrs = Physics.OverlapSphere(transform.position, 1f).ToList().FindAll(c => c.GetComponent<ChipData>());       

        grabbableCollidrs.ForEach(collider =>
        {
            if (chipsViewId.ToList().Exists(id => collider.GetComponent<ChipData>().photonView.ViewID == id))
            {
                triggeredChips.Add(collider.GetComponent<ChipData>());
            }

        });

        var newStack = Instantiate(stackPrefab, transform);

        newStack.transform.localPosition = position;
        newStack.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var stackData = newStack.GetComponent<StackData>();
    
        stackData.owner = this;
        stackData.playerName = triggeredChips[0].GetComponent<ChipData>().Owner;
        stackData.destoyableStack = true;
        stackData.photonView.ViewID = stackViewID;

        Stacks.Add(stackData);

        foreach (var chip in triggeredChips)
        {
            var possibleFiedls = FindPossibleFields(chip);
            if (possibleFiedls.Count == 0)
                MagnetizeObject(chip.gameObject, stackData, ChipUtils.Instance.GetStringOfType(chip.Cost));
            else MagnetizeObject(chip.gameObject, possibleFiedls[0], ChipUtils.Instance.GetStringOfType(chip.Cost));

        }

        if (stackData.Objects.Count == 0)
            Destroy(stackData.gameObject);

        chekingChipsStarted = false;
        lastChip = null;

        triggeredChips.Clear();

    } 
    IEnumerator ChipInField()
    {
        chekingChipsStarted = true;

        while (lastChip.isGrabbed)
        {
            yield return null;
        }

        if (lastChip.GetComponent<OwnerData>().field != this)
        {
            chekingChipsStarted = false;       
            StopAllCoroutines();
            yield return null;
        }

        lastChip.transform.parent = transform;
        var newStack = Instantiate(stackPrefab, transform);

        newStack.transform.localPosition = new Vector3(lastChip.transform.localPosition.x, lastChip.transform.localPosition.y, StackSpawnPoint.localPosition.z);
        newStack.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var view = newStack.GetComponent<PhotonView>();
        PhotonNetwork.AllocateViewID(view);

        var stackData = newStack.GetComponent<StackData>();
        stackData.destoyableStack = true;

        Debug.Log("Stack created");

        int[] chipIds = new int[triggeredChips.Count];

        for (var i = 0; i < triggeredChips.Count; i++)
            chipIds[i] = triggeredChips[i].photonView.ViewID;

        photonView.RPC("CreateStack_RPC", RpcTarget.Others, newStack.transform.localPosition, chipIds, view.ViewID);

        Stacks.Add(stackData);
        stackData.owner = this;
        stackData.playerName = lastChip.GetComponent<ChipData>().Owner;
        
        foreach (var chip in triggeredChips)
        {
            var possibleFiedls = FindPossibleFields(chip);
            if(possibleFiedls.Count == 0)
                MagnetizeObject(chip.gameObject, stackData, ChipUtils.Instance.GetStringOfType(chip.Cost));
            else MagnetizeObject(chip.gameObject, possibleFiedls[0], ChipUtils.Instance.GetStringOfType(chip.Cost));

        }


        if (stackData.Objects.Count == 0)
            Destroy(stackData.gameObject);

        chekingChipsStarted = false;
        lastChip = null;

        triggeredChips.Clear();


    }
    protected override void OnTriggerStay(Collider other)
    {
        if (photonView.IsMine)
        {
            var chipdata = other.GetComponent<ChipData>();

            if (chipdata)
            {

                var grabbableChip = other.GetComponent<GrabbableChip>();

                if (grabbableChip && !triggeredChips.Contains(chipdata))
                {
                    if (grabbableChip.isGrabbed)
                    {
                        Debug.Log("chip added");
                        lastChip = grabbableChip;
                        lastChip.GetComponent<ChipData>().field = this;
                        triggeredChips.Add(other.GetComponent<ChipData>());

                        if (!chekingChipsStarted)
                        {
                            StartCoroutine(ChipInField());
                        }
                    }
                    else if (!chipdata.animator && !chipdata.GetComponent<Rigidbody>().isKinematic)
                    {

                        var stacks = FindStackByType(ChipUtils.Instance.GetStringOfType(chipdata.Cost), Stacks);

                        if (stacks.Count == 0)
                            MagnetizeObject(chipdata.gameObject, Stacks[0]);

                        else MagnetizeObject(chipdata.gameObject, stacks[0]);
                    }
                }

            }
        }
       

    }

    protected override void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine)
        {
            //base.OnTriggerExit(other);
            var chipdata = other.GetComponent<ChipData>();

            if (chipdata && triggeredChips.Contains(chipdata))
            {
                var grabbableChip = other.GetComponent<GrabbableChip>();
                if (grabbableChip)
                {
                    if (grabbableChip.isGrabbed)
                    {
                        Debug.Log("chip removed");
                        chipdata.field = null;
                        triggeredChips.Remove(chipdata);

                        if (triggeredChips.Count == 0)
                        {
                            StopAllCoroutines();
                            chekingChipsStarted = false;
                        }
                    }
                }

            }
        }

    }






}
