using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToStack : MonoBehaviour
{
   
    [SerializeField]
    private float timeToReturn = 3f;

    private OVRGrabbableCustom grabbableObject;
    private Rigidbody rb;
    private ChipData chipData;
    [SerializeField]
    private PlayerChipsField field;
    void Awake()
    {       

        field = GetComponentInParent<PlayerChipsField>();
        grabbableObject = GetComponent<OVRGrabbableCustom>();
        rb = GetComponent<Rigidbody>();
        chipData = GetComponent<ChipData>();
    }

    private void Start()
    {
        field = GetComponentInParent<PlayerChipsField>();
    }

    float currentTime = 0;
    void Update()
    {
        if (!rb.isKinematic && !grabbableObject.isGrabbed)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= timeToReturn)
            {
                field.MagnetizeObject(gameObject, field.StacksByChipCost[chipData.Cost].GetComponent<StackData>());
              
                currentTime = 0;
            }
        }

    }

    
}
