using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class PlayerPlace : MonoBehaviourPun
{
    public Action<bool, PlayerStats> actionReadyOrNot;
    public PlayerStats ps;
    private bool placeTaken = false;

    public bool IsPlaceTaken {get => placeTaken;}
    private bool readyToPlay = false;
    public bool IsReady {get => readyToPlay; }
    PlayerChipsField sf;
    public PlayerWinAnimation playerWinAnim;
   

 



   
    public bool canLeave;

    private void Start()
    {
        ps = null;
        playerWinAnim = GetComponentInChildren<PlayerWinAnimation>();
        sf = GetComponentInChildren<PlayerChipsField>();
    }

    public void TakePlace(PlayerStats ps)
    {
        print("Button clikced");
        if (ps != null && !placeTaken)
        {
            placeTaken = true;
            this.ps = ps;
            print("Button clikced ps == null");
            photonView?.RPC("TakePlace_RPC", RpcTarget.Others, ps.PlayerNick, ps.AllMoney);          
            PreparePlayerPlace();
            photonView.RequestOwnership();
            sf.photonView.RequestOwnership();
            //StartWinAnimation(1000, ps.PlayerNick);

        }
    }


    public void GoOutFromPlace()
    {
        if (ps != null && placeTaken)
        {
            placeTaken = false;
            print("Button clikced ps != null");
            photonView?.RPC("GoOutPlace_RPC", RpcTarget.All);
            ps = null;
            sf.ClearStacks();
        }
    }

    public void ReadyToPlay() {
        if(ps != null && !readyToPlay) {
            readyToPlay = true;
            photonView?.RPC("ReadyPlay_RPC", RpcTarget.All);
            actionReadyOrNot.Invoke(true, ps);
        }
    }

    public void NotReadyToPlay() {
        if(ps != null && readyToPlay) {
            readyToPlay = false;
            photonView?.RPC("NotReadyToPlay_RPC", RpcTarget.All);
            actionReadyOrNot.Invoke(false, ps);
        }
    }

    [PunRPC]
    public void ReadyPlay_RPC() {
        readyToPlay = true;
    }


    [PunRPC]
    public void NotReadyToPlay_RPC() {
        readyToPlay = false;
    }

    [PunRPC]
    public void TakePlace_RPC(string nickname, int money)
    {

        placeTaken = true;
        ps = new PlayerStats(nickname, money);
        print("RPC TAKE PLACE!!!");
    }

    [PunRPC]
    public void GoOutPlace_RPC()
    {
        placeTaken = false;
        ps = null;
        print("RPC GO OUT FROM PLACE!!!");
    }

   
    public void PreparePlayerPlace()
    {
        StartCoroutine(CreateChipWithDelay());
        
    }
    public void StartWinAnimation(int win, string owner)
    {
        playerWinAnim.StartAnimation(win, owner);        
       
    }


    public IEnumerator CreateChipWithDelay()
    {
        Debug.Log(sf);
        
        int money = 3000;
        if (money > 0)
        {

            var starmoney = money;
            
            while (money > 0)
            {
                if (starmoney / 2 < money)
                {
                    sf.InstantiateToStackWithColor(Chips.PURPLE, ref money, ps.PlayerNick);
                   
                }
                else if (starmoney / 4 < money)
                {
                    sf.InstantiateToStackWithColor(Chips.BLACK, ref money, ps.PlayerNick);

                }
                else if (starmoney / 8 < money)
                {
                    sf.InstantiateToStackWithColor(Chips.GREEN, ref money, ps.PlayerNick);

                }
                else if (starmoney / 16 < money)
                {
                    sf.InstantiateToStackWithColor(Chips.BLUE, ref money, ps.PlayerNick);

                }
                else if (starmoney / 32 < money)
                {
                    sf.InstantiateToStackWithColor(Chips.RED, ref money, ps.PlayerNick);

                }
                else sf.InstantiateToStackWithColor(Chips.YELLOW, ref money, ps.PlayerNick);

                yield return null;
            }
        }
       

        
    }

}
