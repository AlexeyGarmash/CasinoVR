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

    private new void Start()
    {
        base.Start();
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


    public void InstantiateToStackWithColor(Chips chipsCost, ref int money, string playerNick)
    {
        var chip = PhotonNetwork.Instantiate(ChipUtils.Instance.GetPathToChip(chipsCost), SpawnPos.position, SpawnPos.rotation);
        chip.GetComponent<PhotonView>().RequestOwnership();
        chip.GetComponent<ChipData>().Owner = playerNick;
        chip.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();
        
        money -= (int)chipsCost;
    }


}
