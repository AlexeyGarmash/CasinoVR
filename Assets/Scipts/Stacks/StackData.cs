using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StackAnimator))]
public class StackData : MonoBehaviourPun
{
    public string playerName = "";
    public string stackType = "";

    [SerializeField]
    public bool destoyableStack = false;

    [SerializeField]
    public AbstractField owner;
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

    private void Awake()
    {
        owner = GetComponentInParent<AbstractField>();
        animator = GetComponent<StackAnimator>();
    }
    public void ExtractOne(GameObject obj)
    {
        if (Objects.Contains(obj))
        {
            obj.transform.parent = null;
            Objects.Remove(obj);
           
        }

        UpdateStackInstantly();

        if (Objects.Count == 0 && destoyableStack)
            StartCoroutine(DestroySalfInEndOfFrame());
    }
    public List<GameObject> ExtractAll()
    {
        animator.Clear();
        Objects.ForEach(o => o.GetComponent<Rigidbody>().isKinematic = false);

        var tempObject = new List<GameObject>();
        tempObject.AddRange(Objects);
        Objects.Clear();
        playerName = "";
        stackType = "";


        if (Objects.Count == 0 && destoyableStack)
            StartCoroutine(DestroySalfInEndOfFrame());

        return tempObject;
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

    IEnumerator DestroySalfInEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        Destroy(this.gameObject);
    }
    public void StartAnim(GameObject chip)
    {        
        animator.StartAnim(chip);
    }
    public void UpdateStackInstantly()
    {       
        animator.UpdateStackInstantly();
    }



    private void OnDestroy()
    {
        if(owner)
            owner.Stacks.Remove(this);

    }




}




