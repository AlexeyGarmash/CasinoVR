using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStackData : StackData
{
    public Chips StackOfChipColor;

    public override void ClearData()
    {
        base.ClearData();
    }

    public void InstantiatePrefab(GameObject chip, Chips chipsCost, ref int money)
    {
        var spawnTransform = transform;
        var createdChip = Instantiate(chip, 
            new Vector3(
                spawnTransform.position.x,
                spawnTransform.position.y + currentY,
                spawnTransform.position.z
            ), new Quaternion(0, 0, 0, 0)
        );
        
        createdChip.transform.parent = spawnTransform;
        createdChip.transform.rotation = new Quaternion(0, 0, 0, 0);
        money -= (int)chipsCost;
        createdChip.GetComponent<Rigidbody>().isKinematic = true;

        Chips.Add(createdChip);
        ChipsUtils.Instance.UpdateStack(this);
    }

    public void UpdateChips()
    {
        
    }


}

