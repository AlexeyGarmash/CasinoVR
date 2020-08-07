using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRoulette : MonoBehaviour
{
    private const string SPIN_ANIM = "Spin";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void RunSpinAnimation()
    {
        
    }

    

}
