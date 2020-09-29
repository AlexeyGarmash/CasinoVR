using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject TeleportGobject;
    [SerializeField] List<TMP_Text> TextsTimer;
    [SerializeField] List<TMP_Text> TextsPlayerNickName;
    [SerializeField] Button ButtonExitInLobby;

    private int secondsInGame = 0;
    private string timeStr = "";

    private void Start()
    {
        ButtonExitInLobby.onClick.AddListener(OnButtonExit_Clicked);
        TextsPlayerNickName.ForEach(textItem => textItem.text = PhotonNetwork.LocalPlayer.NickName);
        StartCoroutine(StartInGameTimer());
    }

    private void OnButtonExit_Clicked()
    {
        print("Im exiting from room now");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }

    private IEnumerator StartInGameTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            secondsInGame++;
            ConvertSecondsToTime();
        }
    }

    private void ConvertSecondsToTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(secondsInGame);
        timeStr = timeSpan.ToString(@"hh\:mm\:ss");
        TextsTimer.ForEach(textItem => textItem.text = timeStr);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Pointer enter canvas");
        TeleportGobject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("Pointer exit canvas");
        TeleportGobject.SetActive(true);
    }
}
