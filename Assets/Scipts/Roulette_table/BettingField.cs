using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingField : MonoBehaviour
{

   
    [SerializeField]
    StackData[] BetStacks;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        ChipsUtils.Instance.MagnetizeChip(other.gameObject, BetStacks);
    }
   
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay");
        ChipsUtils.Instance.ExtractionChip(other.gameObject, BetStacks);
    }

}
