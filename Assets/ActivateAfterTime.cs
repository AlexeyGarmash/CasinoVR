using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivateAfterTime : MonoBehaviour
{
    [SerializeField]
    GameObject[] objects;
    [SerializeField]
    float time = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {
        Debug.Log("WaitTIme");
        yield return new WaitForSeconds(time);
        Debug.Log("WaitTIme");

        objects.ToList().ForEach(o => o.SetActive(true));

        
    }
    // Update is called once per frame
   
}
