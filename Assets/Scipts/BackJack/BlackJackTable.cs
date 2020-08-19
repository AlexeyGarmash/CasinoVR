using Assets.Scipts.Animators;
using Assets.Scipts.BackJack.Buttons;
using Cards;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
namespace Assets.Scipts.BackJack
{
    public enum BlackJackGameStates{ CheckPlayer, CardsToPlayers, PlayersBetting, CheckResults, ResetGame, PlayerTurn }
   
    class BlackJackTable : MonoBehaviourPun
    {
        BlackJackGameStates gameState = BlackJackGameStates.ResetGame;
        [SerializeField]
        TMP_Text tms;
       
        BlackJackLogic blackJackLogic;
        [SerializeField]
        List<PlayerPlace> players;

        List<PlayerPlace> playersInGame;
        List<PlayerPlace> playersOutFromGame;


        [SerializeField]
        BlackJackPlayerCardField BlackJackDilerCardFieldOpen;
        [SerializeField]
        BlackJackPlayerCardField BlackJackDilerCardFieldHidden;

        [SerializeField]
        private CardCurveAnimator cardCurveAnimator;


        const int waitTimeInSec = 10;
        const int OneSec = 1;

        int currWaitTime = 0;
      
       
        bool playerTakeCard;
        bool playerLose;
        int lose;
        bool playerReady;

        private void Start()
        {

            playersOutFromGame = new List<PlayerPlace>();
            playersInGame = new List<PlayerPlace>();
            StartCoroutine(BlackJackLoop());

        }
        //bool playerSkipTurn
        void ResetGame()       
        {
           

            players.ForEach(p => { 
                var buttons = p.GetComponentInChildren<ButtonsHolder>();
                ActivateGameButtons(true, false, false, false, p);
                var fields = p.GetComponent<PlayerBlackJackFields>();
                fields.bettingField1.ClearStacks();
                fields.bettingField1.BlockField(false);
                fields.blackJackField1.ClearStacks();
                

            });

            BlackJackDilerCardFieldOpen.ClearStacks();
            BlackJackDilerCardFieldHidden.ClearStacks();

            lose = -99999;
            playerLose = false;
            playersInGame.Clear();
            playersOutFromGame.Clear();

            if (photonView.IsMine)
            {
                photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.CheckPlayer);
                photonView.RPC("SetZeroTimer_RPC", RpcTarget.All);
            }
                   
           

             
        }
        int i = 0;
        IEnumerator BlackJackLoop()
        {
            while (true)
            {
                
                i++;
                if (gameState == BlackJackGameStates.CheckPlayer)
                {
                    yield return WaitingForSitPlayers();
                }
                else if (gameState == BlackJackGameStates.PlayersBetting)
                {
                    yield return PlayersBetting();
                }                            
                else if (gameState == BlackJackGameStates.CardsToPlayers)
                {
                    yield return CardsToPlyers();
                }
                else if (gameState == BlackJackGameStates.CheckResults)
                {
                    yield return CheckResults();
                }
                else if (gameState == BlackJackGameStates.PlayerTurn)
                {
                    yield return PlayersTurns();
                }
                else if (gameState == BlackJackGameStates.ResetGame)
                {
                    yield return new WaitForSeconds(OneSec);
                    for (var i = 0; i < 5; i++)
                    {

                        DebugLog("reset game in" + ((4) - i).ToString());
                     
                        yield return new WaitForSeconds(OneSec);
                    }
                    
                    ResetGame();
                }
                               
            }
        }

        private IEnumerator PlayersTurns()
        {

            var toRemove = new List<PlayerPlace>();
            bool endTurns = true;

            
            foreach (var p in playersInGame)
            {


                if (blackJackLogic.CanTakeCard(p.ps))
                {
                    ActivateGameButtons(false, true, true, false, p);
                    endTurns = false;
                    if (photonView.IsMine)
                    {                      
                        photonView.RPC("SetZeroTimer_RPC", RpcTarget.All);
                    }
                    playerReady = false;
                    playerLose = false;
                    playerTakeCard = false;

                    while (currWaitTime != waitTimeInSec)
                    {
                        DebugLog(p.ps.PlayerNick + " turn time ->" + (waitTimeInSec - currWaitTime).ToString());
                      
                        if (playerReady)
                        {
                            ActivateGameButtons(false, false, false, false, p);
                            DebugLog(p.ps.PlayerNick + " skiped turn");
                          
                            break;
                        }
                        else if (playerLose)
                        {
                            toRemove.Add(p);
                            DebugLog(p.ps.PlayerNick + " Lose bet " + lose + "so match points");
                           
                            ActivateGameButtons(false, false, false, false, p);
                            break;
                        }
                        else if (playerTakeCard)
                        {
                          
                            DebugLog(p.ps.PlayerNick + " take card");
                           
                            ActivateGameButtons(false, false, false, false, p);
                            break;


                        }

                        if (photonView.IsMine)
                            photonView.RPC("TimerStep_RPC", RpcTarget.All);

                        yield return new WaitForSeconds(OneSec);

                    }
                    ActivateGameButtons(false, false, false, false, p);
                    if (currWaitTime == waitTimeInSec)
                    {
                        
                        blackJackLogic.SkipTruns(p.ps);
                    }
                        

                    yield return new WaitForSeconds(OneSec);


                }

            }
            

            if (endTurns && photonView.IsMine)
                photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.CheckResults);
            if (playersInGame.Count == 0 && photonView.IsMine)
                photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.ResetGame);
            yield return null;
            playersOutFromGame.AddRange(toRemove);
            toRemove.ForEach(p => playersInGame.Remove(p));
        }
        void ActivateGameButtons(bool activateTakePlaceButton, bool activateButtonTakeCard,
            bool activateButtonSkip, bool activateButtonReady, PlayerPlace pp)
        {
            if (pp != null)
            {
                var buttonsHolder = pp.GetComponentInChildren<ButtonsHolder>();

                buttonsHolder.takePlace.gameObject.SetActive(activateTakePlaceButton);
                buttonsHolder.takeCard.gameObject.SetActive(activateButtonTakeCard);
                buttonsHolder.Ready.gameObject.SetActive(activateButtonReady);
                buttonsHolder.Skip.gameObject.SetActive(activateButtonSkip);

            }
        }
        [PunRPC]
        public void TimerStep_RPC()
        {
            currWaitTime++;
        }
        [PunRPC]
        public void SetZeroTimer_RPC()
        {
            currWaitTime = 0;
        }

        [PunRPC]
        public void State_RPC(int state)
        {
            gameState = (BlackJackGameStates)state;
        }


        private IEnumerator WaitingForSitPlayers()
        {

           
            if (players.Exists(p => p.PlayerOnPlace))
            {
                
                DebugLog("Waiting players" + (waitTimeInSec - currWaitTime).ToString());
                yield return new WaitForSeconds(OneSec);

                if (photonView.IsMine)
                    photonView.RPC("TimerStep_RPC", RpcTarget.All);

            }
            else
            {
                DebugLog("no one players waiting=" + i.ToString());
                yield return new WaitForSeconds(OneSec);
            }
            if (currWaitTime == waitTimeInSec)
            {
                var result = players.FindAll(p => p.PlayerOnPlace);
                if (result != null)
                {

                    Debug.Log("to next state -> BlackJackGameStates.PlayersBetting | players=" + result.Count);

                    playersInGame.AddRange(result);
                    //получаем стату игроков



                    yield return new WaitForSeconds(0.1f);


                    if (photonView.IsMine)
                    {
                        DeckData dd = new DeckData();
                        
                        var indexesRnd = dd.GenerateDeck();
                        photonView.RPC("SetDeck", RpcTarget.All, indexesRnd);
                        photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.PlayersBetting);
                    }

                    
                        

                    players.ForEach(p => ActivateGameButtons(false, false, false, false, p));


                }
                else if (photonView.IsMine)
                {
                   
                    photonView.RPC("SetZeroTimer_RPC", RpcTarget.All);
                }
            }
                yield return null;
        }

        [PunRPC]
        protected void SetDeck(int[] indexes)
        {
            List<PlayerStats> p_stats = new List<PlayerStats>();
            playersInGame.ForEach(p => p_stats.Add(p.ps));
            blackJackLogic = new BlackJackLogic(p_stats, indexes);
        }

        private IEnumerator CardsToPlyers()
        {
            int id;
            string nick;
            CardData card;

            if (playersInGame.Count != 0)
            {
                for (var i = 0; i < 2; i++)
                {
                    for (var j = 0; j < playersInGame.Count; j++)
                    {
                        yield return new WaitForSeconds(3f);
                       
                       

                        id = playersInGame[j].PlaceId;
                        nick = playersInGame[j].ps.PlayerNick;
                        card = blackJackLogic.bjPlayers[j].BlackJackStaks[0].cards[i];
                        DebugLog("card to " + playersInGame[j].ps.PlayerNick + " cardID" + card);
                        cardCurveAnimator.StartAnimCardToPlayer(id, nick, card);

                    }

                    yield return new WaitForSeconds(3f);                  

                    id = players.Count;
                    nick = "Diler";
                    card = blackJackLogic.diler.BlackJackStaks[0].cards[i];

                    if (i == 0)
                    {
                        BlackJackDilerCardFieldHidden.BlockField(true);
                        BlackJackDilerCardFieldOpen.BlockField(false);
                    }
                    else
                    {
                        BlackJackDilerCardFieldHidden.BlockField(false);
                        BlackJackDilerCardFieldOpen.BlockField(true);
                    }

                    DebugLog("card to " + nick + " cardID" + card);
                 
                    cardCurveAnimator.StartAnimCardToPlayer(players.Count, nick, card);

                }

                //check black 
                var PlayerToRemove = new List<PlayerPlace>();
                foreach (var p in playersInGame)
                {
                    int win;
                    if (blackJackLogic.CheckBlackJack(p.ps, out win))
                    {
                        DebugLog(p.ps.PlayerNick + " win _> " + win);
                       
                        yield return new WaitForSeconds(1f);
                        PlayerToRemove.Add(p);
                    }
                }

                playersOutFromGame.AddRange(PlayerToRemove);
                PlayerToRemove.ForEach(p => playersInGame.Remove(p));

                if (photonView.IsMine)
                    photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.PlayerTurn);

            }
            else if(photonView.IsMine)
                    photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.ResetGame);
            

            yield return null;
        }

        private void DebugLog(string text)
        {
            Debug.Log(text);
            tms.SetText(text);
        }

        private IEnumerator PlayersBetting()
        {
            List<PlayerPlace> toRemove = new List<PlayerPlace>();
            for (var j = 0; j < playersInGame.Count; j++)
            {
                if (photonView.IsMine)
                {                  
                    photonView.RPC("SetZeroTimer_RPC", RpcTarget.All);
                }

                ActivateGameButtons(false, false, false, true, playersInGame[j]);

                while (currWaitTime != waitTimeInSec)
                {
                    if (playerReady)
                        break;

                    if (photonView.IsMine)
                        photonView.RPC("TimerStep_RPC", RpcTarget.All);

                    DebugLog("Players " + playersInGame[j].ps.PlayerNick + "is betting" + (waitTimeInSec - currWaitTime).ToString());
                   
                    yield return new WaitForSeconds(OneSec);

                }

                ActivateGameButtons(false, false, false, false, playersInGame[j]);

                var playerField = playersInGame[j].GetComponent<PlayerBlackJackFields>();

                var bet = 0;

                foreach (var chip in playerField.bettingField1.Stacks[0].Objects)
                {
                    bet += (int)chip.GetComponent<ChipData>().Cost;                                     
                }
                playerField.bettingField1.BlockField(true);

                if (bet == 0)
                {
                    DebugLog("Players " + playersInGame[j].ps.PlayerNick + "removed from game 0 bet");
                  

                    if (playersInGame[j].GetComponent<PlayerBlackJackFields>().bettingField1.Stacks[0].Objects.Count == 0)
                        toRemove.Add(playersInGame[j]);
                }
                else {

                    DebugLog("in game " + playersInGame[j].ps.PlayerNick);
                    var bjP = blackJackLogic.bjPlayers.FirstOrDefault(bjp => bjp.player.PlayerNick == playersInGame[j].ps.PlayerNick);
                   


                    blackJackLogic.bjPlayers.ForEach(p => DebugLog("player in game logic " + p.player.PlayerNick));

                    
                    if (bjP != null)
                    {
                        bjP.BlackJackStaks[0].bet = bet;
                        DebugLog("Players " + playersInGame[j].ps.PlayerNick + "beted -> " + bet);
                       
                    }
                    else Debug.LogError("bjPlayers not found");

                }

            }

            //выкинуть игрока со стола
            playersOutFromGame.AddRange(toRemove);
            toRemove.ForEach(r =>
            {
                playersInGame.Remove(r);              
            });

            if(photonView.IsMine)
                photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.CardsToPlayers);
           
        }
       
        public void SkipTurn(PlayerStats player)
        {
            if (blackJackLogic.SkipTruns(player))
            {
                playerReady = true;
                ActivateGameButtons(false, false, false, false, players.Find(p => p.ps == player));
            }
        }
      
        public void TakeCard(PlayerStats player)
        {

            if (blackJackLogic.TakeCard(player))
            {
                
                
                if (blackJackLogic.IsPlunk(player, out lose))
                {

                    playerLose = true;
                    ActivateGameButtons(false, false, false, false, players.Find(p => p.ps == player));
                }

                playerTakeCard = true;

                var playerPlace = playersInGame.Find(p => p.ps == player);
                var id = playerPlace.PlaceId;
                var nick = playerPlace.ps.PlayerNick;

                var bjPlayer = blackJackLogic.bjPlayers.Find(bjP => bjP.player == player);
                var card = bjPlayer.BlackJackStaks[0].cards[bjPlayer.BlackJackStaks[0].cards.Count - 1];

                cardCurveAnimator.StartAnimCardToPlayer(id, nick, card);
            }
        }
        public void PlayerReady(PlayerStats player)
        {
            playerReady = true;
            ActivateGameButtons(false, false, false, false, players.Find(p => p.ps == player));
        }

        IEnumerator GiveCardsToDealer()
        {
            BlackJackDilerCardFieldHidden.BlockField(false);
            BlackJackDilerCardFieldHidden.ExtractAllObjects();
            BlackJackDilerCardFieldHidden.BlockField(true);
            blackJackLogic.DealerTakesCards();

            BlackJackDilerCardFieldOpen.BlockField(false);

            yield return new WaitForSeconds(OneSec);
            if (blackJackLogic.diler.BlackJackStaks[0].cards.Count > 2)
                for (var i = 2; i < blackJackLogic.diler.BlackJackStaks[0].cards.Count; i++)
                {
                    

                    var id = players.Count;
                    var nick = "Diler";
                    var card = blackJackLogic.diler.BlackJackStaks[0].cards[i];

                    tms.SetText("card to " + nick);
                    Debug.Log("card to " + nick);
                    cardCurveAnimator.StartAnimCardToPlayer(players.Count, nick, card);

                    yield return new WaitForSeconds(OneSec);


                }

            yield return new WaitForSeconds(5f);

        }
        IEnumerator CheckResults()
        {

            yield return GiveCardsToDealer();

            foreach (var p in playersInGame)
            {
                var win = 0;
                if (blackJackLogic.IsWinVersusDiler(p.ps, out win))
                {
                    DebugLog("player" + p.ps.PlayerNick + " WIN! -> " + win + "$");
                  
                    var animator = p.GetComponentInChildren<PlayerWinAnimation>();
                    animator.StartAnimation(win*2, p.ps.PlayerNick);
                    yield return new WaitForSeconds(OneSec);
                }
                else {
                    DebugLog("player" + p.ps.PlayerNick + " LOSE! -> " + win + "$");
                  
                    yield return new WaitForSeconds(OneSec);
                }
            }

            if (photonView.IsMine)
                photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.ResetGame);

            yield return null;
        }
    }
}
