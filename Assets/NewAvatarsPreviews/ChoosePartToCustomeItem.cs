using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class BodyPart
{
    public CustomizeAvatarManager.CustomizePart PartToCustomize;
    public string PartNameToCustomize;
}

public class ChoosePartToCustomeItem : MonoBehaviour
{
    public Action<BodyPart> OnCustomizePartChanged;
    public BodyPart BodyPart;
    private Button btnChooseItem;

    private void Start()
    {
        btnChooseItem = GetComponent<Button>();
        btnChooseItem.onClick.AddListener(OnBtn_Clicked);
    }

    private void OnBtn_Clicked()
    {
        if (OnCustomizePartChanged != null)
        {
            OnCustomizePartChanged.Invoke(BodyPart);
        }
    }
}
