using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Chips { YELLOW = 1, RED = 5, BLUE = 10, GREEN = 25, BLACK = 100, PURPLE = 250}


public class PlayerInfo : MonoBehaviour
{
    PlayerStats ps;

    public Transform yellowSpawnPos;
    public Transform redSpawnPos;
    public Transform blueSpawnPos;
    public Transform greenSpawnPos;
    public Transform blackSpawnPos;
    public Transform purpleSpawnPos;

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

    private void Start()
    {
        yellowChipPrefab = ChipsUtils.Instance.yellowChipPrefab;
        redChipPrefab = ChipsUtils.Instance.redChipPrefab;
        blueChipPrefab = ChipsUtils.Instance.blueChipPrefab;
        greenChipPrefab = ChipsUtils.Instance.greenChipPrefab;
        blackChipPrefab = ChipsUtils.Instance.blackChipPrefab;
        purpleChipPrefab = ChipsUtils.Instance.purpleChipPrefab;

       
    }

    void ReloadYPos()
    {
        yPosYellow = 0;
        yPosRed = 0;
        yPosBlue = 0;
        yPosGreen = 0;
        yPosBlack = 0;
        yPosPurple = 0;
    }

    public void InstantiateStackOfChips()
    {
        if (ps.AllMoney > 0)
        {
            ReloadYPos();

            var starmoney = ps.AllMoney;

            while (ps.AllMoney > 0)
            {
                if (starmoney / 2 < ps.AllMoney)
                {
                    InstantiatePrefab(purpleChipPrefab, purpleSpawnPos, ref yPosPurple, (int)Chips.PURPLE, ref ps.AllMoney);

                }
                else if (starmoney / 4 < ps.AllMoney)
                {
                    InstantiatePrefab(blackChipPrefab, blackSpawnPos, ref yPosBlack, (int)Chips.BLACK, ref ps.AllMoney);
                }
                else if (starmoney / 8 < ps.AllMoney)
                {
                    InstantiatePrefab(greenChipPrefab, greenSpawnPos, ref yPosGreen, (int)Chips.GREEN, ref ps.AllMoney);
                }
                else if (starmoney / 16 < ps.AllMoney)
                {
                    InstantiatePrefab(blueChipPrefab, blueSpawnPos, ref yPosBlue, (int)Chips.BLUE, ref ps.AllMoney);
                }
                else if (starmoney / 32 < ps.AllMoney)
                {
                    InstantiatePrefab(redChipPrefab, redSpawnPos, ref yPosRed, (int)Chips.RED, ref ps.AllMoney);
                }
                else
                    InstantiatePrefab(yellowChipPrefab, yellowSpawnPos, ref yPosYellow, (int)Chips.YELLOW, ref ps.AllMoney);
            }
        }
    }
    private void InstantiatePrefab(GameObject chip, Transform spawn, ref float yPos, int cost, ref int money)
    {
        var createdChip = Instantiate(chip, new Vector3(spawn.position.x, spawn.position.y + yPos, spawn.position.z), new Quaternion(0, 0, 0, 0));
        yPos += ChipsUtils.Instance.yOffset;
        createdChip.transform.parent = spawn;
        createdChip.transform.rotation = new Quaternion(0, 0, 0, 0);
        money -= cost;
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            

            if (OVRInput.Get(OVRInput.Button.Three))
            {
                if (ps == null)
                {
                    ps = other.gameObject.GetComponent<PlayerStats>();
                    InstantiateStackOfChips();
                }
            }
        }
    }

}
