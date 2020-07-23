using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PlayerBettingChipsField : MonoBehaviour
{
             

    public PlayerStackData playerBettingStack;
    

    public float yOffset = 0.0073f;
   

    private void OnTriggerEnter(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        if (chip != null && chip.transform.parent == null)
        {
            chip.GetComponent<Collider>().isTrigger = true;
            StackUtils.Instance.MagnetizeObject(gameObj, chip.player, yOffset, playerBettingStack);
            chip.transform.parent = playerBettingStack.transform;
            
        }

    }

    private void OnTriggerStay(Collider other)
    {

        var chip = other.GetComponent<ChipData>();
        if (chip != null)
        {
            if (other.GetComponent<GrabbableChip>().grabbedBy != null)
            {
                StackUtils.Instance.ExtractionObject(other.gameObject, yOffset, playerBettingStack);

            }
        }

    }

        
       

       
}

