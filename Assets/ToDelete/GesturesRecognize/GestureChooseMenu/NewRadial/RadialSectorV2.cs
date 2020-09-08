using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum RadialMenuSectorV2
{
    RIGHT,
    LEFT,
    TOP_RIGHT,
    TOP_LEFT,
    BOTTOM_RIGHT,
    BOTTOM_LEFT
}
public class RadialSectorV2 : MonoBehaviour
{
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

    private void Start()
    {
        imageSector = GetComponentInChildren<Image>();
        textSector = GetComponentInChildren<TMP_Text>();
        textSector.text = sectorText;
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
