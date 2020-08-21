using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadialSector : MonoBehaviour
{
    public enum RadialMenuSector
    {
        RIGHT,
        LEFT,
        UP,
        DOWN
    }

    [Header("UI elements")]
    [SerializeField] private RawImage _imageSector;
    [SerializeField] private Color _colorSelected;
    [SerializeField] private Color _colorUnselected;
    [SerializeField] private TMP_Text _textSector;
    [Range(1, 1.2f)]
    [SerializeField] private float _scaleMult;

    [Header("Sector info")]
    [SerializeField] private RadialMenuSector _radialMenuSector;
    [SerializeField] private string _sectorText;

    

    public RadialMenuSector radialMenuSector { get => _radialMenuSector; }

    private void Start()
    {
        _imageSector = GetComponentInChildren<RawImage>();
        _textSector = GetComponentInChildren<TMP_Text>();
        _textSector.text = _sectorText;
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
        _imageSector.color = _colorSelected;
        _imageSector.rectTransform.localScale = new Vector3(1f * _scaleMult, 1f * _scaleMult, 1f);
    } 

    private void UnSelectSector()
    {
        _imageSector.color = _colorUnselected;
        _imageSector.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }
}
