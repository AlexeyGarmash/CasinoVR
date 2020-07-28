using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StackAnimator : MonoBehaviour
{
    
    [SerializeField]
    public const float chipsDropSpeed = 0.4f;

    [SerializeField]
    public const float chipsDropMult = 1.1f;

    [SerializeField]
    public const float pauseBeetwenChips = 0.1f;
    public float currentY;
    public float lastY;

    public float xOffset = 0.004f;
    public float zOffset = 0.004f;

    public float yOffset = 0.0073f;



    public bool IsMoving = false;

    public StackData stack;

    private void Start()
    {
        stack = GetComponent<StackData>();
    }
    IEnumerator MoveLastChips(float chipsDropSpeed, float chipsDropMult, float pause, List<GameObject> Objects)
    {
        IsMoving = true;
        currentY = lastY;

        //for (var i = 0; i < currentObjects.Count; i++)
        //   currentObjects[i].GetComponent<Collider>().enabled = false;

        for (var i = 0; i < currentObjects.Count; i++)
        {

            stack.StartCoroutine(
                    MoveChip(chipsDropSpeed, chipsDropMult, currentObjects[i])
                );
            yield return new WaitForSeconds(pause);

        }
        //for (var i = 0; i < currentObjects.Count; i++)
        //    currentObjects[i].GetComponent<Collider>().enabled = true;

        lastY = currentY;
        IsMoving = false;
        currentObjects.Clear();
    }
    List<GameObject> currentObjects = new List<GameObject>();

    IEnumerator WaitForLastChips(GameObject chip)
    {
        chip.SetActive(false);
        currentObjects.Add(chip);

        yield return new WaitForSeconds(0.2f);

        while (!IsMoving)
        {
            UpdateStackWithAnim();
        }


    }
    public void StartAnim(GameObject chip)
    {
        StopAllCoroutines();
        StartCoroutine(
                WaitForLastChips(chip)
            );
    }


    IEnumerator MoveChip(float chipsDropSpeed, float chipsDropMult, GameObject chip)
    {


        chip.SetActive(true);


        var currOffsetX = Random.Range(-xOffset, xOffset);
        var currOffsetZ = Random.Range(-zOffset, zOffset);

        var pos = gameObject.transform.position;

        chip.transform.rotation = new Quaternion();


        var Start = new Vector3(
                    pos.x + currOffsetX,
                    transform.position.y + currentY,
                    pos.z + currOffsetZ);
        var End = new Vector3(
                    pos.x + currOffsetX,
                    transform.position.y + currentY + 0.2f,
                     pos.z + currOffsetZ);

        var t = chipsDropSpeed * Time.deltaTime;


        chip.transform.position = End;
        currentY += yOffset;

        while (Start != chip.transform.position)
        {
            yield return null;
            chipsDropSpeed *= chipsDropMult;


            chip.transform.position = Vector3.Lerp(End, Start, t);
            t += chipsDropSpeed * Time.deltaTime;
        }


        yield return null;


    }




    private void UpdateStackWithAnim()
    {

        StopAllCoroutines();
        StartCoroutine(MoveLastChips(chipsDropSpeed, chipsDropMult, pauseBeetwenChips, currentObjects));
    }

    public void UpdateStackInstantly()
    {
        StopAllCoroutines();


        currentY = 0;
        lastY = 0;

        for (var i = 0; i < stack.Objects.Count; i++)
        {

            var currOffsetX = Random.Range(-xOffset, xOffset);
            var currOffsetZ = Random.Range(-zOffset, zOffset);

            var pos = gameObject.transform.position;

            stack.Objects[i].transform.rotation = new Quaternion();
            stack.Objects[i].transform.position = new Vector3(
                        pos.x + currOffsetX,
                        transform.position.y + currentY,
                        pos.z + currOffsetZ
             );

            currentY += yOffset;
        }

        lastY = currentY;

    }

    public void Clear()
    {
        currentY = 0;
        lastY = 0;
    }
}

