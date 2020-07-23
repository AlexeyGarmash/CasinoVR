using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChipsField : MonoBehaviour
{
    GameObject yellowChipPrefab;
    GameObject redChipPrefab;
    GameObject blueChipPrefab;
    GameObject greenChipPrefab;
    GameObject blackChipPrefab;
    GameObject purpleChipPrefab;

    public PlayerStackData yellowStack;
    public PlayerStackData redStack;
    public PlayerStackData greenStack;
    public PlayerStackData blackStack;
    public PlayerStackData purpleStack;
    public PlayerStackData blueStack;

    public float yOffset = 0.0073f;
    private void Start()
    {
        InitStacksWithColor();
        SetPrefabs();
    }
    private void SetPrefabs()
    {
        yellowChipPrefab = StackUtils.Instance.yellowChipPrefab;
        redChipPrefab = StackUtils.Instance.redChipPrefab;
        blueChipPrefab = StackUtils.Instance.blueChipPrefab;
        greenChipPrefab = StackUtils.Instance.greenChipPrefab;
        blackChipPrefab = StackUtils.Instance.blackChipPrefab;
        purpleChipPrefab = StackUtils.Instance.purpleChipPrefab;
    }
    private void OnTriggerEnter(Collider other)
    {

        var gameObj = other.gameObject;
        var chip = other.gameObject.GetComponent<ChipData>();
        if (chip != null && chip.transform.parent == null)
        {
            switch (chip.Cost)
            {
                case Chips.BLACK:                   
                    
                    StackUtils.Instance.MagnetizeObject(gameObj, chip.player, yOffset, blackStack);
                    chip.transform.parent = blackStack.transform;
                    break;
                case Chips.BLUE:
                    
                    StackUtils.Instance.MagnetizeObject(gameObj, chip.player, yOffset, blueStack);
                    chip.transform.parent = blueStack.transform;
                    break;
                case Chips.GREEN:
                    StackUtils.Instance.MagnetizeObject(gameObj, chip.player, yOffset, greenStack);
                    chip.transform.parent = greenStack.transform;
                    break;
                case Chips.PURPLE:
                    StackUtils.Instance.MagnetizeObject(gameObj, chip.player, yOffset, purpleStack);
                    chip.transform.parent = purpleStack.transform;
                    break;
                case Chips.RED:
                    StackUtils.Instance.MagnetizeObject(gameObj, chip.player, yOffset, redStack);
                    chip.transform.parent = redStack.transform;
                    break;
                case Chips.YELLOW:
                    StackUtils.Instance.MagnetizeObject(gameObj, chip.player, yOffset, yellowStack);
                    chip.transform.parent = yellowStack.transform;
                    break;
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {

        var chip = other.GetComponent<ChipData>();
        if (chip != null)
        {
            if (other.GetComponent<GrabbableChip>().grabbedBy != null)
            {
                switch (chip.Cost)

                {
                    case Chips.BLACK:
                        StackUtils.Instance.ExtractionObject(other.gameObject, yOffset, blackStack);

                        break;
                    case Chips.BLUE:
                        StackUtils.Instance.ExtractionObject(other.gameObject, yOffset, blueStack);

                        break;
                    case Chips.GREEN:
                        StackUtils.Instance.ExtractionObject(other.gameObject, yOffset, greenStack);
                        break;
                    case Chips.PURPLE:
                        StackUtils.Instance.ExtractionObject(other.gameObject, yOffset, purpleStack);
                        break;
                    case Chips.RED:
                        StackUtils.Instance.ExtractionObject(other.gameObject, yOffset, redStack);
                        break;
                    case Chips.YELLOW:
                        StackUtils.Instance.ExtractionObject(other.gameObject, yOffset, yellowStack);
                        break;
                }

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
        GameObject chip;
        string playerName;

        switch (chipsCost)
        {
            case Chips.BLACK:
                chip = blackStack.InstantiatePrefab(blackChipPrefab, chipsCost, ref money);
                chip.transform.parent = blackStack.transform;
                playerName = chip.GetComponent<ChipData>().player;
                StackUtils.Instance.MagnetizeObject(chip, playerName, yOffset, blackStack);
                break;
            case Chips.BLUE:
                chip = blueStack.InstantiatePrefab(blueChipPrefab, chipsCost, ref money);
                playerName = chip.GetComponent<ChipData>().player;
                chip.transform.parent = blueStack.transform;
                StackUtils.Instance.MagnetizeObject(chip, playerName, yOffset, blueStack);
                break;
            case Chips.GREEN:
                chip = greenStack.InstantiatePrefab(greenChipPrefab, chipsCost, ref money);
                playerName = chip.GetComponent<ChipData>().player;
                chip.transform.parent = greenStack.transform;
                StackUtils.Instance.MagnetizeObject(chip, playerName, yOffset, greenStack);
                break;
            case Chips.PURPLE:
                chip = purpleStack.InstantiatePrefab(purpleChipPrefab, chipsCost, ref money);
                 playerName = chip.GetComponent<ChipData>().player;
                chip.transform.parent = purpleStack.transform;
                StackUtils.Instance.MagnetizeObject(chip, playerName, yOffset, purpleStack);
                break;
            case Chips.RED:
                chip = redStack.InstantiatePrefab(redChipPrefab, chipsCost, ref money);
                playerName = chip.GetComponent<ChipData>().player;
                chip.transform.parent = redStack.transform;
                StackUtils.Instance.MagnetizeObject(chip, playerName, yOffset, redStack);
                break;
            case Chips.YELLOW:
                chip = yellowStack.InstantiatePrefab(yellowChipPrefab, chipsCost, ref money);
                playerName = chip.GetComponent<ChipData>().player;
                chip.transform.parent = yellowStack.transform;
                StackUtils.Instance.MagnetizeObject(chip, playerName, yOffset, yellowStack);
                break;
        }
    }
    private void Clear()
    {
        DestroyChips(yellowStack.transform);
        DestroyChips(blackStack.transform);
        DestroyChips(redStack.transform);
        DestroyChips(greenStack.transform);
        DestroyChips(blueStack.transform);
        DestroyChips(purpleStack.transform);

        yellowStack.ClearData();
        blackStack.ClearData();
        redStack.ClearData();
        greenStack.ClearData();
        blueStack.ClearData();
        purpleStack.ClearData();
    }

    private void DestroyChips(Transform stack)
    {
        foreach (Transform child in stack.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void UpdateFields()
    {
        Clear();
        InitStacksWithColor();
    }

    public void ClearPlace()
    {
        Clear();
       
    }
}
