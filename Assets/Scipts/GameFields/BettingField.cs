using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    new void Start()
    {
        base.Start();

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
                ClearStacks();
                canBet = true;
                break;
            case ROULETTE_EVENT.ROULETTE_GAME_START:
                
                canBet = false;
                break;
            case ROULETTE_EVENT.PLAYER_LEAVE:
                tableCell.ReceiveBetDataByName((string)Param[0]);
                var stacks = Stacks.ToList().FindAll(s => s.playerName == (string)Param[0]);               
                stacks.ForEach(s => { s.StopAllCoroutines();  s.ClearData(); });
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
            var grabbable = other.gameObject.GetComponent<OVRGrabbableCustom>();

            if (chip != null && tableCell != null && grabbable.isGrabbed)
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
