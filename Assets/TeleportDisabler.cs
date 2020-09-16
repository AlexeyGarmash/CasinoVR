using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDisabler : MonoBehaviour
{
    [SerializeField] GameObject teleport;

    public void Disable()
    {
        teleport.SetActive(false);
    }
    public void Enable()
    {
        teleport.SetActive(true);
    }
}
