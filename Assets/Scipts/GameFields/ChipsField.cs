using Photon.Pun;
using System.Linq;
using UnityEngine;


public class ChipsField : AbstractField
{
    

   


    protected void OnTriggerEnter(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        var gc = other.gameObject.GetComponent<GrabbableChip>();
        var rb = other.GetComponent<Rigidbody>();
        var view = gameObj.GetComponent<PhotonView>();

        if (chip != null && gc != null && !gc.isGrabbed && !rb.isKinematic && view != null && photonView.IsMine)
        {
         
            chip.field = this;
            var stacks = FindPossibleFields(chip);
            StackData stack;

            if (stacks.Exists(s => s.stackType != ""))
                stack = stacks[0];
            else stack = stacks.FirstOrDefault(s => s.stackType == "");

            MagnetizeObject(gameObj, stack, ChipUtils.Instance.GetStringOfType(chip.Cost));

        }

    }

    public override void OnEvent(AbstractFieldEvents Event_type, Component Sender, params object[] Param)
    {
        base.OnEvent(Event_type, Sender, Param);


        if (photonView.IsMine)
        {
            switch (Event_type)
            {
                case AbstractFieldEvents.StackAnimationEnded:

                   
                    if (StackAnimStartedCounter == StackAnimEndedCounter)
                    {
                        

                        float maxZ = Stacks.Max(ss => ss.animator.currentZ);


                        int money = 0;

                        Stacks.ToList().ForEach(s => {
                            s.Objects.ForEach(o => {
                                money += (int)o.GetComponent<ChipData>().Cost;
                            });
                        });

                        _fieldEventManager.PostNotification(AbstractFieldEvents.UpdateUI, this, maxZ, money);
                    }

                    break;
                
            }
        }
    }

    public override GameObject ExtranctObject(int viewID)
    {
        var chip = base.ExtranctObject(viewID);

        

        float maxZ = Stacks.Max(ss => ss.animator.currentZ);

        _fieldEventManager.PostNotification(AbstractFieldEvents.ExtractObject, this, (int)chip.GetComponent<ChipData>().Cost, maxZ);

        return chip;
    }
    #region Unity Callbacks



    #endregion



}


