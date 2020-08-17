using System;
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
    [SerializeField] private float ZoomSpeed;
    [SerializeField] private RadialMenuHand radialMenuHand;


    private Vector3 startCanvasPos;

    private float DistanceEyeCanvas = -1f;

    private void Start()
    {
        startCanvasPos = gameObject.transform.position;
        DistanceEyeCanvas = (transform.position - CenterEyeTransform.position).magnitude;
        print("Distance from center eye to canvas = " + DistanceEyeCanvas);

        radialMenuHand.RadialSectorSelected += OnRadialMenuSelected;
    }

    private void OnRadialMenuSelected(RadialSector radialSector)
    {
        print("radial sector invoke + " + radialSector.radialMenuSector);
    }

    private void Update()
    {
        
        if (OVRInput.Get(DraggableButton))
        {

            radialMenuHand.InvokeMenu();

            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
            {
                MoveCanvasFarther();
            }
            if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
            {
                MoveCanvasCloser();
            }
            if (PointerTransform != null)
            {
                startCanvasPos.x = PointerTransform.position.x;
                startCanvasPos.y = PointerTransform.position.y;
                startCanvasPos.z = PointerTransform.position.z;
                //print(startCanvasPos);
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, startCanvasPos, Time.deltaTime * MoveSpeed);
                transform.LookAt(2 * transform.position - CenterEyeTransform.position);
                //}

            }
        }
        else
        {
            radialMenuHand.RevokeMenu();
        }
    }

    private void MoveCanvasCloser()
    {
        startCanvasPos.z -= Time.deltaTime * ZoomSpeed;
        transform.position = Vector3.Lerp(transform.position, startCanvasPos, Time.deltaTime * MoveSpeed);
    }

    private void MoveCanvasFarther()
    {
        startCanvasPos.z += Time.deltaTime * ZoomSpeed;
        transform.position = Vector3.Lerp(transform.position, startCanvasPos, Time.deltaTime * MoveSpeed);
    }
}
