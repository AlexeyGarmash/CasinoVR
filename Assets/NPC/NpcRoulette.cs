using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRoulette : MonoBehaviour
{
    private const string SPIN_ANIM = "SpinT";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void RunSpinAnimation()
    {
        animator.SetTrigger(SPIN_ANIM);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RunSpinAnimation();
        }
    }
}
