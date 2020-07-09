using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public enum SYMBOL
    {
        BELL,
        PLUM,
        CHERRY,
        BAR,
        ORANGE,
        SEVEN,
        LEMON,
        NULL
    }

    public class SymbolItem
    {
        public SYMBOL Symbol { get; set; }
        public string Tag { get; set; }

        public SymbolItem(SYMBOL symbol, string tag)
        {
            Symbol = symbol;
            Tag = tag;
        }

        public override string ToString()
        {
            return Tag;
        }
    }
    public class SlotMachine : MonoBehaviour
    {
        public static List<SymbolItem> Slots = new List<SymbolItem> {
        new SymbolItem(SYMBOL.BAR, "bar"),
        new SymbolItem(SYMBOL.PLUM, "plum"),
        new SymbolItem(SYMBOL.PLUM, "plum"),
        new SymbolItem(SYMBOL.CHERRY, "cherry"),
        new SymbolItem(SYMBOL.CHERRY, "cherry"),
        new SymbolItem(SYMBOL.CHERRY, "cherry"),
        new SymbolItem(SYMBOL.BELL, "bell"),
        new SymbolItem(SYMBOL.ORANGE, "orange"),
        new SymbolItem(SYMBOL.ORANGE, "orange"),
        new SymbolItem(SYMBOL.SEVEN, "seven"),
        new SymbolItem(SYMBOL.LEMON, "lemon"),
        new SymbolItem(SYMBOL.LEMON, "lemon")
    };


        public bool GameInProgress { get; private set; } = false;
        public bool CoinInMachine { get; private set; } = false;
        public int NumberOfCoins { get; private set; } = 0;

        public bool StartGame()
        {
            //CoinInMachine = true;

            if (!GameInProgress && CoinInMachine)
            {
                GameInProgress = true;
                CoinInMachine = false;
                return true;
            }
            else if (GameInProgress)
                Debug.Log("Game in progress!");

            else
                Debug.Log("Insert coin!");

            return false;
        }

        public List<SymbolItem> RandomResult()
        {
            List<SymbolItem> sls = new List<SymbolItem>();

            sls.Add(Slots[Random.Range(0, Slots.Count)]);
            sls.Add(Slots[Random.Range(0, Slots.Count)]);
            sls.Add(Slots[Random.Range(0, Slots.Count)]);

            Debug.Log(sls[0] + " " + sls[1] + " " + sls[2]);

            return sls;

        }

        public List<SymbolItem> SameResult()
        {
            List<SymbolItem> sls = new List<SymbolItem>();

            int sameRandom = Random.Range(0, Slots.Count);

            sls.Add(Slots[sameRandom]);
            sls.Add(Slots[sameRandom]);
            sls.Add(Slots[sameRandom]);

            Debug.Log(sls[0] + " " + sls[1] + " " + sls[2]);

            return sls;
        }
        public void InsertCoin()
        {
            CoinInMachine = true;
            NumberOfCoins++;
        }

        public bool StopGame()
        {
            if (GameInProgress)
            {
                GameInProgress = false;
                NumberOfCoins = 0;
            }
            return !GameInProgress;
        }
    }

}