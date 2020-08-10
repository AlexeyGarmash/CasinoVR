using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class PlayerPlace : MonoBehaviourPun/*, IListener<ROULETTE_EVENT>*/
{
    public PlayerStats ps;
    private bool placeTaken = false;
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

    bool syncStarted = false; 
    public void PreparePlayerPlace()
    {
        StartCoroutine(CreateChipWithDelay());
        SyncStacks();
    }
    public void StartWinAnimation(int win, string owner)
    {
        playerWinAnim.StartAnimation(win, owner);        
        SyncStacks();
    }

    public void SyncStacks()
    {
        if(!syncStarted)
            StartCoroutine(SynchronizeStacks());
    }
    IEnumerator SynchronizeStacks()
    {
        syncStarted = true;
        yield return new WaitForSeconds(2f);
        while (sf.Stacks.ToList().Sum(s => s.animator.AnimationFlag) != sf.Stacks.Length)
        {

            yield return new WaitForSeconds(0.1f);
          
        }


        for(var i =0; i < sf.Stacks.Length; i++)
        {
            for (var j = 0; j < sf.Stacks[i].Objects.Count; j++)
            {
                var position = sf.Stacks[i].Objects[i].transform.position;
                var viewID = sf.Stacks[i].Objects[i].GetComponent<PhotonView>().ViewID;

                photonView.RPC("SyncGameObjects", RpcTarget.Others, viewID, position, i, j);
            }
        }


        foreach (var stack in sf.Stacks)      
            foreach (var chip in stack.Objects)
                chip.GetComponent<PhotonSyncCrontroller>().SyncOn_Photon();


        photonView.RPC("UpdateAllStacks", RpcTarget.All);

        syncStarted = false;

    }

    [PunRPC]
    public void UpdateAllStacks()
    {
        foreach (var stack in sf.Stacks)
        {
            stack.UpdateStackInstantly();
            stack.animator.ChangeStateOfItem(true, false, ViewSynchronization.Unreliable);
        }
    }

    
    [PunRPC]
    public void SyncGameObjects(int viewID, Vector3 position, int stackIndex, int chipsIndex)
    {
        sf.Stacks[stackIndex].Objects[chipsIndex].GetComponent<PhotonView>().ViewID = viewID;
        sf.Stacks[stackIndex].Objects[chipsIndex].transform.position = position;

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

                yield return new WaitForSeconds(0.01f);
            }
        }
       

        
    }

   
}
