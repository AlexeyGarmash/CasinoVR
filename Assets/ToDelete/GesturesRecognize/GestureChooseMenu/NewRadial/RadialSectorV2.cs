using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum RadialMenuSectorV2
{
    FIRST_1_SECTORS,

    FIRST_2_SECTORS,
    SECOND_2_SECTORS,

    FIRST_3_SECTORS,
    SECOND_3_SECTORS,
    THIRD_3_SECTORS,

    FIRST_4_SECTORS,
    SECOND_4_SECTORS,
    THIRD_4_SECTORS,
    FOURTH_4_SECTORS,

    FIRST_5_SECTORS,
    SECOND_5_SECTORS,
    THIRD_5_SECTORS,
    FOURTH_5_SECTORS,
    FIFTH_5_SECTORS,

    FIRST_6_SECTORS,
    SECOND_6_SECTORS,
    THIRD_6_SECTORS,
    FOURTH_6_SECTORS,
    FIFTH_6_SECTORS,
    SIXTH_6_SECTORS

}



public class RadialActionInfo
{
    public RadialActionInfo()
    {
        Text = "Default text";
        OnRadialAction = () => { };
    }
    public RadialActionInfo(Action onRadialAction, string text)
    {
        Text = text;
        OnRadialAction = onRadialAction;
    }
    public Action OnRadialAction { get; set; }
    public string Text { get; set; }


}
public class RadialSectorV2 : MonoBehaviour
{
    public static RadialActionInfo defaultActionData = new RadialActionInfo();

    private RadialActionInfo currentActionData;
    [Header("UI elements")]
    [SerializeField] private Image imageSector;
    [SerializeField] private Color colorSelected;
    [SerializeField] private Color colorUnselected;
    [SerializeField] private TMP_Text textSector;
    [Range(1, 1.2f)]
    [SerializeField] private float scaleMult;

    [Header("Sector info")]
    [SerializeField] private RadialMenuSectorV2 sectorType;

    [SerializeField] private string sectorText;

   

    public RadialMenuSectorV2 RadialSectorType { get => sectorType; }

    private void Awake()
    {
        currentActionData = defaultActionData;
        imageSector = GetComponentInChildren<Image>();
        textSector = GetComponentInChildren<TMP_Text>();
        textSector.SetText(defaultActionData.Text);
    }

    public void UseAction()
    {
        currentActionData.OnRadialAction();
    }
    public void SetSectorData(RadialActionInfo actionInfo)
    {
        currentActionData = actionInfo;
        textSector.SetText(actionInfo.Text);
    }
    public void ClearActionInfo()
    {
        currentActionData = defaultActionData;
        textSector.SetText(defaultActionData.Text);
    }
    public void ResolveSelectSector(bool select)
    {
        if(select)
        {
            SelectSector();
        }
        else
        {
            UnSelectSector();
        }
    }

    private void SelectSector()
    {
        imageSector.color = colorSelected;
        imageSector.rectTransform.localScale = new Vector3(1f * scaleMult, 1f * scaleMult, 1f);
    }

    private void UnSelectSector()
    {
        imageSector.color = colorUnselected;
        imageSector.rectTransform.localScale = Vector3.one;
    }
}
