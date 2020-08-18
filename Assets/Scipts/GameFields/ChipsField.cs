using Assets.Scipts.Chips;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public enum AbstractFieldEvents{ StackAnimationEnded, StackAnimationStarted }
public class ChipsField : AbstractField
{
    
    public override bool MagnetizeObject(GameObject Object, StackData Stack)
    {
      
       
        var rb = Object.GetComponent<Rigidbody>();
        var chip = Object.GetComponent<ChipData>();

        if (Stack.objectType == "")
        Stack.objectType = ChipUtils.Instance.GetStringOfType(chip.Cost);

        var stackData = Stack;

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


    protected new void OnTriggerEnter(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        var gc = other.gameObject.GetComponent<GrabbableChip>();
        var rb = other.GetComponent<Rigidbody>();
        var view = gameObj.GetComponent<PhotonView>();

        if (chip != null && gc != null && !gc.isGrabbed && !rb.isKinematic && view != null)
        {
         
            chip.field = this;
            var clossest = FindClossestField(chip.transform, FindStackByType(ChipUtils.Instance.GetStringOfType(chip.Cost)));
            MagnetizeObject(gameObj, clossest);

        }

    }
    protected new void OnTriggerStay(Collider other)
    {

        //var gameObj = other.gameObject;
        //ChipData chip = other.gameObject.GetComponent<ChipData>();
        //var gc = other.gameObject.GetComponent<GrabbableChip>();
        //var rb = other.GetComponent<Rigidbody>();
        //var view = gameObj.GetComponent<PhotonView>();

        //if (chip != null && gc != null && gc.isGrabbed && rb.isKinematic && view != null && Contain(gameObj))
        //{
        //    ExtranctObject(view.ViewID);
        //    //SyncStacks();
        //}

    }

    #region Unity Callbacks



    #endregion



}


