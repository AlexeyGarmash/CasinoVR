using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoButton : MonoBehaviour
{
    [SerializeField] private float pressLength;
    public int index;
    private PianoButtonSound buttonSound;
    public bool pressed;
    private Vector3 localStartPos;
    private bool clickPerformed;
    public PianoThumb currentPianoThumb;
    public float observDistance;

    private bool thumbExit = false;

    private void Start()
    {
        localStartPos = transform.localPosition;
        index = transform.GetSiblingIndex();
        buttonSound = GetComponent<PianoButtonSound>();
    }

    private void Update()
    {
        if(thumbExit)
        {
            float distance = Vector3.Distance(currentPianoThumb.transform.position, transform.position);
            if (distance > 0.08)
            {
                pressed = false;
                currentPianoThumb = null;
                thumbExit = false;
            }
        }
        if(pressed)
        {
            if (!clickPerformed)
            {
                transform.localPosition = new Vector3(localStartPos.x, localStartPos.y, localStartPos.z + pressLength);
                //some action
                MakeKeyActionDown();
                print(string.Format("Button {0} clicked", gameObject.name));
                clickPerformed = true;
            }
        }
        else
        {
            if (clickPerformed)
            {
                transform.localPosition = new Vector3(localStartPos.x, localStartPos.y, localStartPos.z);
                MakeKeyActionUp();
                clickPerformed = false;
            }
        }
    }

    private void MakeKeyActionDown()
    {
        buttonSound.PlayKeySound(index);
    }

    private void MakeKeyActionUp()
    {
        buttonSound.StopKeySound(-1);
    }

    private void OnTriggerEnter(Collider other)
    {
        var pianoThumb = other.GetComponent<PianoThumb>();
        if(pianoThumb != null && !pressed)
        {
            observDistance = Vector3.Distance(pianoThumb.transform.position, transform.position);
            thumbExit = false;
            pressed = true;
            currentPianoThumb = pianoThumb;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var pianoThumb = other.GetComponent<PianoThumb>();
        if(pianoThumb != null && pressed && pianoThumb == currentPianoThumb)
        {
            /*float distance = Vector3.Distance(pianoThumb.transform.position, transform.position);
            if (distance > 0.07)
            {
                pressed = false;
                currentPianoThumb = null;
            }*/
            thumbExit = true;
        }
    }



}
