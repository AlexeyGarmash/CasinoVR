using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarBodyHolder : MonoBehaviour
{
    public GameObject Hair;
    public GameObject Head;
    public GameObject Beard;
    public GameObject Shirt;
    public GameObject Glasses;

    public void ChangeHair(Material material, Mesh mesh)
    {
        Hair.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
        Hair.GetComponent<SkinnedMeshRenderer>().material = material;
    }

    public void ChangeBeard(Material material, Mesh mesh)
    {
        Beard.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
        Beard.GetComponent<SkinnedMeshRenderer>().material = material;
    }

    public void ChangeShirt(Material material, Mesh mesh)
    {
        Shirt.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
        Shirt.GetComponent<SkinnedMeshRenderer>().material = material;
    }

    internal void ChangeGlasses(Material material, Mesh mesh)
    {
        Glasses.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
        Glasses.GetComponent<SkinnedMeshRenderer>().material = material;
    }

    internal void ChangeHead(Material mat, Mesh mesh)
    {
        Head.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
        Head.GetComponent<SkinnedMeshRenderer>().material = mat;
    }

    internal void ChangeHairColor(Color customColor)
    {
        print($"[Avatar Body Holder] Request to change hair color {customColor}");
        Hair.GetComponent<SkinnedMeshRenderer>().sharedMaterial.SetColor("_BaseColor", customColor);
    }

    internal void ChangeBeardColor(Color customColor)
    {
        print("[Avatar Body Holder] Request to change beard color");
        Beard.GetComponent<SkinnedMeshRenderer>().sharedMaterial.SetColor("_BaseColor", customColor);
    }
}
