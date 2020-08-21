using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GestureLoader : MonoBehaviour
{
    [SerializeField] private Image _imageLoader;
    [SerializeField] private TMP_Text _textTime;

    private void Start()
    {
        Hide(true);
    }
    public void Hide(bool hide)
    {
        gameObject.SetActive(!hide);
    }

    public void SetProgress(float progressCurrent, float total, bool hide)
    {
        Hide(hide);
        if (hide)
        { 
            return;
        }
        _imageLoader.fillAmount = progressCurrent / total;
        _textTime.text = string.Format("{0}s", (total - (int)progressCurrent));
    }
}
