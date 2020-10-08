using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineInit : MonoBehaviour
{
   
    void Start()
    {
        GetComponent<Outline>().enabled = false;
    }

}
