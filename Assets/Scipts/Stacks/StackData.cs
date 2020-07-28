using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StackAnimator))]
public class StackData : MonoBehaviour
{

    public string playerName = "";
   
    public List<GameObject> Objects = new List<GameObject>();

    public StackAnimator animator;

    private void Start()
    {
        animator = GetComponent<StackAnimator>();
    }
    public virtual void ClearData()
    {
        foreach (var chip in Objects)
            Destroy(chip);

        Objects.Clear();
        animator.Clear();
        playerName = "";      

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




