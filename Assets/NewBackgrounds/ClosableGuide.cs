using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClosableGuide : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] RawImage ButtonImageState;
    [SerializeField] Texture CloseTexture;
    [SerializeField] Texture InfoTexture;
    [SerializeField] Button ButtonCloseGuide;
    [SerializeField] GameObject Teleport;
    [SerializeField] GameObject ObjectToClose;
    [SerializeField] Transform PointToTeleportCloseButton;
    private bool ObjectToCloseShowed = true;
    private Vector3 StartPosition;
    private Quaternion StartRotation;

    private void Start()
    {
        ButtonCloseGuide.onClick.AddListener(OnGuideCloseClicked);
        StartPosition = transform.position;
        StartRotation = transform.rotation;
    }

    private void OnGuideCloseClicked()
    {
        print("on close clicked");
        ObjectToCloseShowed = !ObjectToCloseShowed;
        ObjectToClose.SetActive(ObjectToCloseShowed);
        ButtonImageState.texture = ObjectToCloseShowed ? CloseTexture : InfoTexture;
        if(ObjectToCloseShowed)
        {
            transform.position = StartPosition;
            transform.rotation = StartRotation;
        }
        else
        {
            transform.position = PointToTeleportCloseButton.position;
            transform.rotation = PointToTeleportCloseButton.rotation;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("pointer enter to click button");
        Teleport.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("pointer exit to click button");
        Teleport.SetActive(true);
    }
}
