using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneWheelPointer : MonoBehaviour
{
    public FortuneWheelSector LastSector { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        var sector = IsSectorTriggered(other);
        if (sector != null)
        {
            LastSector = sector;
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        var sector = IsSectorTriggered(other);
        if (sector == LastSector)
        {
            LastSector = null;
        }
    }*/

    private FortuneWheelSector IsSectorTriggered(Collider other)
    {
        return other.GetComponent<FortuneWheelSector>();
    }
}
