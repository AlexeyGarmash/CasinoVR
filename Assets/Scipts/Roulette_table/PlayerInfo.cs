using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Chips { YELLOW = 1, RED = 5, BLUE = 10, GREEN = 25, BLACK = 100, PURPLE = 250}


public class PlayerInfo : MonoBehaviour
{
    public int money;

    public Transform yellowSpawnPos;
    public Transform redSpawnPos;
    public Transform blueSpawnPos;
    public Transform greenSpawnPos;
    public Transform blackSpawnPos;
    public Transform purpleSpawnPos;

    private void Start()
    {
        ChipsUtils.Instance.InstantiateStackOfChips(money, yellowSpawnPos, redSpawnPos, blueSpawnPos, greenSpawnPos, blackSpawnPos, purpleSpawnPos);       
    }

}
