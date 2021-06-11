using System;
using UnityEngine;
using static CustomizeAvatarPartV2;

[Serializable]
public class C_BodyPart
{
    public bool IsColored = false;
    public bool IsChanged = false;
    public string Gender;
    public Material Mat;
    public Mesh Mesh;
    public string MaterialName;
    public string MeshName;
    public string ColorName;
}


[Serializable]
public class CustomizeJsonData
{
    public C_BodyPart Hair;
    public C_BodyPart Head;
    public C_BodyPart Glasses;
    public C_BodyPart Shirt;
    public C_BodyPart Beard;

    public CustomizeJsonData(C_BodyPart hair, C_BodyPart head, C_BodyPart glasses, C_BodyPart shirt, C_BodyPart beard)
    {
        this.Hair = hair;
        this.Head = head;
        this.Glasses = glasses;
        this.Shirt = shirt;
        this.Beard = beard;
    }
}
