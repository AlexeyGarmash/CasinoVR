using Assets.Scipts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RadialMenuSectors { ONE = 1, TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6 }

public class RadialMenuHandV2 : MonoBehaviour
{
   

    [SerializeField] private RadialMenuV2 radialMenu;

    private List<RadialActionInfo> actions;

    public RadialSectorV2 currentChosenSector = null;
    private bool stickRelease = true;
    private bool menuShown = false;
    private bool keyPressed = false;
    private bool menuInvoke = false;

   
    public RadialMenuSectors numberOfSectors { get; set; } = RadialMenuSectors.SIX;

    private void Awake()
    {
        actions = new List<RadialActionInfo>();
    }
    private void Start()
    {
        
        menuShown = false;
    }

    public void ClearActions()
    {
        actions.Clear();
        radialMenu.ClearSectorsData();
        numberOfSectors = 0;


    }

    public void AddAction(RadialActionInfo action)
    {
        actions.Add(action);
        numberOfSectors = (RadialMenuSectors)actions.Count;
        
    }
    public void ShowSectors()
    {
        radialMenu.ShowSectors(numberOfSectors, actions);
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
        ShowSectors();
        gameObject.SetActive(true);
        menuInvoke = true;
        
    }

    public void RevokeMenu()
    {
        menuInvoke = false;
        ClearActions();
        //gameObject.SetActive(false);
    }

    #region Selections Modes
    
    private void FiveSectosMode(Vector2 thumb)
    {
        if (thumb.x >= 0  && thumb.x <= 0.9510565163 && thumb.y >= 0.3090169944 && thumb.y <= 1)
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.FIRST_5_SECTORS);
        }

        if (thumb.x >= 0.5877852523 && thumb.x <= 0.9510565163 && thumb.y >= -0.8090169944 && thumb.y <= 0.3090169944)
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.SECOND_5_SECTORS);
        }

        if (thumb.x >= -0.5877852523 && thumb.x <= 0.5877852523 && thumb.y >= -0.9 && thumb.y <= -0.7)
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.THIRD_5_SECTORS);
        }

        if (thumb.x >= -0.9510565163 && thumb.x <= -0.5877852523 && thumb.y >= -0.8090169944 && thumb.y <= 0.3090169944)
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.FOURTH_5_SECTORS);
        }

        if (thumb.x >= -0.5877852523 && thumb.x <= 0 && thumb.y >= 0.3090169944 && thumb.y <= 1)
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.FIFTH_5_SECTORS);
        }

    }
    private void FourSectosMode(Vector2 thumb)
    {
        if (thumb.x >= 0 && thumb.x <= 1 && thumb.y >= 0 && thumb.y <= 1)
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.FIRST_4_SECTORS);
        }
        if (thumb.x >= 0 && thumb.x <= 1 && thumb.y >= -1 && thumb.y <= 0)
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.SECOND_4_SECTORS);
        }
        if (thumb.x >= -1 && thumb.x <= 0 && thumb.y >= -1 && thumb.y <= 0)
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.THIRD_4_SECTORS);
        }
        if (thumb.x >= -1 && thumb.x <= 0 && thumb.y >= 0 && thumb.y <= 1)
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.FOURTH_4_SECTORS);
        }
    }
    private void ThreeSectosMode(Vector2 thumb)
    {
        if (thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= 0.5 && thumb.y <= 1 ||
            thumb.x >= 0.7 && thumb.x <= 0.9 && thumb.y >= -0.5 && thumb.y <= 0.5
            

            )
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.FIRST_3_SECTORS);
        }

        if (thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= -1 && thumb.y <= -0.5 ||
           thumb.x >= 0 && thumb.x <= 0.866 && thumb.y >= -1 && thumb.y <= -0.5           
           )
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.SECOND_3_SECTORS);
        }
        if (thumb.x >= -0.9 && thumb.x <= -0.7 && thumb.y >= -0.5 && thumb.y <= 0.5 ||
           thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= 0.5 && thumb.y <= 1
          )
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.THIRD_3_SECTORS);
        }
    }
    private void TwoSectosMode(Vector2 thumb)
    {
        if (thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= 0.5 && thumb.y <= 1 ||
            thumb.x >= 0.7 && thumb.x <= 0.9 && thumb.y >= -0.5 && thumb.y <= 0.5 ||
            thumb.x >= 0 && thumb.x <= 0.866 && thumb.y >= -1 && thumb.y <= -0.5

            )
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.FIRST_2_SECTORS);
        }

        if (thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= -1 && thumb.y <= -0.5 ||
           thumb.x >= -0.9 && thumb.x <= -0.7 && thumb.y >= -0.5 && thumb.y <= 0.5 ||
           thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= 0.5 && thumb.y <= 1

           )
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.SECOND_2_SECTORS);
        }
    }
    private void OneSectosMode(Vector2 thumb)
    {
        if (thumb.x >= 0.7 && thumb.x <= 0.9 && thumb.y >= -0.5 && thumb.y <= 0.5 ||
            thumb.x >= 0 && thumb.x <= 0.866 && thumb.y >= 0.5 && thumb.y <= 1 ||
            thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= 0.5 && thumb.y <= 1 ||
            thumb.x >= -0.9 && thumb.x <= -0.7 && thumb.y >= -0.5 && thumb.y <= 0.5||
            thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= -1 && thumb.y <= -0.5||
            thumb.x >= 0 && thumb.x <= 0.866 && thumb.y >= -1 && thumb.y <= -0.5
            )
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.FIRST_1_SECTORS);
        }
    }
    private void SixSectosMode(Vector2 thumb)
    {
        if (thumb.x >= 0.7 && thumb.x <= 0.9 && thumb.y >= -0.5 && thumb.y <= 0.5)
        {
            //print("RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.SECOND_6_SECTORS);
        }
        if (thumb.x >= 0 && thumb.x <= 0.866 && thumb.y >= 0.5 && thumb.y <= 1)
        {
            //print("TOP RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.FIRST_6_SECTORS);
        }
        if (thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= 0.5 && thumb.y <= 1)
        {
            //print("TOP LEFT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.SIXTH_6_SECTORS);
        }
        if (thumb.x >= -0.9 && thumb.x <= -0.7 && thumb.y >= -0.5 && thumb.y <= 0.5)
        {
            //print("LEFT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.FIFTH_6_SECTORS);
        }
        if (thumb.x >= -0.866 && thumb.x <= 0 && thumb.y >= -1 && thumb.y <= -0.5)
        {
            //print("BOTTOM LEFT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.FOURTH_6_SECTORS);
        }
        if (thumb.x >= 0 && thumb.x <= 0.866 && thumb.y >= -1 && thumb.y <= -0.5)
        {
            //print("BOTTOM RIGHT");
            stickRelease = false;
            ResolveShowSelect(RadialMenuSectorV2.THIRD_6_SECTORS);
        }
    }
    #endregion

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
            Vector2 thumb = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

            if (thumb != Vector2.zero)
            {
               

                switch (numberOfSectors)
                {
                    case RadialMenuSectors.SIX:                        
                        SixSectosMode(thumb);
                        break;
                    case RadialMenuSectors.FIVE:
                        FiveSectosMode(thumb);
                        break;
                    case RadialMenuSectors.FOUR:
                        FourSectosMode(thumb);
                        break;
                    case RadialMenuSectors.THREE:
                        ThreeSectosMode(thumb);
                        break;
                    case RadialMenuSectors.TWO:
                        TwoSectosMode(thumb);
                        break;
                    case RadialMenuSectors.ONE:
                       OneSectosMode(thumb);
                        break;                  
                }
               

            }

            if(!OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.RTouch) &&
            !OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.RTouch) &&
            !OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch) &&
            !OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch))
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
            currentChosenSector.UseAction();
            currentChosenSector.ResolveSelectSector(false);
            print(string.Format("Invoked {0}!!!", currentChosenSector.name));
            currentChosenSector = null;
            
        }
    }

}
