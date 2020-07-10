using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Chips { YELLOW = 1, RED = 5, BLUE = 10, GREEN = 25, BLACK = 100, PURPLE = 250}


public class PlayerInfo : MonoBehaviour
{
    PlayerStats ps;
    PlayerStackField sf;

    private void Start()
    {
        sf = GetComponentInChildren<PlayerStackField>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            

            if (OVRInput.GetDown(OVRInput.Button.Three))
            {
                if (ps == null)
                {
                    ps = other.gameObject.GetComponent<PlayerStats>();
                    PreparePlayerPlace();
                }
                else {
                    ps = null;
                    sf.Clear();

                }
            }
        }
    }

    public void PreparePlayerPlace()
    {
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
