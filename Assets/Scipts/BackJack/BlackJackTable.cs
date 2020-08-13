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
    //public enum JackPlayersEvets { PlayerSplited, PlayerSurrendered, PlayerTakedCard, PlayerSaved,
    //    PlayerCanSplit, PlayerCanSurrendered, PlayerCanTakeCard, PlayerCanSave, PlayerWin, PlayerLose }
    class BlackJackTable : MonoBehaviourPun/*, IListener<JackPlayersEvets>*/
    {
        BlackJackGameStates gameState = BlackJackGameStates.ResetGame;
        [SerializeField]
        TMP_Text tms;
        //public EventManager<JackPlayersEvets> gameEventManager;
        BlackJackLogic blackJackLogic;
        [SerializeField]
        private List<PlayerPlace> players;

        List<PlayerPlace> playersInGame;
        List<PlayerPlace> playersOutFromGame;


        [SerializeField]
        BlackJackPlayerCardField BlackJackDilerCardField;
       
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
            //gameEventManager = new EventManager<JackPlayersEvets>();

            //gameEventManager.AddListener(JackPlayersEvets.PlayerSaved, this);
            //gameEventManager.AddListener(JackPlayersEvets.PlayerSplited, this);
            //gameEventManager.AddListener(JackPlayersEvets.PlayerSurrendered, this);
            //gameEventManager.AddListener(JackPlayersEvets.PlayerTakedCard, this);

            playersOutFromGame = new List<PlayerPlace>();
            playersInGame = new List<PlayerPlace>();
            StartCoroutine(BlackJackLoop());

        }
        //bool playerSkipTurn
        void ResetGame()       
        {
            playersInGame.ForEach(p => {
                var fields = p.GetComponent<PlayerBlackJackFields>();
                fields.bettingField.ClearStacks();
                fields.blackJackField.ClearStacks();
            });
            playersOutFromGame.ForEach(p => {
                var fields = p.GetComponent<PlayerBlackJackFields>();
                fields.bettingField.ClearStacks();
                fields.blackJackField.ClearStacks();
            });

            lose = -99999;
            playerLose = false;
            playersInGame.Clear();
            playersOutFromGame.Clear();
            currWaitTime = 0;          
            gameState = BlackJackGameStates.CheckPlayer;

             
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
                        
                        tms.SetText("reset game in" + ((4) - i).ToString());
                        Debug.Log("reset game in" + ((4) - i).ToString());
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
                currWaitTime = 0;
                playerReady = false;
                playerLose = false;

                if (blackJackLogic.CanTakeCard(p.ps))
                {
                    ActivateGameButtons(false, true, true, false, p);
                    endTurns = false;
                    while (currWaitTime != waitTimeInSec)
                    {
                        if (playerReady)
                        {
                            ActivateGameButtons(false, false, false, false, p);
                            tms.SetText(p.ps.PlayerNick + " skiped turn");
                            Debug.Log(p.ps.PlayerNick + " skiped turn");
                            break;
                        }
                        else if (playerLose)
                        {
                            toRemove.Add(p);
                            Debug.Log(p.ps.PlayerNick + " Lose bet "+ lose + "so match points");
                            tms.SetText(p.ps.PlayerNick + " Lose bet " + lose + "so match points");
                            break;
                        }
                        else if (playerTakeCard)
                        {
                            Debug.Log(p.ps.PlayerNick + " take card");
                            tms.SetText(p.ps.PlayerNick + " take card");
                            break;
                        }
                        currWaitTime++;

                       
                    }  
                    
                    blackJackLogic.SkipTruns(p.ps);
                    ActivateGameButtons(false, false, false, false, p);

                    yield return new WaitForSeconds(OneSec);


                }
              
            }

            if (endTurns)
                gameState = BlackJackGameStates.CheckResults;
            if(playersInGame.Count == 0)
                gameState =  BlackJackGameStates.ResetGame;
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

        private IEnumerator WaitingForSitPlayers()
        {

           
            if (players.Exists(p => p.ps != null))
            {
                
                tms.SetText("Waiting players" + (waitTimeInSec - currWaitTime).ToString());
                yield return new WaitForSeconds(OneSec);
                currWaitTime++;

            }
            else
            {
                tms.SetText("Waiting players main cicle time= " + i);
            }
            if (currWaitTime == waitTimeInSec)
            {
                var result = players.FindAll(p => p.ps != null);
                if (result != null)
                {
                    
                    Debug.Log("to next state -> BlackJackGameStates.CardsToPlayers. players=" + result.Count);

                    playersInGame.AddRange(result);
                    //получаем стату игроков
                    List<PlayerStats> p_stats = new List<PlayerStats>();
                    playersInGame.ForEach(p => p_stats.Add(p.ps));
                    blackJackLogic = new BlackJackLogic(p_stats);

                    players.ForEach(p => ActivateGameButtons(false, false, false, false, p));

                    gameState = BlackJackGameStates.PlayersBetting;
                   
                }
                else currWaitTime = 0;
            }                
            
            yield return null;
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
                        tms.SetText("card to " + playersInGame[j].ps.PlayerNick);
                        Debug.Log("card to " + playersInGame[j].ps.PlayerNick + "curveID=" + playersInGame[j].PlaceId);

                        id = playersInGame[j].PlaceId;
                        nick = playersInGame[j].ps.PlayerNick;
                        card = blackJackLogic.bjPlayers[j].BlackJackStaks[0].cards[i];

                        cardCurveAnimator.StartAnimCardToPlayer(id, nick, card);

                    }

                    yield return new WaitForSeconds(3f);
                    Debug.Log("card to Diler curveindex=" + players.Count);

                    id = players.Count;
                    nick = "Diler";
                    card = blackJackLogic.diler.BlackJackStaks[0].cards[i];

                    tms.SetText("card to " + nick);
                    Debug.Log("card to " + nick);
                    cardCurveAnimator.StartAnimCardToPlayer(players.Count, nick, card);

                }

                //check black 
                var PlayerToRemove = new List<PlayerPlace>();
                foreach (var p in playersInGame)
                {
                    int win;
                    if (blackJackLogic.CheckBlackJack(p.ps, out win))
                    {
                        tms.SetText(p.ps.PlayerNick + " win _> " + win);
                        yield return new WaitForSeconds(1f);
                        PlayerToRemove.Add(p);
                    }
                }

                playersOutFromGame.AddRange(PlayerToRemove);
                PlayerToRemove.ForEach(p => playersInGame.Remove(p));


                gameState = BlackJackGameStates.PlayerTurn;
            }
            else gameState = BlackJackGameStates.ResetGame;

            yield return null;
        }

        private IEnumerator PlayersBetting()
        {
            List<PlayerPlace> toRemove = new List<PlayerPlace>();
            for (var j = 0; j < playersInGame.Count; j++)
            {
                currWaitTime = 0;
                ActivateGameButtons(false, false, false, true, playersInGame[j]);

                while (currWaitTime != waitTimeInSec)
                {
                    if (playerReady)
                        break;
                    currWaitTime++;
                    tms.SetText("Players "+ playersInGame[j].ps.PlayerNick + "is betting" + (waitTimeInSec - currWaitTime).ToString());
                    Debug.Log("Players " + playersInGame[j].ps.PlayerNick + "is betting" + (waitTimeInSec - currWaitTime).ToString());
                    yield return new WaitForSeconds(OneSec);

                }

                ActivateGameButtons(false, false, false, false, playersInGame[j]);

                var playerField = playersInGame[j].GetComponent<PlayerBlackJackFields>();

                var bet = 0;

                foreach (var chip in playerField.bettingField.Stacks[0].Objects)
                {
                    bet += (int)chip.GetComponent<ChipData>().Cost;
                }
                if (bet == 0)
                {
                    tms.SetText("Players " + playersInGame[j].ps.PlayerNick + "removed from game 0 bet");
                    Debug.Log("Players " + playersInGame[j].ps.PlayerNick + "removed from game 0 bet");

                    if (playersInGame[j].GetComponent<PlayerBlackJackFields>().bettingField.Stacks[0].Objects.Count == 0)
                        toRemove.Add(playersInGame[j]);
                }
                else {

                    var bjP = blackJackLogic.bjPlayers.Find(bjp => bjp.player.PlayerNick == playersInGame[j].ps.PlayerNick);
                    if (bjP != null)
                    {
                        bjP.BlackJackStaks[0].bet = bet;
                        tms.SetText("Players " + playersInGame[j].ps.PlayerNick + "beted -> " + bet);
                        Debug.Log("Players " + playersInGame[j].ps.PlayerNick + "beted -> " + bet);
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

            gameState = BlackJackGameStates.CardsToPlayers;
        }

        //public void OnEvent(JackPlayersEvets Event_type, Component Sender, params object[] Param)
        //{
        //    switch (Event_type)
        //    {
        //        case JackPlayersEvets.PlayerSaved:
        //            break;
        //        case JackPlayersEvets.PlayerSplited:
        //            break;
        //        case JackPlayersEvets.PlayerSurrendered:
        //            break;
        //        case JackPlayersEvets.PlayerTakedCard:
        //            break;
        //    }
        //}
       
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
            
        }

        IEnumerator GiveCardsToDealer()
        {
            blackJackLogic.DealerTakesCards();

            if (blackJackLogic.diler.BlackJackStaks[0].cards.Count > 2)
                for (var i = 2; i < blackJackLogic.diler.BlackJackStaks[0].cards.Count; i++)
                {
                    yield return new WaitForSeconds(OneSec);

                    var id = players.Count;
                    var nick = "Diler";
                    var card = blackJackLogic.diler.BlackJackStaks[0].cards[i];

                    tms.SetText("card to " + nick);
                    Debug.Log("card to " + nick);
                    cardCurveAnimator.StartAnimCardToPlayer(players.Count, nick, card);

                    
                }
        }
        IEnumerator CheckResults()
        {

            yield return GiveCardsToDealer();

            foreach (var p in playersInGame)
            {
                var win = 0;
                if (blackJackLogic.IsWinVersusDiler(p.ps, out win))
                {
                    tms.SetText("player" + p.ps.PlayerNick + " WIN! -> " + win + "$");
                    Debug.Log("player" + p.ps.PlayerNick + " WIN! -> " + win + "$");
                    var animator = p.GetComponentInChildren<PlayerWinAnimation>();
                    animator.StartAnimation(win, p.ps.PlayerNick);
                    yield return new WaitForSeconds(OneSec);
                }
                else {
                    tms.SetText("player" + p.ps.PlayerNick + " LOSE! -> " + win + "$");
                    Debug.Log("player" + p.ps.PlayerNick + " LOSE! -> " + win + "$");
                    yield return new WaitForSeconds(OneSec);
                }
            }
                
            gameState = BlackJackGameStates.ResetGame;
            yield return null;
        }
    }
}
