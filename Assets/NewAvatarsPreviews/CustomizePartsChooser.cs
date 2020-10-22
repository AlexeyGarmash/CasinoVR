using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CustomizeAvatarPartV2;

public class CustomizePartsChooser : MonoBehaviour
{
    public CA_Part BodyPartType;
    public PlayerAvatarType PlayerAvatarType;

    public List<CustomizeAvatarPartV2> CustomParts;


    private void Start()
    {
        foreach (var customPart in CustomParts)
        {
            customPart.BodyPartType = BodyPartType;
        }
        /*if(CustomParts !=null && CustomParts.Count == 0)
        {
            CustomParts.Add(GetComponentInChildren<CustomizeAvatarPartV2>());
        }*/
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        SendPartsToCustomize();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void SendPartsToCustomize()
    {
        CustomizeAvatarV2.Instance.SetPartsToCustomize(this);
    }

    
}
