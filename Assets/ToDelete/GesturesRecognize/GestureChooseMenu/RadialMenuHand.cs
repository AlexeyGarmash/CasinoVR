using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadialMenuHand : MonoBehaviour
{
    public Action<RadialSector> RadialSectorSelected;
    [SerializeField] private RadialMenu _radialMenu;


    private Animator animator;

    private RadialSector currentDownSector = null;
    private bool stickUp = false;

    private bool menuShown = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        menuShown = false;
    }

    private bool keyPressed = false;
    private void ResolveShowMenu(bool show)
    {
        if (show && !menuShown)
        {
            animator.SetBool("Show", show);
            menuShown = true;
        }
        if (!show && menuShown)
        {
            animator.SetBool("Show", show);
            menuShown = false;
        }
    }

    private bool menuInvoke = false;
    public void InvokeMenu()
    {
        menuInvoke = true;
    }

    public void RevokeMenu()
    {
        menuInvoke = false;
    }

    private void Update()
    {
        if (menuInvoke)
        {
            if (!keyPressed)
            {
                ResolveShowMenu(true); 
                keyPressed = true;
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight))
            {
                stickUp = false;
                ResolveSelectSector(RadialSector.RadialMenuSector.RIGHT);
                print("RIGHT CLICK");
            }
             if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft))
            {
                stickUp = false;
                ResolveSelectSector(RadialSector.RadialMenuSector.LEFT);
                print("LEFT CLICK");
            }
             if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp))
            {
                stickUp = false;
                ResolveSelectSector(RadialSector.RadialMenuSector.UP);
                print("UP CLICK");
            }
             if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown))
            {
                stickUp = false;
                ResolveSelectSector(RadialSector.RadialMenuSector.DOWN);
                print("DOWN CLICK");
            }

             if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickRight))
            {
                stickUp = true;
                ResolveSelectSector(RadialSector.RadialMenuSector.RIGHT);
            }
             if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickLeft))
            {
                stickUp = true;
                ResolveSelectSector(RadialSector.RadialMenuSector.LEFT);
            }
             if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickUp))
            {
                stickUp = true;
                ResolveSelectSector(RadialSector.RadialMenuSector.UP);
            }
             if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstickDown))
            {
                stickUp = true;
                ResolveSelectSector(RadialSector.RadialMenuSector.DOWN);
            }
        }
        else
        {
            if (keyPressed)
            {
                ResolveShowMenu(false);
                keyPressed = false;
            }
        }

        /*if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown) ||
            OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp)||
            OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft)||
            OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
        {
            Vector2 thumb = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            print(thumb);
        }*/
    }

    private void ResolveSelectSector(RadialSector.RadialMenuSector radialMenuSector)
    {
        currentDownSector = _radialMenu[radialMenuSector];
        if (currentDownSector != null)
        {
            if (!stickUp)
            {
                currentDownSector.ResolveSelectSector(true);
                RadialSectorSelected.Invoke(currentDownSector);
            }
            else
            {
                currentDownSector.ResolveSelectSector(false);
            }
        }
    }

    
}
