using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;

public abstract class AbstractField : MonoBehaviourPun, IMagnetize
{ 
    [SerializeField]
    public StackData[] Stacks;

    [SerializeField]
    protected int maxObjectsOnField = 20;

    protected void Awake()
    {
        Stacks = GetComponentsInChildren<StackData>();
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
        List<float> distances = new List<float>();
        for (var i = 0; i < PossibleField.Count; i++)
        {
            distances.Add(Vector3.Distance(PossibleField[i].transform.position, chip.position));
        }

        return PossibleField[distances.IndexOf(distances.Min())];
    }

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

    protected (GameObject chip, StackData stack) GetChipAndHisStack(int viewID)
    {
        var stack = Stacks.ToList().Find(s => s.Objects.Find(c => c.GetComponent<PhotonView>().ViewID == viewID));
        if (stack == null)
            return (null, null);
        var chip = stack.Objects.Find(s => s.GetComponent<PhotonView>().ViewID == viewID);

        return (chip, stack);


    }

    public abstract bool MagnetizeObject(GameObject Object, StackData Stack);

}


