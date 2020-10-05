using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BettingFieldUIElements : MonoBehaviour, IListener<AbstractFieldEvents>
{
    [SerializeField]
    float currentHightByZ;
    [SerializeField]
    float currentHightOffsetByZ = 0.1f;
    [SerializeField]
    string preText = "bet ";
    [SerializeField]
    string afterText = " $";
    
   

    [SerializeField]
    GameObject TMP_3D;
    [SerializeField]
    GameObject FieldShader;
    [SerializeField]
    GameObject MoneyInFieldEffect;

    private TextMeshPro text;
    private Vector3 defaultHightText;
    private AudioSource source;


    EventManager<AbstractFieldEvents> _fieldEventManager;

    int sumOfPoints = 0;
    bool ignoreVibration = false;

    void Start()
    {
        _fieldEventManager = GetComponent<AbstractField>().FieldEventManager;

        _fieldEventManager.AddListener(AbstractFieldEvents.StackAnimationStarted, this);
        _fieldEventManager.AddListener(AbstractFieldEvents.UpdateUI, this);
        _fieldEventManager.AddListener(AbstractFieldEvents.FieldUnbloked, this);
        _fieldEventManager.AddListener(AbstractFieldEvents.FieldBloked, this);
        _fieldEventManager.AddListener(AbstractFieldEvents.ExtractObject, this);


        text = TMP_3D.GetComponent<TextMeshPro>();
        source = GetComponent<AudioSource>();
     
        text.gameObject.SetActive(false);
        defaultHightText = TMP_3D.transform.position;
    }

    public void OnEvent(AbstractFieldEvents Event_type, Component Sender, params object[] Param)
    {
        switch (Event_type)
        {
           
            case AbstractFieldEvents.UpdateUI:

                sumOfPoints = Convert.ToInt32(Param[1]);

                if (sumOfPoints > 0)
                {
                    TMP_3D.SetActive(true);
                    currentHightByZ = (float)Convert.ToDouble(Param[0]);
                    TMP_3D.transform.position = new Vector3(defaultHightText.x, defaultHightText.y + currentHightByZ + currentHightOffsetByZ, defaultHightText.z);


                    text.SetText(preText + sumOfPoints + afterText);
                }
                else TMP_3D.SetActive(false);
                    break;

            case AbstractFieldEvents.StackAnimationStarted:
                if(TMP_3D.activeSelf)
                    TMP_3D.SetActive(false);
                break;

            case AbstractFieldEvents.FieldBloked:
                if (FieldShader)
                {
                    FieldShader.SetActive(false);
                    MoneyInFieldEffect.SetActive(false);
                }
                break;

            case AbstractFieldEvents.FieldUnbloked:
                if (FieldShader)
                {
                    FieldShader.SetActive(true);
                }
                break;
            case AbstractFieldEvents.ExtractObject:
                sumOfPoints -= Convert.ToInt32(Param[0]);
                if (sumOfPoints > 0)
                {
                    if(!TMP_3D.activeSelf)
                        TMP_3D.SetActive(true);

                    text.SetText(preText + sumOfPoints + afterText);
                    currentHightByZ = (float)Convert.ToDouble(Param[1]);
                    TMP_3D.transform.position = new Vector3(defaultHightText.x, defaultHightText.y + currentHightByZ + currentHightOffsetByZ, defaultHightText.z);
                }
                else TMP_3D.SetActive(false);

                break;
        }
    }

    private void Update()
    {
        if(TMP_3D.gameObject.activeSelf)
            TMP_3D.transform.LookAt(Camera.main.transform.position);
    }

    IEnumerator FieldVibration()
    {
        OpenVRVibrationManager.DoVibration(0.5f, 0.05f);
        ignoreVibration = true;
        yield return new WaitForSeconds(0.2f);
        ignoreVibration = false;
    }
    void ActivateBettingEffect(bool activate, Collider other)
    {
        if (FieldShader && FieldShader.activeSelf && MoneyInFieldEffect && other.GetComponent<ChipData>() != null)
        {
            MoneyInFieldEffect.SetActive(true);
            if (source)
                source.Play();

            if (!ignoreVibration)
            {
                StartCoroutine(FieldVibration());
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ActivateBettingEffect(true, other);
    }

    private void OnTriggerExit(Collider other)
    {
        ActivateBettingEffect(false, other);
    }
}
