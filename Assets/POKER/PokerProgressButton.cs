using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PokerProgressButton : MonoBehaviourPun
{
    [SerializeField] protected Image progressImage;
    [SerializeField] protected TMP_Text buttonText;
    [SerializeField] protected RawImage backImage;

    [SerializeField] protected Texture positiveTexture;
    [SerializeField] protected Texture negativeTexture;

    [SerializeField] protected float _holdTime;

    public PokerPlayerPlace PokerPlayerPlace;
    

    protected bool inProgress = false;
    protected float currentHoldTime = 0f;
    protected bool clickedAlready = false;

    protected void Start()
    {
        PokerPlayerPlace = GetComponentInParent<PokerPlayerPlace>();
        backImage.texture = negativeTexture;
        progressImage.fillAmount = 0f;
    }

    protected void Update()
    {
        if (inProgress)
        {
            ShowProgress();
        }
    }

    protected void ResetProgress()
    {
        inProgress = false;
        currentHoldTime = 0f;
        FillImageProgress(currentHoldTime);
    }
    protected void FillImageProgress(float progress)
    {
        progressImage.fillAmount = progress;
    }

    protected void ShowProgress()
    {
        currentHoldTime += Time.deltaTime;
        if (currentHoldTime >= _holdTime)
        {
            if (!clickedAlready)
            {
                InvokeClickIn();
            }
            else
            {
                InvokeClickOut();
            }
            ResetProgress();
        }
        FillImageProgress(currentHoldTime / _holdTime);
    }

    protected virtual void InvokeClickOut()
    {

    }
    

    protected virtual void InvokeClickIn()
    {
        
    }

    protected Collider lastCollider;
    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        
    }
}
