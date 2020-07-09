using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingField : MonoBehaviour
{

   
    [SerializeField]
    StackData[] BetStacks;
    



    private void OnTriggerEnter(Collider other)
    {
        ChipsUtils.Instance.MagnetizeChip(other.gameObject, BetStacks);
    }
}
