﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerPlace : MonoBehaviourPun
{
    public PlayerStats ps;
    private bool placeTaken = false;
    PlayerChipsField sf;

    EventManager<ROULETTE_EVENT> rouletteManager;

    
    private void Start()
    {
        ps = null;
        rouletteManager = GetComponentInParent<TableBetsManager>().rouletteEventManager;
     
        sf = GetComponentInChildren<PlayerChipsField>();
    }

     private void OnTriggerStay(Collider other)
    {
        /*if (other.gameObject.tag == "Player")
        {
            if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            {
                if (ps == null && !placeTaken)
                {
                    photonView.RPC("TakePlace_RPC", RpcTarget.All);
                    ps = other.gameObject.GetComponent<PlayerStats>();
                    PreparePlayerPlace();
                }
                else {
                    photonView.RPC("GoOutPlace_RPC", RpcTarget.All);
                    ps = null;
                    sf.ClearPlace();
                }
            }
        }*/
    }

    public void TakePlace()
    {
        print("Button clikced");
        if (ps == null && !placeTaken)
        {
            print("Button clikced ps == null");
            photonView?.RPC("TakePlace_RPC", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);
            ps = new PlayerStats(PhotonNetwork.LocalPlayer.NickName);
            //PreparePlayerPlace();
        }
    }

    public void GoOutFromPlace()
    {
        if (ps != null && placeTaken)
        {
            print("Button clikced ps != null");
            photonView?.RPC("GoOutPlace_RPC", RpcTarget.All);
            ps = null;
            //sf.ClearPlace();
        }
    }

    [PunRPC]
    public void TakePlace_RPC(string nickname)
    {
        placeTaken = true;
        ps = new PlayerStats(nickname);
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
                    sf.InstantiateToStackWithColor(Chips.PURPLE, ref money);

                }
                else if (starmoney / 4 < money)
                {
                    sf.InstantiateToStackWithColor(Chips.BLACK, ref money);

                }
                else if (starmoney / 8 < money)
                {
                    sf.InstantiateToStackWithColor(Chips.GREEN, ref money);

                }
                else if (starmoney / 16 < money)
                {
                    sf.InstantiateToStackWithColor(Chips.BLUE, ref money);

                }
                else if (starmoney / 32 < money)
                {
                    sf.InstantiateToStackWithColor(Chips.RED, ref money);

                }
                else sf.InstantiateToStackWithColor(Chips.YELLOW, ref money);

            }
        }
    }

   
    
}
