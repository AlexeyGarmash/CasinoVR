
using UnityEngine;


class ChipsUtils : Singleton<ChipsUtils>
{
    public GameObject yellowChipPrefab;
    public GameObject redChipPrefab;
    public GameObject blueChipPrefab;
    public GameObject greenChipPrefab;
    public GameObject blackChipPrefab;
    public GameObject purpleChipPrefab;

    public float yOffset = 0.0073f;
    private float xOffset = 0.004f;
    private float zOffset = 0.004f;

    private void Awake()
    {
        yellowChipPrefab = Resources.Load("Chips/Casino_Chip_Y") as GameObject;
        redChipPrefab = Resources.Load("Chips/Casino_Chip_R") as GameObject;
        blueChipPrefab = Resources.Load("Chips/Casino_Chip_Blue") as GameObject;
        greenChipPrefab = Resources.Load("Chips/Casino_Chip_G") as GameObject;
        blackChipPrefab = Resources.Load("Chips/Casino_Chip_Black") as GameObject;
        purpleChipPrefab = Resources.Load("Chips/Casino_Chip_P") as GameObject;
    }
   
          
    public bool MagnetizeChip(GameObject chip, StackData[] stacks)
    {
        
        var chipData = chip.GetComponent<ChipData>();

        if (chipData != null)
        {
            var rb = chip.GetComponent<Rigidbody>();

            for (int i = 0;  i < stacks.GetLength(0); i++)
            {

                var stackData = stacks[i];
                var transform = stackData.gameObject.transform;
                if (stackData.playerName.Equals(chipData.player) || stackData.playerName == "")
                {
                    if (stackData.playerName == "")
                        stackData.playerName = chipData.player;

                    if (rb.isKinematic != true)
                    {
                        if (stacks[i].Chips.Count == 0)
                            stacks[i].startY = transform.position.y;
                        rb.isKinematic = true;

                        var currOffsetX = Random.Range(-xOffset, xOffset);
                        var currOffsetZ = Random.Range(-zOffset, zOffset);

                        chip.transform.position = new Vector3(transform.position.x + currOffsetX, transform.position.y + stackData.currentY, transform.position.z + currOffsetZ);
                        chip.transform.rotation = transform.rotation;
                        stackData.currentY += yOffset;

                        stackData.Chips.Add(chip);


                        return true;
                    }
                }               
            }


        }
        return false;
    }

    public bool ExtractionChip(GameObject chip, StackData[] stacks)
    {
        for (var i = 0; i < stacks.Length; i++)
        {
            if (stacks[i].Chips.Contains(chip) && chip.GetComponent<OVRGrabbable>().grabbedBy != null)
            {
                stacks[i].Chips.Remove(chip);
                UpdateStack(stacks[i]);
                return true;
            }
        }
        return false;
    }

    public void UpdateStack(StackData stack)
    {
       
        stack.currentY = 0;
        for (var i = 0; i < stack.Chips.Count; i++)
        {
            var currOffsetX = Random.Range(-xOffset, xOffset);
            var currOffsetZ = Random.Range(-zOffset, zOffset);

            var pos = stack.gameObject.transform.position;

            stack.Chips[i].transform.position = new Vector3(
                pos.x + currOffsetX,
                stack.startY + stack.currentY,
                pos.z + currOffsetZ
            );
            stack.Chips[i].transform.rotation = stack.gameObject.transform.rotation;
            stack.currentY += yOffset;
        }
    }


 }

