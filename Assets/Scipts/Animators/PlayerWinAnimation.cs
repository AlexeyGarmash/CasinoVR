using Assets.Scipts;
using Assets.Scipts.Chips;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinAnimation : CurveAnimator
{    
   
    public void StartAnimation(int win, string nickName)
    {
        CreateWinChips(win, nickName);
        photonView.RPC("StartAnimation_RPC", RpcTarget.All, 0);    
    }
    private void CreateWinChips(int money, string nickName)
    {
        
        var starmoney = money;
        Chips chipCost;
        
        while (money > 0)
        {
           
            chipCost = Chips.YELLOW;
            if (starmoney / 2 < money && money > (int)Chips.PURPLE)           
                chipCost = Chips.PURPLE;
            
            else if (starmoney / 4 < money && money > (int)Chips.BLACK)            
                chipCost = Chips.BLACK;

            else if (starmoney / 8 < money && money > (int)Chips.GREEN)           
                chipCost = Chips.GREEN;
    
            else if (starmoney / 16 < money && money > (int)Chips.BLUE)           
                chipCost = Chips.BLUE;              
            
            else if (starmoney / 32 < money && money > (int)Chips.RED)           
                chipCost = Chips.RED;               
                


            var chip = PhotonNetwork.Instantiate(ChipUtils.Instance.GetPathToChip(chipCost), transform.position, transform.rotation);
            chip.GetComponent<ChipData>().SetOwner_Photon(nickName);
            money -= (int)chipCost;
        }

       
    }
    

    

    


}

