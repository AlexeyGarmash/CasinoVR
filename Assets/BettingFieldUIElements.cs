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

    private TextMeshPro text;

    private Vector3 defaultHightText;
   

   

    EventManager<AbstractFieldEvents> _fieldEventManager;

    int sumOfPoints = 0;

    void Start()
    {
        _fieldEventManager = GetComponent<AbstractField>().FieldEventManager;

        _fieldEventManager.AddListener(AbstractFieldEvents.StackAnimationStarted, this);
        _fieldEventManager.AddListener(AbstractFieldEvents.UpdateUI, this);
        _fieldEventManager.AddListener(AbstractFieldEvents.FieldUnbloked, this);
        _fieldEventManager.AddListener(AbstractFieldEvents.FieldBloked, this);
        _fieldEventManager.AddListener(AbstractFieldEvents.ExtractObject, this);


        text = TMP_3D.GetComponent<TextMeshPro>();

        defaultHightText = TMP_3D.transform.position;
    }

    public void OnEvent(AbstractFieldEvents Event_type, Component Sender, params object[] Param)
    {
        switch (Event_type)
        {
           
            case AbstractFieldEvents.UpdateUI:
               
                TMP_3D.SetActive(true);
                currentHightByZ = (float)Convert.ToDouble(Param[0]);
                TMP_3D.transform.position = new Vector3(defaultHightText.x, defaultHightText.y + currentHightByZ + currentHightOffsetByZ, defaultHightText.z);
                sumOfPoints = Convert.ToInt32(Param[1]);
                text.SetText(preText + sumOfPoints + afterText);
                break;

            case AbstractFieldEvents.StackAnimationStarted:
                if(TMP_3D.activeSelf)
                    TMP_3D.SetActive(false);
                break;

            case AbstractFieldEvents.FieldBloked:
                if(FieldShader)
                    FieldShader.SetActive(false);
                break;

            case AbstractFieldEvents.FieldUnbloked:
                if (FieldShader)
                    FieldShader.SetActive(true);
                break;
            case AbstractFieldEvents.ExtractObject:
                sumOfPoints -= Convert.ToInt32(Param[0]);
                text.SetText(preText + sumOfPoints + afterText);
                currentHightByZ = (float)Convert.ToDouble(Param[1]);
                TMP_3D.transform.position = new Vector3(defaultHightText.x, defaultHightText.y + currentHightByZ + currentHightOffsetByZ, defaultHightText.z);
                break;
        }
    }
}
