using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerSettings : MonoBehaviour
{
    public static PhotonPlayerSettings Instance;


    public string PrefabResourceName { get; set; }
    public Material PrefabMaterial { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(Instance == null)
            Instance = this;
    }
}
