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


    public void InstantiateToStackWithColor(Chips chipsCost, ref int money)
    {
       
        GameObject prefab = null;
        switch (chipsCost)
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
        if (prefab != null)
        {
             Instantiate(prefab,
                      new Vector3(
                          SpawnPos.position.x,
                          SpawnPos.position.y,
                          SpawnPos.position.z
                      ), new Quaternion(0, 0, 0, 0)
                  );
        }
        money -= (int)chipsCost;
    }
      
}
