using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    public event Action<WheelCellData> OnBallInCell;

    private void OnTriggerEnter(Collider other)
    {
        WheelCellTrigger wheelCellTrigger = other.gameObject.GetComponent<WheelCellTrigger>();
        if (wheelCellTrigger != null)
        {
            OnBallInCell.Invoke(wheelCellTrigger.WheelCellData);
        }
    }
}
