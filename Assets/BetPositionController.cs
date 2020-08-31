using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
public class BetPositionController : MonoBehaviour
{

    public Vector3 StartPos;
    public Vector3 StartBettingPos;
    public Vector3 CurrentBettingPos;
    public RoulettedBettingField currentField;
    private Rigidbody rb;
    bool coroutineStarted;

    private const float WaintSec = 2f;
    private const int reapitNum = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {

       
        var currentField = other.gameObject.GetComponent<RoulettedBettingField>();
        if (currentField != null)
        {
           
            Debug.Log(currentField.name);
            StopAllCoroutines();
            this.currentField = currentField;
            StartBettingPos = transform.position;
            coroutineStarted = true;
            StartCoroutine(CheckBetPos());
        }
      
        
    }
  


    IEnumerator CheckBetPos()
    {

        for (var i = 0; i < reapitNum; i++)
        {
            Debug.Log("reapitNum" + i);
            yield return new WaitForSeconds(WaintSec);
            CurrentBettingPos = transform.position;
           
            Debug.Log(StartBettingPos);
            Debug.Log(CurrentBettingPos);
            if (IsEqualPoses(StartBettingPos, CurrentBettingPos))
            {
               
                List<ChipData> chips = GetAllChips(GetComponent<PlayerBettingChipsField>());
                
                foreach (ChipData chip in chips)
                {
                    chip.gameObject.transform.parent = null;
                    //chip.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    currentField.MagnetizeObject(chip.gameObject, currentField.Stacks[0], "betStack");
                                       
                }
                GetComponent<PlayerBettingChipsField>().Stacks[0].Objects.Clear();
                transform.position = StartPos;
                break;
            }
            StartBettingPos = CurrentBettingPos;
        }
        coroutineStarted = false;
    }

    bool IsEqualPoses(Vector3 po1, Vector3 po2)
    {
        return Math.Round(po1.x, 3) == Math.Round(po2.x, 3) && Math.Round(po1.y, 3) == Math.Round(po2.y, 3) && Math.Round(po1.z, 3) == Math.Round(po2.z, 3);
    }

    List<ChipData> GetChipsFromStack(Transform stack)
    {
        List<ChipData> chips = new List<ChipData>();
        chips.AddRange(stack.GetComponentsInChildren<ChipData>().ToList());

        foreach (Transform chip in stack)
        {
            chip.transform.parent = null;
        }

        stack.GetComponent<StackData>().Objects.Clear();
        return chips;
    }

    List<ChipData> GetAllChips(PlayerBettingChipsField pf)
    {
        List<ChipData> chips = new List<ChipData>();

        chips.AddRange(GetChipsFromStack(pf.Stacks[0].transform));
        

        return chips;
    }

 }
