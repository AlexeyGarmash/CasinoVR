using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackJackBettingField : ChipsField
{

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        var grabbableStack = other.GetComponent<StackData>();

        if (grabbableStack)
        {
            var chips = grabbableStack.ExtractAll();

            foreach (var chip in chips)
                MagnetizeObject(chip, Stacks[0]);
        }
    }
}
