using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingWithProgressManager : MonoBehaviour
{
    [SerializeField] private TMP_Text ProgressText;

    public void SetProgress(float progress)
    {
        ProgressText.text = string.Format("Loading {0}%", (int)(progress * 100));
    }
}
