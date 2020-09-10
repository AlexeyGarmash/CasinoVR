using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnimatorController : MonoBehaviour
{

    private Animator animator;
    private Vector3 previousPos;
    private VRRig vrRig;
    public float speedTreshold = 0.1f;
   
    [Range(0,1)]
    public float smoothing = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        vrRig = GetComponent<VRRig>();
        previousPos = vrRig.head.vrTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Compute speed
        Vector3 headsetSpeed = (vrRig.head.vrTarget.position - previousPos) / Time.deltaTime;
        headsetSpeed.y = 0;

        //local speed
        Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);
        previousPos = vrRig.head.vrTarget.position;

        //set animator value
        float previousDirectionX = animator.GetFloat("DiractionX");
        float previousDirectionY = animator.GetFloat("DiractionY");

        animator.SetBool("IsMoving", headsetLocalSpeed.magnitude > speedTreshold);
        animator.SetFloat("DiractionX", Mathf.Lerp(previousDirectionX, Mathf.Clamp(headsetLocalSpeed.x, -1, 1), smoothing));
        animator.SetFloat("DiractionY", Mathf.Lerp(previousDirectionY, Mathf.Clamp(headsetLocalSpeed.z, -1, 1), smoothing));
    }
}
