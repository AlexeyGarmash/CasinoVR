using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class PlayerPlace : MonoBehaviourPun
{
    public PlayerStats ps;
    [SerializeField]
    private bool placeTaken = false;

    private PlayerChipsField sf;
    private PlayerWinAnimation playerWinAnim;
   
    public bool canLeave;

    [SerializeField]
    private int _placeId;
    
    public int PlaceId { get => _placeId; }
    public bool PlayerOnPlace { get => ps.PlayerNick != ""; }
    public bool PlayerReady { get => true; }

    private void Awake()
    {
        ps = GetComponent<PlayerStats>();
        playerWinAnim = GetComponentInChildren<PlayerWinAnimation>();
        sf = GetComponentInChildren<PlayerChipsField>();
    }
    private void Start()
    {
       
    }


    public void TakePlace(PlayerStats ps)
    {
        print("Button clikced");
        if (ps != null && !placeTaken)
        {
            placeTaken = true;
            //this.ps = ps;
            photonView.RequestOwnership();
            sf.photonView.RequestOwnership();
            print("Button clikced ps == null");
          
            photonView?.RPC("TakePlace_RPC", RpcTarget.Others, ps.PlayerNick, ps.AllMoney);
            PreparePlayerPlace();

            //StartWinAnimation(1000, ps.PlayerNick);

        }
    }


    public void GoOutFromPlace()
    {
        if (ps != null && placeTaken)
        {
            placeTaken = false;
            ps = GetComponent<PlayerStats>();
            print("Button clikced ps != null");
            photonView?.RPC("GoOutPlace_RPC", RpcTarget.All);
          
            sf.ClearStacks();
        }
    }

    [PunRPC]
    public void TakePlace_RPC(string nickname, int money)
    {

        placeTaken = true;
        ps.PlayerNick = nickname;
        ps.AllMoney = money;
        print("RPC TAKE PLACE!!!");
    }

    [PunRPC]
    public void GoOutPlace_RPC()
    {

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

                yield return null;
            }
        }
       

        
    }

}
