using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldAreaReact : MonoBehaviour, IListener<AbstractFieldEvents>
{
    AbstractField field;
    Animator animator;

    const string Inside = "Inside";
    public void OnEvent(AbstractFieldEvents Event_type, Component Sender, params object[] Param)
    {
        switch (Event_type)
        {
            case AbstractFieldEvents.ObjectInside:

                Debug.Log("Inside");
                animator.ResetTrigger(Inside);
                animator.SetTrigger(Inside);
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();       

        field = GetComponent<AbstractField>();
       
        if (field)
        {
            field.FieldEventManager.AddListener(AbstractFieldEvents.ObjectInside, this);          
        }
    }


}
