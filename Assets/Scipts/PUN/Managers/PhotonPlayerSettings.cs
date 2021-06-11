using System;
using UnityEngine;
using static CustomizeAvatarPartV2;

public enum PlayerAvatarType
{
    Man1,
    Woman1,
    Man2,
    Woman2
}
public class PhotonPlayerSettings : MonoBehaviour
{
    public const string NoInfo = "[NONE]";
    public const string BaseHairMaterialsResourcePath = "Avatars/Materials/{0}/Hairs/{1}";
    public const string BaseBeardMaterialsResourcePath = "Avatars/Materials/{0}/Beards/{1}";
    public const string BaseGlassesMaterialsResourcePath = "Avatars/Materials/{0}/Glasses/{1}";
    public const string BaseShirtMaterialsResourcePath = "Avatars/Materials/{0}/Shirts/{1}";
    public const string BaseFaceMaterialsResourcePath = "Avatars/Materials/{0}/Faces/{1}";

    public const string BaseHairMeshesResourcePath = "Avatars/Meshes/{0}/Hairs/{1}";
    public const string BaseBeardMeshesResourcePath = "Avatars/Meshes/{0}/Beards/{1}";
    public const string BaseGlassesMeshesResourcePath = "Avatars/Meshes/{0}/Glasses/{1}";
    public const string BaseShirtMeshesResourcePath = "Avatars/Meshes/{0}/Shirts/{1}";
    public const string BaseFaceMeshesResourcePath = "Avatars/Meshes/{0}/Faces/{1}";


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

    public C_BodyPart Hair;
    public C_BodyPart Head;
    public C_BodyPart Glasses;
    public C_BodyPart Shirt;
    public C_BodyPart Beard;

    public void SetupBodyPart(bool male, CA_Part bodyPart, Material material, Mesh mesh)
    {
        string gender = male ? "Male" : "Female";
        string matName = material != null ? material.name : NoInfo;
        string meshName = mesh != null ? mesh.name : NoInfo;

        C_BodyPart bdPart = new C_BodyPart();
        bdPart.IsChanged = true;
        bdPart.Gender = gender;
        bdPart.Mat = material;
        bdPart.Mesh = mesh;
        bdPart.MaterialName = matName;
        bdPart.MeshName = meshName;

        switch (bodyPart)
        {
            case CA_Part.Glasses:
                Glasses = bdPart;
                break;
            case CA_Part.Beard:
                Beard = bdPart;
                break;
            case CA_Part.Hair:
                Hair = bdPart;
                break;
            case CA_Part.Head:
                Head = bdPart;
                break;
            case CA_Part.Shirt:
                Shirt = bdPart;
                break;
                
        }
    }

    public void SetupBodyPartColor(CA_Part bodyPart, Color customColor)
    {
        switch(bodyPart)
        {
            case CA_Part.Hair:
                Hair.IsColored = true;
                Hair.ColorName = customColor.ToStringColor();
                break;

            case CA_Part.Beard:
                Beard.IsColored = true;
                Beard.ColorName = customColor.ToStringColor();
                break;
        }
    }

    public void ResetBodyParts()
    {
        Hair = null;
        Head = null;
        Glasses = null;
        Shirt = null;
        Beard = null;
    }

    public string GetCustomizeAvatarJsonData()
    {
        CustomizeJsonData customizeJsonData = new CustomizeJsonData(Hair, Head, Glasses, Shirt, Beard);
        string jsonString = JsonUtility.ToJson(customizeJsonData, true);
        return jsonString;
    }

    public CustomizeJsonData RestoreCustomizeDataFromJson(string jsonData)
    {
        CustomizeJsonData customizeJsonData = JsonUtility.FromJson<CustomizeJsonData>(jsonData);
        return customizeJsonData;
    }

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
