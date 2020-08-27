using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDanceBehavour : StateMachineBehaviour
{
    private float entryTime;
    public float currentPercentage;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        entryTime = Time.time;
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentPercentage = (Time.time - entryTime) / stateInfo.length;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentPercentage = 0;
    }
}
