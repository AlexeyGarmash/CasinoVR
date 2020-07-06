using SlotMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonPad : MonoBehaviour
{
    public float pressLength;
    public bool pressed;
    EventManager em;
    [SerializeField] private EVENT_TYPE ButtonEvent;
    Vector3 startPos;
    Rigidbody rb;

    void Start()
    {
        em = gameObject.transform.parent.GetComponent<EventManager>();
        startPos = transform.localPosition;
        rb = GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        // If our distance is greater than what we specified as a press
        // set it to our max distance and register a press if we haven't already
        float distance = Mathf.Abs(transform.localPosition.y - startPos.y);
        //Debug.Log("Distance: " + distance);
        if (distance >= pressLength)
        {
            // Prevent the button from going past the pressLength
            transform.localPosition = new Vector3(transform.localPosition.x, startPos.y - pressLength, transform.localPosition.z);
            if (!pressed)
            {
                pressed = true;
                // If we have an event, invoke it
                //downEvent?.Invoke();
                em.PostNotification(ButtonEvent, this, null);
            }
        }
        else
        {
            // If we aren't all the way down, reset our press
            pressed = false;
        }
        // Prevent button from springing back up past its original position
        if (transform.localPosition.y > startPos.y)
        {
            transform.localPosition = new Vector3(startPos.x, startPos.y, startPos.z);
        }
    }

    
}
