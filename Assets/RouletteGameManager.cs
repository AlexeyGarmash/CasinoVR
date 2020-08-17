using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RouletteGameManager : MonoBehaviour, IListener<ROULETTE_EVENT>
{
    enum RouletteGameState { WaitingPlayers, Bettring, StatSpin, RouletteWaiting }

    [SerializeField]
    TableBetsManager tbm;
    
    [SerializeField]
    TMP_Text text;

    [SerializeField]
    int stateWaitTime = 30;
    int currentTime = 0;
    int gameTime = 0;
    const int sec = 1;
    bool rouletteStoped = false;
    RouletteGameState state = RouletteGameState.WaitingPlayers;

    EventManager<ROULETTE_EVENT> em;
    void Start()
    {
        em = tbm.rouletteEventManager;
        em.AddListener(ROULETTE_EVENT.ROULETTE_SPIN_END, this);
        StartCoroutine(GameLoop());
    }



    IEnumerator GameLoop()
    {
        while (true)
        {
            switch (state)
            {
                case RouletteGameState.WaitingPlayers:
                    yield return WaitingPlayersState();
                    break;
                case RouletteGameState.Bettring:
                    yield return BettingState();
                    break;
                case RouletteGameState.StatSpin:
                    yield return StartSpinState();
                    break;
                case RouletteGameState.RouletteWaiting:
                    yield return WaitingStopSpinState();
                    break;
            }
            yield return null;
        }
    }

    private void DebugLog(string str)
    {
        text.SetText(str);
        Debug.Log(str);
    }
    IEnumerator WaitingPlayersState()
    {
        while (tbm.plyers.ToList().Exists(p => p.ps.PlayerNick != ""))
        {

            if (currentTime == stateWaitTime)
                break;
            DebugLog("Waiting Players" + (stateWaitTime - currentTime).ToString());
            currentTime++;
            yield return new WaitForSeconds(sec);

        }
        if(!tbm.plyers.ToList().Exists(p => p.ps.PlayerNick != "")) {

            DebugLog("No players" + gameTime.ToString());
           
            gameTime++;
            yield return new WaitForSeconds(sec);
        }

        if (currentTime == stateWaitTime)
        {
            currentTime = 0;
            state = RouletteGameState.Bettring;
        }
    }

    IEnumerator BettingState()
    {
        while (currentTime != stateWaitTime)
        {

            
            DebugLog("Players betting " + (stateWaitTime - currentTime).ToString());
            currentTime++;
            yield return new WaitForSeconds(sec);

        }
       
        if (currentTime == stateWaitTime)
        {
            currentTime = 0;
            state = RouletteGameState.StatSpin;
        }
    }

    IEnumerator StartSpinState()
    {
        while (currentTime != stateWaitTime)
        {


            DebugLog("Players betting " + (stateWaitTime - currentTime).ToString());
            currentTime++;
            yield return new WaitForSeconds(sec);

        }
       
        if (currentTime == stateWaitTime)
        {
            currentTime = 0;
            tbm.StartSpinAllParts();
            state = RouletteGameState.RouletteWaiting;
        }
    }
    IEnumerator WaitingStopSpinState()
    {
        currentTime = 0;
        while (!rouletteStoped)
        {


            DebugLog("Waiting Spin End " + (currentTime).ToString());
            currentTime++;
            yield return new WaitForSeconds(sec);

        }

        currentTime = 0;
        state = RouletteGameState.WaitingPlayers;
        
    }

    public void OnEvent(ROULETTE_EVENT Event_type, Component Sender, params object[] Param)
    {
        switch (Event_type)
        {
            case ROULETTE_EVENT.ROULETTE_SPIN_END:
                rouletteStoped = true;
                break;
        }
    }
}
