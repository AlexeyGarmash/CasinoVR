using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
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
        print("Croupier start animation");
        photonView.RPC("StartSpinAnimation_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void StartSpinAnimation_RPC()
    {
        print("Croupier start animation Coroutine");
        animatorController.SetTrigger(SPIN_TRIGGER);
        StartCoroutine(RealStartRoulette(0.5f));
    }

    public IEnumerator RealStartRoulette(float pause)
    {
        print("Start pause");
        yield return new WaitForSeconds(pause);
        print("End pause");
        onCroupierPausedOff.Invoke();
    }
}
