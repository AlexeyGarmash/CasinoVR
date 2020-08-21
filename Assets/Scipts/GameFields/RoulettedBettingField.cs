using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoulettedBettingField : ChipsField, IListener<ROULETTE_EVENT>
{  
   
  
   
    private TableCell tableCell;
    public float yOffset = 0.0073f;
    EventManager<ROULETTE_EVENT> EventManager;
    private GlowPart glowPart;

    private bool isReadyRemoveBet = false;

    bool canBet = true;
    protected new void Awake()
    {
        base.Awake();
        tableCell = GetComponent<TableCell>();
    }

    void Start()
    {

        base.Start();
        glowPart = GetComponentInChildren<GlowPart>();

        EventManager = GetComponentInParent<TableBetsManager>().rouletteEventManager;

      
        EventManager.AddListener(ROULETTE_EVENT.ROULETTE_GAME_START, this);
        EventManager.AddListener(ROULETTE_EVENT.ROULETTE_GAME_END, this);
        EventManager.AddListener(ROULETTE_EVENT.PLAYER_LEAVE, this);

    }
    public void OnEvent(ROULETTE_EVENT Event_type, Component Sender, params object[] Param)
    {
               
        switch (Event_type)
        {           
               
            case ROULETTE_EVENT.ROULETTE_GAME_END:
                ClearGlow();
                ClearStacks();
                canBet = true;
                break;
            case ROULETTE_EVENT.ROULETTE_GAME_START:
                
                canBet = false;
                break;
            case ROULETTE_EVENT.PLAYER_LEAVE:
                print("PLAYER LEAVE");
                tableCell.ReceiveBetDataByName((string)Param[0]);
                var stacks = Stacks.ToList().FindAll(s => s.playerName == (string)Param[0]);               
                stacks.ForEach(s => { s.StopAllCoroutines();  s.ClearData(); });
                break;
        }
    }

    private void ClearGlow()
    {
        if(glowPart != null)
            glowPart.GlowCell(false, true);
    }

    private new void OnTriggerEnter(Collider other)
    {

        if (canBet)
        {
            var chip = other.gameObject.GetComponent<ChipData>();
            var bettingController = other.gameObject.GetComponentInParent<BetPositionController>();

            if(chip != null && tableCell != null)
            {
                glowPart.GlowCell(true, true);
            }

            if (chip != null && tableCell != null && bettingController == null)
            {
                chip.Owner = PhotonNetwork.LocalPlayer.NickName;
                var grabbadBy = other.gameObject.GetComponent<GrabbableChip>().grabbedBy;
                
                if (grabbadBy == null && !Contain(chip.gameObject))
                {
                    var chipPhotonView = chip.GetComponent<PhotonView>();
                    MagnetizeObject(other.gameObject, FindStackByName(chip.transform));
                    if (chipPhotonView != null && chipPhotonView.IsMine)
                    {
                        isReadyRemoveBet = true;
                        glowPart.GlowCell(false, false);
                        print(string.Format("Invoke RECEIVE bet at cell {0} by player {1} chip {2}$", tableCell.name, chip.Owner, chip.Cost));
                        //tableCell.ReceiveBetData(new BetData(new PlayerStats(chip.Owner), (int)chip.Cost));
                        tableCell.ReceiveBetData(new BetData(chip.Owner, (int)chip.Cost));
                    }
                }
            }
        }
        else
        {
            glowPart.GlowCell(true, false);
        }
    }

    private new void OnTriggerStay(Collider other)
    {
        if (canBet)
        {
            var chip = other.gameObject.GetComponent<ChipData>();
            var grabbable = other.gameObject.GetComponent<OVRGrabbableCustom>();

            if (chip != null && tableCell != null && grabbable.isGrabbed)
            {

                
                var chipPhotonView = chip.GetComponent<PhotonView>();
                ExtranctObject(chipPhotonView.ViewID);

                ///Debug.Log("OnTriggerStay");
                
                if (isReadyRemoveBet && chipPhotonView != null && chipPhotonView.IsMine /*&& ExtranctChipOnAll(chipPhotonView.ViewID)*/)
                {
                    isReadyRemoveBet = false;
                    print(string.Format("Invoke REMOVE bet at cell {0} by player {1} with chip {2}$", tableCell.name, chip.Owner, chip.Cost));
                    //tableCell.RemoveBetData(new BetData(new PlayerStats(chip.Owner), (int)chip.Cost));
                    tableCell.RemoveBetData(new BetData(chip.Owner, (int)chip.Cost));
                }
            }
        }
    }

    

    private void OnTriggerExit(Collider other)
    {
        var chip = other.gameObject.GetComponent<ChipData>();

        if(chip != null)
        {
            glowPart.GlowCell(false, false);
        }
    }

}
