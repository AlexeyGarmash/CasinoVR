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
        if(triggeredChips.Contains(data as ChipData))
            triggeredChips.Remove(data as ChipData);
    }
    IEnumerator ChipInField()
    {
        chekingChipsStarted = true;

        while (lastChip.isGrabbed)
        {
            yield return null;
        }

        if (lastChip.GetComponent<OwnerData>().field)
        {
            StopAllCoroutines();
            yield return null;
        }

        lastChip.transform.parent = transform;
        var newStack = Instantiate(stackPrefab, transform);

        newStack.transform.localPosition = new Vector3(lastChip.transform.localPosition.x, lastChip.transform.localPosition.y, StackSpawnPoint.localPosition.z);
        newStack.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var stackData = newStack.GetComponent<StackData>();
        stackData.destoyableStack = true;

        Debug.Log("Stack created");

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
    protected new void OnTriggerStay(Collider other)
    {
        var chipdata = other.GetComponent<ChipData>();
        
        if (chipdata && !triggeredChips.Contains(chipdata) && !chipdata.field)
        {
            var grabbableChip = other.GetComponent<GrabbableChip>();

            if (grabbableChip)
            {
                if (grabbableChip.isGrabbed)
                {
                    Debug.Log("chip added");
                    lastChip = grabbableChip;                   
                    triggeredChips.Add(other.GetComponent<ChipData>());
                    if (!chekingChipsStarted)
                        StartCoroutine(ChipInField());
                }               
            }


        }

    }

    protected override void OnTriggerExit(Collider other)
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
                    triggeredChips.Remove(chipdata);

                    if (triggeredChips.Count == 0)
                        StopAllCoroutines();
                }
            }


        }

    }






}
