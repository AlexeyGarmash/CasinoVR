using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PlayerBettingChipsField : ChipsField
{
              

    public float yOffset = 0.0073f;
    

    private void OnTriggerEnter(Collider other)
    {
        var playerBettingStack = Stacks[0];

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        var gc = other.gameObject.GetComponent<GrabbableChip>();
        var rb = other.GetComponent<Rigidbody>();

        if (chip != null && gc != null && gc.grabbedBy == null && !rb.isKinematic)
        {
            chip.GetComponent<Collider>().isTrigger = true;
            MagnetizeObject(gameObj, playerBettingStack);
            chip.transform.parent = playerBettingStack.transform;
            
        }

    }

    private void OnTriggerStay(Collider other)
    {
       
        var chip = other.GetComponent<ChipData>();      
        var gc = other.gameObject.GetComponent<GrabbableChip>();

        var rb = other.GetComponent<Rigidbody>();
        if (chip != null && gc != null && gc.grabbedBy != null && rb.isKinematic)
        {
            ExtractionObject(other.gameObject);

            
        }

    }

        
       

       
}

