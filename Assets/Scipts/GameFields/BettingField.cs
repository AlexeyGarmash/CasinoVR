﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingField : ChipsField, IListener<ROULETTE_EVENT>
{  
   
  
   
    private TableCell tableCell;
    public float yOffset = 0.0073f;
    EventManager<ROULETTE_EVENT> EventManager;

    bool canBet = true;
    protected new void Awake()
    {
        base.Awake();
        tableCell = GetComponent<TableCell>();
    }

    void Start()
    {
        EventManager = GetComponentInParent<TableBetsManager>().rouletteEventManager;

      
        EventManager.AddListener(ROULETTE_EVENT.ROULETTE_GAME_START, this);
        EventManager.AddListener(ROULETTE_EVENT.ROULETTE_GAME_END, this);

    }
    public void OnEvent(ROULETTE_EVENT Event_type, Component Sender, params object[] Param)
    {
               
        switch (Event_type)
        {           
               
            case ROULETTE_EVENT.ROULETTE_GAME_END:
                ClearStacks();
                canBet = true;
                break;
            case ROULETTE_EVENT.ROULETTE_GAME_START:
                
                canBet = false;
                break;
        }
    }
    
    
  
    
    private new void OnTriggerEnter(Collider other)
    {

        if (canBet)
        {
            var chip = other.gameObject.GetComponent<ChipData>();
            var bettingController = other.gameObject.GetComponentInParent<BetPositionController>();

            if (chip != null && tableCell != null && bettingController == null)
            {
                chip.Owner = PhotonNetwork.LocalPlayer.NickName;
                var grabbadBy = other.gameObject.GetComponent<GrabbableChip>().grabbedBy;

                if(grabbadBy == null && !Contain(chip.gameObject))                    
                {
                    var chipPhotonView = chip.GetComponent<PhotonView>();
                    MagnetizeObject(other.gameObject, FindStackByName(chip.transform));
                    if (chipPhotonView != null && chipPhotonView.IsMine)
                    {
                        tableCell.ReceiveBetData(new BetData(new PlayerStats(chip.Owner), (int)chip.Cost));
                    }
                 }
                }
            }
        }

    private new void OnTriggerStay(Collider other)
    {
        if (canBet)
        {
            var chip = other.gameObject.GetComponent<ChipData>();


            if (chip != null && tableCell != null && chip.isGrabbed)
            {

                
                var chipPhotonView = chip.GetComponent<PhotonView>();
                ExtranctChip(chipPhotonView.ViewID);

                Debug.Log("OnTriggerStay");
                if (chipPhotonView != null && chipPhotonView.IsMine /*&& ExtranctChipOnAll(chipPhotonView.ViewID)*/)
                {
                    tableCell.RemoveBetData(new BetData(new PlayerStats(chip.Owner), (int)chip.Cost));
                }
                
            }
        }
    }   

}
