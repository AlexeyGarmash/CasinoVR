using Assets.Scipts;
using Assets.Scipts.Chips;

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinAnimation : CurveAnimator
{
    [PunRPC]
    void InstantiateChip(int viewID, int chip, string owner)
    {
        var chipObj = Instantiate(ChipUtils.Instance.GetChipByChipEnum((Chips)chip), transform.position, transform.rotation);
        chipObj.GetComponent<PhotonView>().ViewID = viewID;
        chipObj.GetComponent<OwnerData>().Owner = owner;
        chipObj.SetActive(false);
        ObjectToAnimation.Add(chipObj);
    }
    public void StartAnimation(int win, string nickName)
    {
        CreateWinChips(win, nickName);
        photonView.RPC("StartAnimation_RPC", RpcTarget.All, 0);    
    }
    private void CreateWinChips(int money, string nickName)
    {
        
        if(photonView.IsMine)
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


                var chip = Instantiate(ChipUtils.Instance.GetChipByChipEnum(chipCost), transform.position, transform.rotation);
                var view = chip.GetComponent<PhotonView>();
                chip.GetComponent<OwnerData>().Owner = nickName;

                ObjectToAnimation.Add(chip);

                PhotonNetwork.AllocateViewID(view);
                chip.SetActive(false);

                photonView.RPC("InstantiateChip", RpcTarget.OthersBuffered, view.ViewID, (int)chipCost, nickName);
                money -= (int)chipCost;
                
            }
        }

       
    }
    

    

    


}

