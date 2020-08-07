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

    private void Start()
    {
        SetPrefabs();
    

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
       
        GameObject prefab = null;

        switch ((Chips)color)
        {
            case Chips.BLACK:
                prefab = blackChipPrefab;
                break;
            case Chips.BLUE:
                prefab = blueChipPrefab;
                break;
            case Chips.GREEN:
                prefab = greenChipPrefab;
                break;
            case Chips.PURPLE:
                prefab = purpleChipPrefab;
                break;
            case Chips.RED:
                prefab = redChipPrefab;
                break;
            case Chips.YELLOW:
                prefab = yellowChipPrefab;
                break;              
        }
     
        var chip = Instantiate(prefab, SpawnPos);

        chip.GetComponent<ItemNetworkInfo>().Synchronization = ViewSynchronization.Off;
       


    }
    public void InstantiateToStackWithColor(Chips chipsCost, ref int money, string playerNick)
    {
        var chip = PhotonNetwork.Instantiate(ChipUtils.Instance.GetPathToChip(chipsCost), SpawnPos.position, SpawnPos.rotation);
        chip.GetComponent<ItemNetworkInfo>().Owner = playerNick;
        money -= (int)chipsCost;
    }
      
}
