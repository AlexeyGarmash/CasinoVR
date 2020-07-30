using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingField : MonoBehaviour, IListener<ROULETTE_EVENT>
{  
   
    public BetStackData[] BetStacks;

    private TableCell tableCell;
    public float yOffset = 0.0073f;
    EventManager<ROULETTE_EVENT> EventManager;

    bool canBet = true;
    private void Awake()
    {
        BetStacks = GetComponentsInChildren<BetStackData>();
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
                ClearBettingFields();
                canBet = true;
                break;
            case ROULETTE_EVENT.ROULETTE_GAME_START:
                
                canBet = false;
                break;
        }
    }
    private void ClearBettingFields()
    {
        for (var i = 0; i < BetStacks.Length; i++)
        {
            foreach (Transform child in transform)
                foreach (Transform child1 in child)
                    Destroy(child1.gameObject);
                        
            BetStacks[i].ClearData();
      
        }
    }
    
  
    
    private void OnTriggerEnter(Collider other)
    {

        if (canBet)
        {
            var chip = other.gameObject.GetComponent<ChipData>();
            var bettingController = other.gameObject.GetComponentInParent<BetPositionController>();

            if (chip != null && tableCell != null && bettingController == null)
            {
                var grabbadBy = other.gameObject.GetComponent<GrabbableChip>().grabbedBy;
                if(grabbadBy == null)
                    if (StackUtils.Instance.MagnetizeObject(other.gameObject, chip.player, yOffset, BetStacks))
                    {
                        tableCell.ReceiveBetData(new BetData(new PlayerStats(PhotonNetwork.LocalPlayer.NickName), (int)chip.Cost));
                    }
            }
        }
    }
   
    private void OnTriggerStay(Collider other)
    {
        if (canBet)
        {
            var chip = other.gameObject.GetComponent<ChipData>();
           

            if (chip != null && tableCell != null)
            {
                var grabbadBy = other.gameObject.GetComponent<GrabbableChip>().grabbedBy;
                if (grabbadBy != null)
                    if (StackUtils.Instance.ExtractionObject(other.gameObject, yOffset, BetStacks))
                {
                    tableCell.RemoveBetData(new BetData(new PlayerStats(chip.player), (int)chip.Cost));
                }
            }
        }
    }

}
