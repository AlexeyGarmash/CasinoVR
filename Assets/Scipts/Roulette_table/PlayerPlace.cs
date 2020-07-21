using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerPlace : MonoBehaviour
{
    public PlayerStats ps;
    PlayerChipsField sf;

    EventManager<ROULETTE_EVENT> rouletteManager;

    
    private void Start()
    {
        rouletteManager = GetComponentInParent<TableBetsManager>().rouletteEventManager;
     
        sf = GetComponentInChildren<PlayerChipsField>();
    }

     private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            

            if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
            {
                if (ps == null)
                {
                    ps = other.gameObject.GetComponent<PlayerStats>();
                    PreparePlayerPlace();
                }
                else {
                    ps = null;
                    sf.ClearPlace();

                }
            }
        }
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
