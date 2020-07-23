using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    public event Action<WheelCellData> OnBallInCell;
    public int winingNumber;
    private void OnTriggerEnter(Collider other)
    {
        WheelCellTrigger wheelCellTrigger = other.gameObject.GetComponent<WheelCellTrigger>();
        if (wheelCellTrigger != null)
        {
            if (winingNumber == wheelCellTrigger.WheelCellData.Number)
            {
                transform.position = other.transform.position;
                transform.parent = other.transform.parent.transform;
            }
            OnBallInCell.Invoke(wheelCellTrigger.WheelCellData);
        }
        
    }
}
