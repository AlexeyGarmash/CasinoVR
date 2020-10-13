
﻿using Assets.Scipts.Chips;
﻿using Photon.Pun;
using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class PlayerPlace : MonoBehaviourPun
{
    public GameObject playerAreaField;
   
    public RadialMenuHandV2 handMenu;
    public Action<bool, PlayerStats> actionReadyOrNot;
    public Action actionJoinOut;
    public PlayerStats ps;
    [SerializeField]
    private bool placeTaken = false;

    public PlayerChipsField sf;
    private PlayerWinAnimation playerWinAnim;
   
    public bool canLeave;

    [SerializeField]
    private int _placeId;
    
    public int PlaceId { get => _placeId; }
    public bool PlayerOnPlace { get => ps.PlayerNick != ""; }

    private void Awake()
    {
        ps = GetComponent<PlayerStats>();

    }
    public bool IsPlaceTaken {get => placeTaken;}
    private bool readyToPlay = false;
    public bool IsReady {get => readyToPlay; }
   


    private void Start()
    {
      
        playerWinAnim = GetComponentInChildren<PlayerWinAnimation>();
        sf = GetComponentInChildren<PlayerChipsField>();
        //sf.gameObject.SetActive(false);
    }

    public void TakePlace(PlayerStats ps)
    {
        print("Button clikced");
        if (ps.IsNotNull() && !placeTaken)
        {      
            if(playerAreaField)
                playerAreaField.SetActive(true);  
            
            placeTaken = true;
            //actionJoinOut.Invoke();
            this.ps = ps;

            var pc = ps.GetComponentInParent<OVRPlayerController>();
            pc.EnableRotation = false;
            pc.GetComponentInChildren<TeleportDisabler>().Disable();


            sf.gameObject.SetActive(true);
            photonView.RequestOwnership();
            sf.photonView.RequestOwnership();

            photonView.SetOwnerInternal(PhotonNetwork.LocalPlayer, PhotonNetwork.LocalPlayer.ActorNumber);
            print("Button clikced ps == null");
          
            photonView.RPC("TakePlace_RPC", RpcTarget.Others, ps.PlayerNick, ps.AllMoney);
            PreparePlayerPlace();

           

        }
    }


    public void GoOutFromPlace(PlayerStats player)
    {
        if (ps.IsNotNull() && placeTaken)
        {
            sf.gameObject.SetActive(false);
            if (playerAreaField)
                playerAreaField.SetActive(false);
            var pc = ps.GetComponentInParent<OVRPlayerController>();
            pc.EnableRotation = true;
            pc.GetComponentInChildren<TeleportDisabler>().Enable();
            placeTaken = false;
            ps = GetComponent<PlayerStats>();
            print("Button clikced ps != null");
            photonView?.RPC("GoOutPlace_RPC", RpcTarget.All);
          
            sf.ClearStacks();


        }
    }

    public void ReadyToPlay() {
        if(ps != null && !readyToPlay) {
            print("");
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
        sf.gameObject.SetActive(true);
        placeTaken = true;
        ps.PlayerNick = nickname;
        ps.AllMoney = money;
        print("RPC TAKE PLACE!!!");
    }

    [PunRPC]
    public void GoOutPlace_RPC()
    {
        sf.gameObject.SetActive(false);
        ps.PlayerNick = "";
        ps.AllMoney = 0;

        print("RPC GO OUT FROM PLACE!!!");
        sf.ClearStacks();
    }

   
    public void PreparePlayerPlace()
    {
        StartCoroutine(CreateChipWithDelay());
        
    }
    public void StartWinAnimation(int win, string owner)
    {
        playerWinAnim.StartAnimation(win, owner);        
       
    }

    [PunRPC]
    void InstatianeChip_RPC(int chipCost, string playerNick, int viewID)
    {
        var stack = sf.StacksByChipCost[(Chips)chipCost];
        
        var chip = Instantiate(ChipUtils.Instance.GetPrefabByColor((Chips)chipCost), stack.position, stack.rotation);
        chip.GetComponent<PhotonView>().ViewID = viewID;
        var chipData = chip.GetComponent<ChipData>();
        chipData.Owner = playerNick;
        chip.GetComponent<PhotonSyncCrontroller>().SyncOff_RPC();

        sf.MagnetizeObject(chip, stack.GetComponent<StackData>(), ChipUtils.Instance.GetStringOfType(chipData.Cost));

    }
    public void InstantiateToStackWithColor(Chips chipsCost, ref int money, string playerNick)
    {
        var stack = sf.StacksByChipCost[chipsCost];

        var chip = Instantiate(ChipUtils.Instance.GetPrefabByColor(chipsCost), stack.position, stack.rotation);

        var chipData = chip.GetComponent<ChipData>();
        chipData.Owner = playerNick;
        PhotonNetwork.AllocateViewID(chipData.photonView);

        chip.GetComponent<PhotonSyncCrontroller>().SyncOff_RPC();

        photonView.RPC("InstatianeChip_RPC", RpcTarget.OthersBuffered, (int)chipsCost, playerNick, chipData.photonView.ViewID);

        sf.MagnetizeObject(chip, stack.GetComponent<StackData>(), ChipUtils.Instance.GetStringOfType(chipData.Cost));

        money -= (int)chipsCost;
    }

    public IEnumerator CreateChipWithDelay()
    {
        Debug.Log(sf);
        
        int money = ps.AllMoney;
        if (money > 0)
        {

            var starmoney = money;
            
            while (money > 0)
            {
                if(money < 5)
                    InstantiateToStackWithColor(Chips.YELLOW, ref money, ps.PlayerNick);
                else if (starmoney / 2 < money)               
                    InstantiateToStackWithColor(Chips.PURPLE, ref money, ps.PlayerNick);
             
                else if (starmoney / 4 < money)               
                    InstantiateToStackWithColor(Chips.BLACK, ref money, ps.PlayerNick);
                
                else if (starmoney / 8 < money)              
                    InstantiateToStackWithColor(Chips.GREEN, ref money, ps.PlayerNick);
                
                else if (starmoney / 16 < money)                
                    InstantiateToStackWithColor(Chips.BLUE, ref money, ps.PlayerNick);
                
                else InstantiateToStackWithColor(Chips.RED, ref money, ps.PlayerNick);

                
               


                yield return null;
            }
        }
       

        
    }

    public bool ExtractChipByCost(Chips cost, out GameObject chip)
    {

        chip = null;
        foreach (var stack in sf.Stacks)
        {

            chip = stack.Objects.FirstOrDefault(o => o.GetComponent<ChipData>().Cost == cost);
            if (chip != null)
            {
                chip.GetComponent<OwnerData>().ExtractObject();

                return true;
                
            }
 
        }
        return false;
       
    }

}
