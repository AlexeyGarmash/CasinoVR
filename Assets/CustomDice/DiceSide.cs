using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    [SerializeField] private int _diceValue;

    private bool _onGround;



    public int DiceValue { get => _diceValue; }
    public bool OnGround { get => _onGround; }

    private void OnTriggerStay(Collider other)
    {
        if(DetectGround(other))
        {
            _onGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (DetectGround(other))
        {
            _onGround = false;
        }
    }

    private bool DetectGround(Collider other)
    {
        return other.gameObject.CompareTag("Ground");
    }
}
