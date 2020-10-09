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
            StartCoroutine(StackInField(grabbableStack));
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        var grabbableStack = other.GetComponent<StackData>();

        if (grabbableStack)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator StackInField(StackData grabbableStack)
    {
        yield return new WaitForSeconds(3f);

        var chips = grabbableStack.ExtractAll();

        foreach (var chip in chips)
            MagnetizeObject(chip, Stacks[0]);
    }
}
