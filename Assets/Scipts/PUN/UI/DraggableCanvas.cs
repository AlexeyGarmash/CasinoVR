using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCanvas : MonoBehaviour
{
    [SerializeField] private Transform PointerTransform;
    [SerializeField] private OVRInput.Button DraggableButton;

    private Vector3 startCanvasPos;

    private void Start()
    {
        startCanvasPos = gameObject.transform.position;
    }

    private void Update()
    {
        if (OVRInput.Get(DraggableButton))
        {
            if (PointerTransform != null)
            {
                startCanvasPos.x = PointerTransform.position.x;
                startCanvasPos.y = PointerTransform.position.y;
                gameObject.transform.position = startCanvasPos;
            }
        }
    }
}
