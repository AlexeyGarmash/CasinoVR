using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStackField : MonoBehaviour
{
    GameObject yellowChipPrefab;
    GameObject redChipPrefab;
    GameObject blueChipPrefab;
    GameObject greenChipPrefab;
    GameObject blackChipPrefab;
    GameObject purpleChipPrefab;

    PlayerStackData yellowStack;
    PlayerStackData redStack;
    PlayerStackData greenStack;
    PlayerStackData blackStack;
    PlayerStackData purpleStack;
    PlayerStackData blueStack;

    private void Start()
    {
        InitStacksWithColor();
        SetPrefabs();
    }
    private void SetPrefabs()
    {
        yellowChipPrefab = ChipsUtils.Instance.yellowChipPrefab;
        redChipPrefab = ChipsUtils.Instance.redChipPrefab;
        blueChipPrefab = ChipsUtils.Instance.blueChipPrefab;
        greenChipPrefab = ChipsUtils.Instance.greenChipPrefab;
        blackChipPrefab = ChipsUtils.Instance.blackChipPrefab;
        purpleChipPrefab = ChipsUtils.Instance.purpleChipPrefab;
    }
    private void OnTriggerEnter(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        if (chip != null)
        {
            switch (chip.Cost)
            {
                case Chips.BLACK:                   
                    
                    ChipsUtils.Instance.MagnetizeChip(gameObj, blackStack);
                    break;
                case Chips.BLUE:
                    
                    ChipsUtils.Instance.MagnetizeChip(gameObj, blueStack);
                   
                    break;
                case Chips.GREEN:
                    ChipsUtils.Instance.MagnetizeChip(gameObj, greenStack);
                    break;
                case Chips.PURPLE:
                    ChipsUtils.Instance.MagnetizeChip(gameObj, purpleStack);
                    break;
                case Chips.RED:
                    ChipsUtils.Instance.MagnetizeChip(gameObj, redStack);
                    break;
                case Chips.YELLOW:
                    ChipsUtils.Instance.MagnetizeChip(gameObj, yellowStack);
                    break;
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {

        var chip = other.gameObject.GetComponent<ChipData>();
        if (chip != null)
        {
            switch (chip.Cost)
            {
                case Chips.BLACK:
                    ChipsUtils.Instance.ExtractionChip(other.gameObject, blackStack);

                    break;
                case Chips.BLUE:
                    ChipsUtils.Instance.ExtractionChip(other.gameObject, blueStack);

                    break;
                case Chips.GREEN:
                    ChipsUtils.Instance.ExtractionChip(other.gameObject, greenStack);
                    break;
                case Chips.PURPLE:
                    ChipsUtils.Instance.ExtractionChip(other.gameObject, purpleStack);
                    break;
                case Chips.RED:
                    ChipsUtils.Instance.ExtractionChip(other.gameObject, redStack);
                    break;
                case Chips.YELLOW:
                    ChipsUtils.Instance.ExtractionChip(other.gameObject, yellowStack);
                    break;
            }


        }

    }

    private void InitStacksWithColor()
    {
        var StacksWithColor = GetComponentsInChildren<PlayerStackData>();

        for (var i = 0; i < StacksWithColor.Length; i++)
        {
            switch (StacksWithColor[i].StackOfChipColor)
            {
                case Chips.BLACK:
                    blackStack = StacksWithColor[i];
                    break;
                case Chips.BLUE:
                    blueStack = StacksWithColor[i];
                    break;
                case Chips.GREEN:
                    greenStack = StacksWithColor[i];
                    break;
                case Chips.PURPLE:
                    purpleStack = StacksWithColor[i];
                    break;
                case Chips.RED:
                    redStack = StacksWithColor[i];
                    break;
                case Chips.YELLOW:
                    yellowStack = StacksWithColor[i];
                    break;
            }
        }
    }
    
    public void InstantiateToStackWithColor(Chips chipsCost, ref int money)
    {
        switch (chipsCost)
        {
            case Chips.BLACK:
                blackStack.InstantiatePrefab(blackChipPrefab, chipsCost, ref money);
                break;
            case Chips.BLUE:
                blueStack.InstantiatePrefab(blueChipPrefab, chipsCost, ref money);
                break;
            case Chips.GREEN:
                greenStack.InstantiatePrefab(greenChipPrefab, chipsCost, ref money);
                break;
            case Chips.PURPLE:
                purpleStack.InstantiatePrefab(purpleChipPrefab, chipsCost, ref money);
                break;
            case Chips.RED:
                redStack.InstantiatePrefab(redChipPrefab, chipsCost, ref money);
                break;
            case Chips.YELLOW:
                yellowStack.InstantiatePrefab(yellowChipPrefab, chipsCost, ref money);
                break;
        }
    }
    public void Clear()
    {
        yellowStack.ClearData();
        blackStack.ClearData();
        redStack.ClearData();
        greenStack.ClearData();
        blueStack.ClearData();
        purpleStack.ClearData();
    }
}
