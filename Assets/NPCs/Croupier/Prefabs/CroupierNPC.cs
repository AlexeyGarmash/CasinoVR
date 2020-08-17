using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class CroupierNPC : MonoBehaviourPun
{
    public Action onCroupierPausedOff;
    public const string SPIN_TRIGGER = "CroupierSpin";

    private Animator animatorController;

    private void Awake()
    {
        animatorController = GetComponent<Animator>();
    }

    public void StartSpinAnimation()
    {
        photonView.RPC("StartSpinAnimation_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void StartSpinAnimation_RPC()
    {
        animatorController.SetTrigger(SPIN_TRIGGER);
        StartCoroutine(RealStartRoulette(0.5f));
    }

    public IEnumerator RealStartRoulette(float pause)
    {
        yield return new WaitForSeconds(pause);
        onCroupierPausedOff.Invoke();
    }
}
