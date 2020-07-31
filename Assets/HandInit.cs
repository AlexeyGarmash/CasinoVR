using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInit : MonoBehaviour
{
    // Start is called before the first frame update
    SkinnedMeshRenderer smr;
    void Start()
    {
        smr =  GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!smr.enabled)
            GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    
}
