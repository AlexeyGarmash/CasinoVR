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
    public static PhotonPlayerSettings Instance;


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
    }
}
