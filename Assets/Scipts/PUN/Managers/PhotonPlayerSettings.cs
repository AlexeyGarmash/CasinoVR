using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAvatarType
{
    Man1,
    Woman1,
    Man2,
    Woman2
}
public class PhotonPlayerSettings : MonoBehaviour
{
    public const string BaseTexturesResPath = "Textures/";
    public static PhotonPlayerSettings Instance;
    [Serializable]
    public class SkinData
    {
        public string skinColor;
        public string hairColor;
        public string irisColor;
        public string dressColor;
        public string lipsColor;
        public string textureName;

        public SkinData(Color _skinColor, Color _hairColor, Color _irisColor, Color _dressColor, Color _lipsColor, string textureName)
        {
            skinColor = _skinColor.ToStringColor();
            hairColor = _hairColor.ToStringColor();
            irisColor = _irisColor.ToStringColor();
            dressColor = _dressColor.ToStringColor();
            lipsColor = _lipsColor.ToStringColor();
            this.textureName = textureName;
        }
    }

    public NetworkManager.CasinoGameTypes CurrentGameType { get; set; } = NetworkManager.CasinoGameTypes.Slots;
    public string PrefabResourceName { get; set; }
    public Material PrefabMaterial { get; set; }

    public Color SkinColor { get; set; } = Color.white;
    public Color HairColor { get; set; } = Color.white;
    public Color IrisColor { get; set; } = Color.white;
    public Color DressColor { get; set; } = Color.white;
    public Texture DressTexture { get; set; }
    public Color LipsColor { get; internal set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(Instance == null)
            Instance = this;
        CurrentGameType = NetworkManager.CasinoGameTypes.Slots;
    }

    public string GetJsonSkinData()
    {
        SkinData skinData = new SkinData(SkinColor, HairColor, IrisColor, DressColor, LipsColor, DressTexture == null ? "" : DressTexture.name);
        string jsonSkinData = JsonUtility.ToJson(skinData);
        return jsonSkinData;
    }

    public SkinData GetSkinData(string jsonSkinData)
    {
        SkinData skinData = JsonUtility.FromJson<SkinData>(jsonSkinData);
        return skinData;
    }
}
