
using UnityEngine;

public enum Chips { YELLOW = 1, RED = 5, BLUE = 10, GREEN = 25, BLACK = 100, PURPLE = 250 }
class StackUtils : Singleton<StackUtils>
{
    public GameObject yellowChipPrefab;
    public GameObject redChipPrefab;
    public GameObject blueChipPrefab;
    public GameObject greenChipPrefab;
    public GameObject blackChipPrefab;
    public GameObject purpleChipPrefab;

   
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
   
          
    public bool MagnetizeObject(GameObject chip, string playerName, float yOffset, params StackData[] stacks)
    {
        var rb = chip.GetComponent<Rigidbody>();

        for (int i = 0;  i < stacks.GetLength(0); i++)
        {

            var stackData = stacks[i];
            var transform = stackData.gameObject.transform;
            if (stackData.playerName.Equals(playerName) || stackData.playerName == "")
            {
                if (stackData.playerName == "")
                    stackData.playerName = playerName;

                if (rb.isKinematic != true)
                {
                                                  
                    rb.isKinematic = true;

                    var currOffsetX = Random.Range(-xOffset, xOffset);
                    var currOffsetZ = Random.Range(-zOffset, zOffset);

                    chip.transform.position = new Vector3(transform.position.x + currOffsetX, transform.position.y + stackData.currentY, transform.position.z + currOffsetZ);
                    chip.transform.rotation = transform.rotation;
                    stackData.currentY += yOffset;
                    chip.transform.parent = stacks[i].transform;
                    stackData.Objects.Add(chip);

                   
                }
                UpdateStack(stacks[i], yOffset);
                return true;
            }               
        }

        return false;
    }

    public bool ExtractionObject(GameObject chip, float yOffset, params StackData[] stacks)
    {
        for (var i = 0; i < stacks.Length; i++)
        {
            if (stacks[i].Objects.Contains(chip))
            {
                stacks[i].Objects.Remove(chip);
                UpdateStack(stacks[i], yOffset);
                return true;
            }
        }
        return false;
    }

    private void UpdateStack(StackData stack, float yOffset)
    {
       
        stack.currentY = 0;
        for (var i = 0; i < stack.Objects.Count; i++)
        {
            var currOffsetX = Random.Range(-xOffset, xOffset);
            var currOffsetZ = Random.Range(-zOffset, zOffset);

            var pos = stack.gameObject.transform.position;

            stack.Objects[i].transform.position = new Vector3(
                pos.x + currOffsetX,
                stack.transform.position.y + stack.currentY,
                pos.z + currOffsetZ
            );
            stack.Objects[i].transform.rotation = stack.gameObject.transform.rotation;
            stack.currentY += yOffset;
        }
    }


 }

