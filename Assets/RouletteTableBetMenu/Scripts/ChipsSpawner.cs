using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipsSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] SpawnPoints;

    public void RecieveAndSpawnBoughtChips(List<ChooseChipGroup> groups, bool syncWithNetwork)
    {
        if(syncWithNetwork)
        {
            SpawnChipsNetwork(groups);
        }
        else
        {
            SpawnChipsLocal(groups);
        }
    }

    private void SpawnChipsLocal(List<ChooseChipGroup> groups)
    {
        for (int i = 0; i < groups.Count; i++)
        {
            var chip = groups[i];
            for (int j = 0; j < chip.CurrentChipCount; j++)
            {
                Instantiate(chip.ChipPrefab, SpawnPoints[i].position, Quaternion.identity);
            }
        }
    }

    private void SpawnChipsNetwork(List<ChooseChipGroup> groups)
    {
        for (int i = 0; i < groups.Count; i++)
        {
            var chip = groups[i];
            for (int j = 0; j < chip.CurrentChipCount; j++)
            {
                PhotonNetwork.Instantiate(chip.ChipResourceName, SpawnPoints[i].position, Quaternion.identity);
            }
        }
    }
}
