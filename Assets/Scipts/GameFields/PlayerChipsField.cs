using OVR.OpenVR;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerChipsField : ChipsField
{
    GameObject yellowChipPrefab;
    GameObject redChipPrefab;
    GameObject blueChipPrefab;
    GameObject greenChipPrefab;
    GameObject blackChipPrefab;
    GameObject purpleChipPrefab;

    [SerializeField]
    private float yOffset = 0.0073f;

    [SerializeField]
    private Transform SpawnPos;

    PhotonView View;

    private void Start()
    {
        SetPrefabs();

        View = GetComponent<PhotonView>();

    }


    private void SetPrefabs()
    {
        yellowChipPrefab = ChipUtils.Instance.yellowChipPrefab;
        redChipPrefab = ChipUtils.Instance.redChipPrefab;
        blueChipPrefab = ChipUtils.Instance.blueChipPrefab;
        greenChipPrefab = ChipUtils.Instance.greenChipPrefab;
        blackChipPrefab = ChipUtils.Instance.blackChipPrefab;
        purpleChipPrefab = ChipUtils.Instance.purpleChipPrefab;
    }


    //private void InstatiateChip(GameObject prefab, Chips chipsCost, ref int money)
    //{

    //    Instantiate(prefab, SpawnPos);
    //    money -= (int)chipsCost;
    //}

    [PunRPC]
    private void InstantiateChip_RPC(int color)
    {
        switch ((Chips)color)
        {
            case Chips.BLACK:
                Instantiate(blackChipPrefab, SpawnPos);
                break;
            case Chips.BLUE:
                Instantiate(blueChipPrefab, SpawnPos);
                break;
            case Chips.GREEN:
                Instantiate(greenChipPrefab, SpawnPos);
                break;
            case Chips.PURPLE:
                Instantiate(purpleChipPrefab, SpawnPos);
                break;
            case Chips.RED:
                Instantiate(redChipPrefab, SpawnPos);
                break;
            case Chips.YELLOW:
                Instantiate(yellowChipPrefab, SpawnPos);
                break;              
        }
      
    }
    public void InstantiateToStackWithColor(Chips chipsCost, ref int money)
    {

        View.RPC("InstantiateChip_RPC", RpcTarget.All, (int)chipsCost);
        money -= (int)chipsCost;
    }
      
}
