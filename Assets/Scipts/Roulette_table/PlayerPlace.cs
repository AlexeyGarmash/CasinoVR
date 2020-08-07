using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerPlace : MonoBehaviourPun/*, IListener<ROULETTE_EVENT>*/
{
    public PlayerStats ps;
    private bool placeTaken = false;
    PlayerChipsField sf;
    public PlayerWinAnimation playerWinAnim;
    public int PlayerPlaceID;

    EventManager<ROULETTE_EVENT> rouletteManager;

    public bool canLeave;
    private void Start()
    {
        ps = null;
       
        rouletteManager = GetComponentInParent<TableBetsManager>().rouletteEventManager;
        //rouletteManager.AddListener(ROULETTE_EVENT.PLAYER_CONNECTED, this);

        playerWinAnim = GetComponentInChildren<PlayerWinAnimation>();
        sf = GetComponentInChildren<PlayerChipsField>();
    }


    public void TakePlace(PlayerStats ps)
    {
        print("Button clikced");
        if (ps != null && !placeTaken)
        {
            print("Button clikced ps == null");
            photonView?.RPC("TakePlace_RPC", RpcTarget.All, ps.PlayerNick, ps.AllMoney);          
            PreparePlayerPlace();
            playerWinAnim.StartAnimation(1000, ps.PlayerNick);
        }
    }


    public void GoOutFromPlace()
    {
        if (ps != null && placeTaken)
        {
            print("Button clikced ps != null");
            photonView?.RPC("GoOutPlace_RPC", RpcTarget.All);
            ps = null;
            sf.ClearStacks();
        }
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
        Debug.Log(sf);
        
        int money = ps.AllMoney;
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

               
            }
        }
       

        
    }

    //public void OnEvent(ROULETTE_EVENT Event_type, Component Sender, params object[] Param)
    //{
    //    switch (Event_type)
    //    {
    //        case ROULETTE_EVENT.PLAYER_CONNECTED:
    //            if (PlayerPlaceID == (int)Param[0])
    //                ps = (PlayerStats)Param[1];
    //            break;
    //    }
    //}
}
