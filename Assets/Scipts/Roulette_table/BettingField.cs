using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingField : MonoBehaviour, IListener<ROULETTE_EVENT>
{  
   
    private BetStackData[] BetStacks;

    private TableCell tableCell;
    EventManager<ROULETTE_EVENT> EventManager;

    bool canBet = true;
    private void Awake()
    {
        BetStacks = GetComponentsInChildren<BetStackData>();
        tableCell = GetComponent<TableCell>();
    }

    void Start()
    {
        EventManager = transform.parent.parent.gameObject.GetComponent<TableBetsManager>().rouletteEventManager;

        EventManager.AddListener(ROULETTE_EVENT.BET_LOST, this);
        EventManager.AddListener(ROULETTE_EVENT.BET_WIN, this);
        EventManager.AddListener(ROULETTE_EVENT.ROULETTE_GAME_START, this);
        EventManager.AddListener(ROULETTE_EVENT.ROULETTE_GAME_END, this);

    }
    public void OnEvent(ROULETTE_EVENT Event_type, Component Sender, params object[] Param)
    {
               
        switch (Event_type)
        {
            case ROULETTE_EVENT.BET_LOST:
                BetData bd = (BetData)Param[0];
                ClearBettingField(bd.PlayerStat.PlayerNick);
                Debug.Log("BET_LOST");
                break;

            case ROULETTE_EVENT.BET_WIN:
                bd = (BetData)Param[0];
                ClearBettingField(bd.PlayerStat.PlayerNick);
                Debug.Log("BET_WIN");
                break;
            case ROULETTE_EVENT.ROULETTE_GAME_END:
                LockUnlockChips();
                canBet = true;
                break;
            case ROULETTE_EVENT.ROULETTE_GAME_START:
                
                canBet = false;
                break;
        }
    }
    private void ClearBettingField(string playerName)
    {
        for (var i = 0; i < BetStacks.Length; i++)
        {
            if (BetStacks[i].playerName == playerName)
            {
                BetStacks[i].ClearData();
                ChipsUtils.Instance.UpdateStack(BetStacks[i]);
            }
        }
    }
    
    void LockUnlockChips()
    {
        
        for (var i = 0; i < BetStacks.Length; i++)
        {
            for (var j = 0; j < BetStacks[i].Chips.Count; j++)
            {
                BetStacks[i].Chips[j].GetComponent<OVRGrabbable>().enabled = false;
                
            }
        }
    }
   
    
    private void OnTriggerEnter(Collider other)
    {

        if (canBet)
        {
            var chip = other.gameObject.GetComponent<ChipData>();
            if (chip != null && tableCell != null)
            {
                if (ChipsUtils.Instance.MagnetizeChip(other.gameObject, BetStacks))
                {
                    tableCell.ReceiveBetData(new BetData(new PlayerStats(chip.player), (int)chip.Cost));
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
                if (ChipsUtils.Instance.ExtractionChip(other.gameObject, BetStacks))
                {
                    tableCell.RemoveBetData(new BetData(new PlayerStats(chip.player), (int)chip.Cost));
                }
            }
        }
    }

}
