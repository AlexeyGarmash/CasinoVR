using System.Linq;
using UnityEngine;
using Valve.VR;

/// <summary>
/// Override the material and texture of the HTC Vive controllers, with your own material after SteamVR has loaded and
/// applied the original material. This is useful to help preserve interactions in the model itself.
///
/// NOTE: This is only compatible with the default HTC vive controllers (see UpdateControllerMaterial() below).
///
/// Modified by Patrick Nelson / chunk_split (pat@catchyour.com) from original "OverrideControllerTexture" class
/// by Mr_FJ (from https://steamcommunity.com/app/358720/discussions/0/357287304420388604/) to allow override of full
/// material instead of just textures and also utilize the latest SteamVR_Events model.
///
/// See also: https://forum.unity.com/threads/changing-the-htc-vive-controller-model-with-a-custom-hand-model.395107/
/// </summary>
public class OverrideControllerMaterial : MonoBehaviour
{
    #region Public variables
    [Header("Variables")]
    public Material defaultMaterial;
  

    private MeshRenderer[] meshes;
    #endregion

    private void Start()
    {
        meshes = GetComponentsInChildren<MeshRenderer>();

        meshes.ToList().ForEach(r => r.material = defaultMaterial);
    }
   
}