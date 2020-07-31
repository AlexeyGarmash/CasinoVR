using OVR.OpenVR;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private PhotonView view;

    private void Start()
    {
        SetPrefabs();
        view = GetComponent<PhotonView>();
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
    private void InstantiateChip_RPC(byte[] prefab, int chipCost, int money)
    {
        var convertedPrefab = prefab.FromByteArray() as GameObject;  

        Instantiate(convertedPrefab, SpawnPos);
        money -= chipCost;
    }
    public void InstantiateToStackWithColor(Chips chipsCost, ref int money)
    {
             
        switch (chipsCost)
        {
            case Chips.BLACK:

                view.RPC("InstantiateChip_RPC", RpcTarget.All, blackChipPrefab.ToByteArray(), (int)chipsCost);
                
                break;
            case Chips.BLUE:
                view.RPC("InstantiateChip_RPC", RpcTarget.All, blueChipPrefab.ToByteArray(), (int)chipsCost);
                break;
            case Chips.GREEN:
                view.RPC("InstantiateChip_RPC", RpcTarget.All, greenChipPrefab.ToByteArray(), (int)chipsCost);
                break;
            case Chips.PURPLE:
                view.RPC("InstantiateChip_RPC", RpcTarget.All, purpleChipPrefab.ToByteArray(), (int)chipsCost);
                break;
            case Chips.RED:
                view.RPC("InstantiateChip_RPC", RpcTarget.All, redChipPrefab.ToByteArray(), (int)chipsCost);
                break;
            case Chips.YELLOW:
                view.RPC("InstantiateChip_RPC", RpcTarget.All, yellowChipPrefab.ToByteArray(), (int)chipsCost);
                break;
        }       
       
    }
      
}
