using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizeAvatarPartV2 : MonoBehaviour//, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<CustomizeAvatarPartV2> OnBodyPartChanged;
    public enum CA_Part
    {
        Hair,
        Head,
        Beard,
        Glasses,
        Shirt
    }

    [SerializeField] private Button BtnChooseBodyPart;
    public CA_Part BodyPartType;
    public Mesh MeshData;
    public Material MaterialData;
    public Texture TextureData;
    public Texture NormalData;


    private void Start()
    {
        BtnChooseBodyPart.onClick.AddListener(OnButtonChoosePartClicked);
    }

    private void OnButtonChoosePartClicked()
    {
        OnBodyPartChanged.Invoke(this);
    }
}
