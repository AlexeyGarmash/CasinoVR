using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuHandV2 : MonoBehaviour
{
    public Action<RadialSectorV2> OnRadialSectorChangeListener { get; set; }
    [SerializeField] private RadialMenuV2 radialMenu;

    public RadialSectorV2 currentChosenSector = null;
    private bool stickRelease = true;
    private bool menuShown = false;
    private bool keyPressed = false;
    private bool menuInvoke = false;

    

    private void Start()
    {
        menuShown = false;
    }

    private void ResolveShowMenu(bool show)
    {
        if(show && !menuShown)
        {
            radialMenu.gameObject.SetActive(true);
            menuShown = true;
        }
        if(!show && menuShown)
        {
            radialMenu.gameObject.SetActive(false);
            menuShown = false;
        }
    }


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
        if(menuInvoke)
        {
            if(!keyPressed)
            {
                ResolveShowMenu(true);
                keyPressed = true;
            }
            /*OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft) ||
            OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight)*/
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != Vector2.zero)
            {
                Vector2 thumb = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                if(thumb.x >= 0.7 && thumb.x <= 0.9 && thumb.y >= -0.5 && thumb.y <= 0.5)
                {
                    //print("RIGHT");
                    stickRelease = false;
                    ResolveShowSelect(RadialMenuSectorV2.RIGHT);
                }
                if(thumb.x >= 0 && thumb.x <= 0.866 && thumb.y >= 0.5 && thumb.y <= 1)
                {
                    //print("TOP RIGHT");
                    stickRelease = false;
                    ResolveShowSelect(RadialMenuSectorV2.TOP_RIGHT);
                }
                if (thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= 0.5 && thumb.y <= 1)
                {
                    //print("TOP LEFT");
                    stickRelease = false;
                    ResolveShowSelect(RadialMenuSectorV2.TOP_LEFT);
                }
                if (thumb.x >= -0.9 && thumb.x <= -0.7 && thumb.y >= -0.5 && thumb.y <= 0.5)
                {
                    //print("LEFT");
                    stickRelease = false;
                    ResolveShowSelect(RadialMenuSectorV2.LEFT);
                }
                if (thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= -1 && thumb.y <= -0.5)
                {
                    //print("BOTTOM LEFT");
                    stickRelease = false;
                    ResolveShowSelect(RadialMenuSectorV2.BOTTOM_LEFT);
                }
                if (thumb.x >= 0 && thumb.x <= 0.866 && thumb.y >= -1 && thumb.y <= -0.5)
                {
                    //print("BOTTOM RIGHT");
                    stickRelease = false;
                    ResolveShowSelect(RadialMenuSectorV2.BOTTOM_RIGHT);
                }
                //print(thumb);
            }
            if(!OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown) &&
            !OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp) &&
            !OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft) &&
            !OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
            {
                stickRelease = true;
                ResolveSelectSelect();
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
    }

    private void ResolveShowSelect(RadialMenuSectorV2 radialMenuSectorV2Type)
    {
        if (currentChosenSector != null )
        {
            if (currentChosenSector.RadialSectorType != radialMenuSectorV2Type)
            {
                currentChosenSector.ResolveSelectSector(false);
                currentChosenSector = radialMenu[radialMenuSectorV2Type];
                currentChosenSector.ResolveSelectSector(true);
            }
        }
        else
        {
            currentChosenSector = radialMenu[radialMenuSectorV2Type];
            currentChosenSector.ResolveSelectSector(true);
        }
    }

    private void ResolveSelectSelect()
    {
        if (currentChosenSector != null && stickRelease)
        {
            currentChosenSector.ResolveSelectSector(false);
            //OnRadialSectorChangeListener.Invoke(currentChosenSector);
            print(string.Format("Invoked {0}!!!", currentChosenSector.name));
            currentChosenSector = null;
            
        }
    }

}
