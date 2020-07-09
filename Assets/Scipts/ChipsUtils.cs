using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class ChipsUtils : Singleton<ChipsUtils>
{
    GameObject yellowChipPrefab;
    GameObject redChipPrefab;
    GameObject blueChipPrefab;
    GameObject greenChipPrefab;
    GameObject blackChipPrefab;
    GameObject purpleChipPrefab;

    private float yPosYellow;
    private float yPosRed;
    private float yPosBlue;
    private float yPosGreen;
    private float yPosBlack;
    private float yPosPurple;

    int money;

    const float yOffset = 0.009f;
    void ReloadYPos()
    {
        yPosYellow = 0;
        yPosRed = 0;
        yPosBlue = 0;
        yPosGreen = 0;
        yPosBlack = 0;
        yPosPurple = 0;
    }
    private void Awake()
    {
        yellowChipPrefab = Resources.Load("Chips/Casino_Chip_Y") as GameObject;
        redChipPrefab = Resources.Load("Chips/Casino_Chip_R") as GameObject;
        blueChipPrefab = Resources.Load("Chips/Casino_Chip_Blue") as GameObject;
        greenChipPrefab = Resources.Load("Chips/Casino_Chip_G") as GameObject;
        blackChipPrefab = Resources.Load("Chips/Casino_Chip_Black") as GameObject;
        purpleChipPrefab = Resources.Load("Chips/Casino_Chip_P") as GameObject;
    }
   
        
    public void InstantiateStackOfChips(int money, Transform yellowSpawnPos, Transform redSpawnPos,
        Transform blueSpawnPos, Transform greenSpawnPos, Transform blackSpawnPos, Transform purpleSpawnPos)
    {
        if (money > 0)
        {
            ReloadYPos();

            var starmoney = money;

            while (money > 0)
            {
                if (starmoney / 2 < money)
                {
                    InstantiatePrefab(purpleChipPrefab, purpleSpawnPos, ref yPosPurple, (int)Chips.PURPLE, ref money);
                    
                }
                else if (starmoney / 4 < money)
                {
                    InstantiatePrefab(blackChipPrefab, blackSpawnPos, ref yPosBlack, (int)Chips.BLACK, ref money);
                }
                else if (starmoney / 8 < money)
                {
                    InstantiatePrefab(greenChipPrefab, greenSpawnPos, ref yPosGreen, (int)Chips.GREEN, ref money);
                }
                else if (starmoney / 16 < money)
                {
                    InstantiatePrefab(blueChipPrefab, blueSpawnPos, ref yPosBlue, (int)Chips.BLUE, ref money);
                }
                else if (starmoney / 32 < money)
                {
                    InstantiatePrefab(redChipPrefab, redSpawnPos, ref yPosRed, (int)Chips.RED, ref money);
                }
                else
                    InstantiatePrefab(yellowChipPrefab, yellowSpawnPos, ref yPosYellow, (int)Chips.YELLOW, ref money);
            }
        }
    }
    private void InstantiatePrefab(GameObject chip, Transform spawn, ref float yPos, int cost, ref int money)
    {
        var createdChip = Instantiate(chip, new Vector3(spawn.position.x, spawn.position.y + yPos, spawn.position.z), new Quaternion(0,0,0,0));
        yPos+= yOffset;
        createdChip.transform.parent = spawn;
        createdChip.transform.rotation = new Quaternion(0, 0, 0, 0);
        money -= cost;
    }
    public void MagnetizeChip(GameObject chip, StackData[] stacks)
    {
        
        var chipData = chip.GetComponent<ChipData>();

        if (chipData != null)
        {
            var rb = chip.GetComponent<Rigidbody>();

            for (int i = 0;  i < stacks.GetLength(0); i++)
            {


                var stackData = stacks[i];
                var transform = stackData.gameObject.transform;
                if (stackData.playerName == chipData.player)
                {
                    if (rb.isKinematic != true)
                    {
                        rb.isKinematic = true;
                        chip.transform.position = new Vector3(transform.position.x, transform.position.y + stackData.currentY, transform.position.z);
                        chip.transform.rotation = transform.rotation;
                        stackData.currentY += yOffset;
                        return;
                    }
                }
            }


        }
    }
    
 }

