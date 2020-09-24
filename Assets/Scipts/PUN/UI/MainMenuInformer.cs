using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MainMenuMessageType
{
    None,
    Info,
    Warning,
    Danger
}
public class MainMenuInformer : MonoBehaviour
{
    public static MainMenuInformer Instance;
    [SerializeField] private TMP_Text textInfo;
    [SerializeField] private float InfoExitTimeShort = 1.5f;
    [SerializeField] private float InfoExitTimeLong = 2.5f;
    [SerializeField] private List<RawImage> messageTypeImages;
    [SerializeField] private Texture infoTexture;
    [SerializeField] private Texture dangerTexture;
    [SerializeField] private Texture warningTexture;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowInfoWithExitTime(string message, MainMenuMessageType messageType)
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowWithTimer(message, messageType));
    } 


    private IEnumerator ShowWithTimer(string text, MainMenuMessageType messageType, bool longTime = false)
    {
        SetText(text);
        SetMessageType(messageType);
        yield return new WaitForSeconds(longTime ? InfoExitTimeLong : InfoExitTimeShort);
        SetText(string.Empty);
        SetMessageType(MainMenuMessageType.None);
        gameObject.SetActive(false);
    }

    private void SetText(string text)
    {
        textInfo.text = text;
    }

    private void SetMessageType(MainMenuMessageType messageType)
    {
        switch (messageType)
        {
            case MainMenuMessageType.Info:
                messageTypeImages.ForEach(mi => mi.texture = infoTexture);
                break;
            case MainMenuMessageType.Danger:
                messageTypeImages.ForEach(mi => mi.texture = dangerTexture);
                break;
            case MainMenuMessageType.Warning:
                messageTypeImages.ForEach(mi => mi.texture = warningTexture);
                break;
            case MainMenuMessageType.None:
                messageTypeImages.ForEach(mi => mi.texture = null);
                break;
        }
    }
}
