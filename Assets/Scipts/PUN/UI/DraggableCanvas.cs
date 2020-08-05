using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCanvas : MonoBehaviour
{
    [SerializeField] private Transform CenterEyeTransform;

    [SerializeField] private Transform PointerTransform;
    [SerializeField] private OVRInput.Button DraggableButton;
    [SerializeField] private float MoveSpeed;

    private Vector3 startCanvasPos;

    private float DistanceEyeCanvas = -1f;

    private void Start()
    {
        startCanvasPos = gameObject.transform.position;
        DistanceEyeCanvas = (transform.position - CenterEyeTransform.position).magnitude;
        print("Distance from center eye to canvas = " + DistanceEyeCanvas);
    }

    private void Update()
    {
        
        if (OVRInput.Get(DraggableButton))
        {
            if (PointerTransform != null)
            {
                startCanvasPos.x = PointerTransform.position.x;
                startCanvasPos.y = PointerTransform.position.y;
                startCanvasPos.z = CenterEyeTransform.position.z * DistanceEyeCanvas;
                print(startCanvasPos);
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, startCanvasPos, Time.deltaTime * MoveSpeed);
                transform.LookAt(2 * transform.position - CenterEyeTransform.position);
                //}

            }
        }
    }
}
