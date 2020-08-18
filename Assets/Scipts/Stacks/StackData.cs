using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StackAnimator))]
public class StackData : MonoBehaviourPun
{

    public string playerName = "";
    public string stackType = "";


    [PunRPC]
    private void SetStackType_RPC(string _stackType)
    {
        stackType = _stackType;
    }
    public void SetStackType(string stackType)
    {
        photonView.RPC("SetStackType_RPC", RpcTarget.All, stackType);
    }
    public List<GameObject> Objects = new List<GameObject>();

    

    public StackAnimator animator;

    private void Start()
    {
        animator = GetComponent<StackAnimator>();
    }

    public void ExtractAll()
    {
        animator.Clear();
        Objects.ForEach(o => o.GetComponent<Rigidbody>().isKinematic = false);
        Objects.Clear();
        playerName = "";
        stackType = "";
    }
    public virtual void ClearData()
    {

        animator.Clear();
        foreach (var chip in Objects)
            Destroy(chip);

        Objects.Clear();     
        playerName = "";
        stackType = "";

    }
    public void StartAnim(GameObject chip)
    {        
        animator.StartAnim(chip);
    }
    public void UpdateStackInstantly()
    {       
        animator.UpdateStackInstantly();
    }








}




