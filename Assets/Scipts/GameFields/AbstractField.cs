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
    protected int maxChipsOnField = 20;

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


    public abstract bool MagnetizeObject(GameObject Object, StackData Stack);

}


