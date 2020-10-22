using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizePanelUI : MonoBehaviour
{
    public List<CustomizeMainUiElement> ElementsForChooseParts;


    private void Start()
    {
        foreach (var chooserPart in ElementsForChooseParts)
        {
            chooserPart.OnBodyPartsToActivateChoosen += OnBodyPartsToActivateChoosen;
        }
    }

    private void OnBodyPartsToActivateChoosen(CustomizeMainUiElement customizeElement)
    {
        customizeElement.Activate();
        foreach (var chooserPart in ElementsForChooseParts)
        {
            if(chooserPart != customizeElement)
            {
                chooserPart.Deactivate();
            }
        }
    }

    public void ActivateOnlyUI()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateOnlyUI()
    {
        gameObject.SetActive(false);
    }

    public void DeactivateAll()
    {
        foreach (var chooserPart in ElementsForChooseParts)
        {
            chooserPart.Deactivate();
        }
        DeactivateOnlyUI();
    }
}
