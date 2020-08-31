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
    public enum BlackJackGameStates { CheckPlayer, CardsToPlayers, PlayersBetting, CheckResults, ResetGame, PlayerTurn }

    class BlackJackTable : MonoBehaviourPun
    {
        BlackJackGameStates gameState = BlackJackGameStates.ResetGame;

        [SerializeField]
        CroupierBlackJackNPC bjNPC;

        private WinLoseAudioPlayer bjNPCWinLosePlayer;
        [SerializeField]
        TMP_Text tms;

        BlackJackLogic blackJackLogic;
        [SerializeField]
        List<PlayerPlace> players;

        List<PlayerPlace> playersInGame;
        List<PlayerPlace> playersOutFromGame;


       

        [SerializeField]
        private CardCurveAnimator cardCurveAnimator;


        const int waitTimeInSec = 10;
        const int OneSec = 1;


        const string giveMeCard = "give me card";
        const string skip = "skip";
        const string giveUp = "give up";
        const string split = "split";
        const string save = "save";
        const string doubleBet = "double bet";

        int currWaitTime = 0;


        bool playerTakeCard;
        bool playerLose;
        private bool needSplit;

        bool playerReady;

        int lose;
        
        int i = 0;
        int currentBJStackIndex;

        CardData takedCard;

        private void Start()
        {

            bjNPCWinLosePlayer = bjNPC.GetComponent<WinLoseAudioPlayer>();
            playersOutFromGame = new List<PlayerPlace>();
            playersInGame = new List<PlayerPlace>();
            StartCoroutine(BlackJackLoop());

        }
        //bool playerSkipTurn
        void ResetGame()
        {
            playerReady = false;
            playerTakeCard = false;
            playerLose = false;
            bjNPC.ResetValues();
            
            players.ForEach(p => {
                var buttons = p.GetComponentInChildren<ButtonsHolder>();
                ActivateGameButtons(true, false, false, false, p);
                var fields = p.GetComponent<PlayerBlackJackFields>();

                fields.bettingField.ClearStacks();
                fields.bettingField.BlockField(false);

                fields.bettingFieldForSplit.BlockField(true);
                fields.bettingFieldForSplit.ClearStacks();

                fields.blackJackCardField.ClearStacks();
                fields.blackJackCardField.BlockField(false);

                fields.blackJackCardFieldForSplit.BlockField(true);
                fields.blackJackCardFieldForSplit.ClearStacks();


            });

            

            lose = -99999;
            playerLose = false;
            playersInGame.Clear();
            playersOutFromGame.Clear();

            if (photonView.IsMine)
            {
                photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.CheckPlayer);
                photonView.RPC("SetZeroTimer_RPC", RpcTarget.All);
                PhotonNetwork.SendAllOutgoingCommands();
               
            }

        }
       

        IEnumerator BlackJackLoop()
        {
            while (true)
            {

                i++;
                switch (gameState)
                {
                    case BlackJackGameStates.CheckPlayer:
                        yield return WaitingForSitPlayers();
                        break;
                    case BlackJackGameStates.PlayersBetting:
                        yield return PlayersBetting();
                        break;
                    case BlackJackGameStates.CardsToPlayers:
                        yield return CardsToPlyers();
                        break;
                    case BlackJackGameStates.PlayerTurn:
                        yield return PlayersTurns();
                        break;
                    case BlackJackGameStates.CheckResults:
                        yield return CheckResults();
                        break;
                    case BlackJackGameStates.ResetGame:
                        
                        for (var i = 0; i < 5; i++)
                        {

                            DebugLog("reset game in" + ((4) - i).ToString());

                            yield return new WaitForSeconds(OneSec);
                        }

                        ResetGame();
                        break;
                }


            }
        }

        private IEnumerator PlayersTurns()
        {

            var toRemove = new List<PlayerPlace>();
            bool endTurns = true;


            foreach (var p in playersInGame)
            {
                currentBJStackIndex = 0;
                var bjPlayer = blackJackLogic.bjPlayers.Find(bjp => bjp.player.PlayerNick == p.ps.PlayerNick);

                if (bjPlayer != null)
                {
                    for (var j = 0; j <  bjPlayer.BlackJackStaks.Count; j++)
                    {
                        currentBJStackIndex = j;
                        if (blackJackLogic.CanTakeCard(p.ps.PlayerNick, j))
                        {

                            
                            //if (blackJackLogic.CanSplit(p.ps.PlayerNick))
                            ActivateGameButtons(false, true, true, false, p, true);
                            //else ActivateGameButtons(false, true, true, false, p);
                            var recognizer = p.GetComponent<VoiceManager>();

                            //recognizer.AddVoiceAction(skip, SkipTurn);
                            //recognizer.AddVoiceAction(giveMeCard, TakeCard);
                            //recognizer.StartRecognize();

                            endTurns = false;

                            if (photonView.IsMine)
                            {
                                photonView.RPC("SetZeroTimer_RPC", RpcTarget.All);
                                PhotonNetwork.SendAllOutgoingCommands();
                            }

                            playerReady = false;
                            playerLose = false;
                            playerTakeCard = false;
                            needSplit = false;
                            while (currWaitTime != waitTimeInSec)
                            {
                                DebugLog(p.ps.PlayerNick + " turn time ->" + (waitTimeInSec - currWaitTime).ToString());
                                if (playerLose)
                                {
                                    //toRemove.Add(p);
                                    DebugLog(p.ps.PlayerNick + " Lose bet " + lose + "so match points");

                                    break;
                                }
                                else if (playerTakeCard)
                                {
                                    break;
                                }

                                else if (playerReady)
                                {

                                    DebugLog(p.ps.PlayerNick + " skiped turn");

                                    break;
                                }

                                if (needSplit)
                                {
                                    DebugLog(p.ps.PlayerNick + " splited");
                                    break;



                                }



                                if (photonView.IsMine)
                                {
                                    photonView.RPC("TimerStep_RPC", RpcTarget.All);
                                    PhotonNetwork.SendAllOutgoingCommands();
                                }
                                yield return new WaitForSeconds(OneSec);

                            }

                            ActivateGameButtons(false, false, false, false, p);
                            var field = p.GetComponent<PlayerBlackJackFields>();

                            if (playerTakeCard)
                            {
                                if (currentBJStackIndex == 0)
                                {
                                    field.blackJackCardFieldForSplit.BlockField(true);
                                    field.blackJackCardField.BlockField(false);

                                   
                                }
                                else if (currentBJStackIndex == 1)
                                {
                                    field.blackJackCardFieldForSplit.BlockField(false);
                                    field.blackJackCardField.BlockField(true);

                                    
                                }

                                bjNPC.AddCardToHand(p.PlaceId, takedCard);

                                yield return bjNPC.TakeCardsToPlayers(false);

                                takedCard = null;

                                playerTakeCard = false;
                            }

                            else if (needSplit)
                            {
                                blackJackLogic.UseSplit(p.ps.PlayerNick);

                               
                                //добавляем ставку для сплита
                                foreach (var chip in field.bettingField.Stacks[0].Objects)
                                {
                                    GameObject extractedChip;
                                    if (p.ExtractChipByCost(chip.GetComponent<ChipData>().Cost, out extractedChip))
                                    {
                                        field.bettingFieldForSplit.MagnetizeObject(extractedChip, field.bettingFieldForSplit.Stacks[0]);
                                    }
                                }
                                //разделяем карты
                                var player = blackJackLogic.bjPlayers.Find(bjp => p.ps.PlayerNick == bjp.player.PlayerNick);
                                var card = field.blackJackCardField.ExtractObject(player.BlackJackStaks[1].cards[0]);
                                field.blackJackCardFieldForSplit.MagnetizeObject(card, field.blackJackCardFieldForSplit.Stacks[0]);

                                //добавляем по 2 карыт

                                for (var k = 0; k < 2; k++)
                                {
                                    if (k == 0)
                                    {
                                        field.blackJackCardFieldForSplit.BlockField(true);
                                        field.blackJackCardField.BlockField(false);

                                        bjNPC.AddCardToHand(p.PlaceId, player.BlackJackStaks[0].cards[1]);
                                    }
                                    else if (k == 1)
                                    {
                                        field.blackJackCardFieldForSplit.BlockField(false);
                                        field.blackJackCardField.BlockField(true);

                                        bjNPC.AddCardToHand(p.PlaceId, player.BlackJackStaks[1].cards[1]);
                                    }

                                    yield return bjNPC.TakeCardsToPlayers(false);

                                }
                                break;

                            }


                           // recognizer.StopRecognize();


                            yield return new WaitForSeconds(OneSec * 3);

                            if (currWaitTime == waitTimeInSec)
                            {

                                blackJackLogic.SkipTruns(p.ps.PlayerNick, j);
                            }


                            yield return new WaitForSeconds(OneSec);


                        }
                        currentBJStackIndex = 0;
                    }
                }
            }

            //yield return WaitDistributionOfCards();


            if (endTurns)
            {
                if (photonView.IsMine)
                {
                    photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.CheckResults);
                    PhotonNetwork.SendAllOutgoingCommands();
                }
                else {
                    while (BlackJackGameStates.CheckResults != gameState)
                        yield return null;
                }
            }

            if (playersInGame.Count == 0)
            {
                if (photonView.IsMine)
                {
                    photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.ResetGame);
                    PhotonNetwork.SendAllOutgoingCommands();
                }
                else
                {
                    while (BlackJackGameStates.ResetGame != gameState)
                        yield return null;
                }
            }
            yield return null;
            playersOutFromGame.AddRange(toRemove);
            toRemove.ForEach(p => playersInGame.Remove(p));
        }
        void ActivateGameButtons(bool activateTakePlaceButton, bool activateButtonTakeCard,
            bool activateButtonSkip, bool activateButtonReady, PlayerPlace pp, bool activateSplitButton = false)
        {
            if (pp != null)
            {
                var buttonsHolder = pp.GetComponentInChildren<ButtonsHolder>();

                buttonsHolder.takePlace.gameObject.SetActive(activateTakePlaceButton);
                buttonsHolder.takeCard.gameObject.SetActive(activateButtonTakeCard);
                buttonsHolder.Ready.gameObject.SetActive(activateButtonReady);
                buttonsHolder.Skip.gameObject.SetActive(activateButtonSkip);
                buttonsHolder.Split.gameObject.SetActive(activateSplitButton);

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
            Debug.Log("to state State_RPC" + (BlackJackGameStates)state);
            gameState = (BlackJackGameStates)state;
        }

        [PunRPC]
        public void FindPlayers()
        {
            playersInGame = players.FindAll(p => p.PlayerOnPlace);
        }
        private IEnumerator WaitingForSitPlayers()
        {


            if (players.Exists(p => p.PlayerOnPlace))
            {

                DebugLog("Waiting players" + (waitTimeInSec - currWaitTime).ToString());
                yield return new WaitForSeconds(OneSec);

                if (photonView.IsMine)
                {
                    photonView.RPC("TimerStep_RPC", RpcTarget.All);
                    PhotonNetwork.SendAllOutgoingCommands();
                }

            }
            else
            {
                DebugLog("no one players waiting=" + i.ToString());
                yield return new WaitForSeconds(OneSec);
            }

            if (currWaitTime == waitTimeInSec)
            {
                if (photonView.IsMine)
                {
                    photonView.RPC("FindPlayers", RpcTarget.All);
                    PhotonNetwork.SendAllOutgoingCommands();
                }

                if (playersInGame.Count != 0)
                {

                    Debug.Log("to next state -> BlackJackGameStates.PlayersBetting | players=" + playersInGame.Count);

                    //получаем стату игроков

                    if (photonView.IsMine)
                    {
                        yield return new WaitForSeconds(0.1f);
                        DeckData dd = new DeckData();

                        var indexesRnd = dd.GenerateDeck();
                        photonView.RPC("SetDeck", RpcTarget.All, indexesRnd);
                        photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.PlayersBetting);
                        PhotonNetwork.SendAllOutgoingCommands();

                    }

                    while (BlackJackGameStates.PlayersBetting != gameState)
                        yield return null;

                    players.ForEach(p => ActivateGameButtons(false, false, false, false, p));

                }
                else if (photonView.IsMine)
                {
                    photonView.RPC("SetZeroTimer_RPC", RpcTarget.All);
                    PhotonNetwork.SendAllOutgoingCommands();
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

            DebugLog("Players in game" + playersInGame.Count);


            if (playersInGame.Count != 0)
            {
                for (var i = 0; i < 2; i++)
                {
                    for (var j = 0; j < playersInGame.Count; j++)
                    {



                        id = playersInGame[j].PlaceId;
                        nick = playersInGame[j].ps.PlayerNick;
                        card = blackJackLogic.bjPlayers[j].BlackJackStaks[0].cards[i];
                       

                        bjNPC.AddCardToHand(id, card);
                        //cardCurveAnimator.StartAnimCardToPlayerWithInstantiate(id, nick, card);

                    }

                    //yield return new WaitForSeconds(3f);

                    id = players.Count;
                    nick = "Diler";
                    card = blackJackLogic.diler.BlackJackStaks[0].cards[i];
                    bjNPC.AddCardToHand(2, card);

                    DebugLog("card to " + nick + " card face = " + card.Face + " card sign=  " + card.Sign);


                    // cardCurveAnimator.StartAnimCardToPlayerWithInstantiate(players.Count, nick, card);

                }

                //check black 
                var PlayerToRemove = new List<PlayerPlace>();
                foreach (var p in playersInGame)
                {
                    int win;
                    if (blackJackLogic.CheckBlackJack(p.ps.PlayerNick, out win, 0))
                    {
                        DebugLog(p.ps.PlayerNick + " win _> " + win);

                        yield return new WaitForSeconds(1f);
                        PlayerToRemove.Add(p);
                    }
                }

                playersOutFromGame.AddRange(PlayerToRemove);
                PlayerToRemove.ForEach(p => playersInGame.Remove(p));



                yield return bjNPC.TakeCardsToPlayers(true);


                yield return WaitDistributionOfCards();

                if (photonView.IsMine)
                {
                    photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.PlayerTurn);
                    PhotonNetwork.SendAllOutgoingCommands();
                }

                while (BlackJackGameStates.PlayerTurn != gameState)
                    yield return null;
            }
            else 
            {
                if (photonView.IsMine)
                {
                    photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.ResetGame);
                    PhotonNetwork.SendAllOutgoingCommands();
                }

                while (BlackJackGameStates.ResetGame != gameState)
                    yield return null;
            }


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
                    PhotonNetwork.SendAllOutgoingCommands();
                }

                ActivateGameButtons(false, false, false, true, playersInGame[j]);
                var voiceRecognizer = playersInGame[j].GetComponent<VoiceManager>();
                voiceRecognizer.AddVoiceAction(skip, PlayerReady);
                voiceRecognizer.StartRecognize();
                while (currWaitTime != waitTimeInSec)
                {
                    if (playerReady)
                        break;

                    if (photonView.IsMine)
                    {
                        photonView.RPC("TimerStep_RPC", RpcTarget.All);
                        PhotonNetwork.SendAllOutgoingCommands();
                    }

                    DebugLog("Players " + playersInGame[j].ps.PlayerNick + "is betting" + (waitTimeInSec - currWaitTime).ToString());

                    yield return new WaitForSeconds(OneSec * 2);

                }

                voiceRecognizer.StopRecognize();
                ActivateGameButtons(false, false, false, false, playersInGame[j]);

                var playerField = playersInGame[j].GetComponent<PlayerBlackJackFields>();

                var bet = 0;

                foreach (var chip in playerField.bettingField.Stacks[0].Objects)
                {
                    bet += (int)chip.GetComponent<ChipData>().Cost;
                }
                playerField.bettingField.BlockField(true);

                if (bet == 0)
                {
                    DebugLog("Players " + playersInGame[j].ps.PlayerNick + "removed from game 0 bet");


                    if (playersInGame[j].GetComponent<PlayerBlackJackFields>().bettingField.Stacks[0].Objects.Count == 0)
                        toRemove.Add(playersInGame[j]);
                }
                else
                {

                    DebugLog("in game " + playersInGame[j].ps.PlayerNick);
                    



                    blackJackLogic.bjPlayers.ForEach(p => DebugLog("player in game logic " + p.player.PlayerNick));


                    if (blackJackLogic.bjPlayers.Exists(bjp => bjp.player.PlayerNick.Equals(playersInGame[j].ps.PlayerNick)))
                    {
                        var bjP = blackJackLogic.bjPlayers.Find(bjp => bjp.player.PlayerNick.Equals(playersInGame[j].ps.PlayerNick));
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

            if (photonView.IsMine)
            {
                photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.CardsToPlayers);
                PhotonNetwork.SendAllOutgoingCommands();
            }
            else
            {
                while (gameState != BlackJackGameStates.CardsToPlayers)
                {
                    yield return null;
                }
            }

        }

        


       

        IEnumerator WaitDistributionOfCards()
        {
            while (bjNPC.distributionOfCards)
                yield return null;
        }

        

        IEnumerator GiveCardsToDealer()
        {
            bjNPC.BlackJackDilerCardFieldHidden.BlockField(false);
            bjNPC.BlackJackDilerCardFieldHidden.ExtractAllObjects();
            bjNPC.BlackJackDilerCardFieldHidden.BlockField(true);
            blackJackLogic.DealerTakesCards();

            bjNPC.BlackJackDilerCardFieldOpen.BlockField(false);

            yield return new WaitForSeconds(OneSec * 2);
            if (blackJackLogic.diler.BlackJackStaks[0].cards.Count > 2)
            {
                for (var i = 2; i < blackJackLogic.diler.BlackJackStaks[0].cards.Count; i++)
                {


                    var idCurve = players.Count;
                    var nick = "Diler";
                    var card = blackJackLogic.diler.BlackJackStaks[0].cards[i];
                    bjNPC.AddCardToHand(2, card);

                    DebugLog("card to " + nick + " card face = " + card.Face + " card sign=  " + card.Sign);


                }

                yield return bjNPC.TakeCardsToPlayers(false);

                yield return WaitDistributionOfCards();
            }

            yield return new WaitForSeconds(3f);

        }
        IEnumerator CheckResults()
        {

            yield return GiveCardsToDealer();      

            var winners = new List<PlayerPlace>();
            var winNumbers = new Dictionary<PlayerPlace, int>();

            foreach (var p in playersInGame)
            {
                var win = 0;
                var bet = 0;
                var pointsPlayer = 0;
                var pointsDiller = 0;

                var bjPlayer = blackJackLogic.bjPlayers.FirstOrDefault(bj => bj.player.PlayerNick == p.ps.PlayerNick);

                int winSum = 0;

                bool winner = false;
                for (var j = 0; j < bjPlayer.BlackJackStaks.Count; j++)
                {
                    if (blackJackLogic.IsWinVersusDiler(p.ps.PlayerNick, j , out win, out bet, out pointsPlayer, out pointsDiller))
                    {
                        winner = true;
                        winSum += win + bet;
                       
                        DebugLog("player " + pointsPlayer + " vs diller " + pointsDiller + "  " + p.ps.PlayerNick + " WIN! -> " + win + "$");

                    }
                    else
                    {
                        DebugLog("player " + pointsPlayer + " vs diller " + pointsDiller + "  " + p.ps.PlayerNick + " LOSE! -> " + win + "$");

                    }
                    yield return new WaitForSeconds(3f);
                }

                if (winner)
                {
                    winners.Add(p);
                    winNumbers.Add(p, winSum);
                }
               
            }

               
            

            if (winners.Count > 0)
            {
                bjNPCWinLosePlayer.winPlayer.PlayRandomClip();

               
             
                bjNPC.WinAnimation();

                yield return bjNPC.StartRandomWinDance();

                yield return new WaitForSeconds(0.1f);

                if (bjNPC.winDanceBehavour.currentPercentage >= 1)
                    bjNPC.winDanceBehavour.currentPercentage = 0;

                while (bjNPC.winDanceBehavour.currentPercentage < 0.9f)
                {
                    yield return null;
                }
                List<PlayerWinAnimation> animations = new List<PlayerWinAnimation>();

                winners.ForEach(w =>
                {
                    var animation = w.GetComponentInChildren<PlayerWinAnimation>();                  
                    animation.StartAnimation(winNumbers[w], w.ps.PlayerNick);

                    animations.Add(animation);
                });

                while (animations.Exists(a => a.animStarted))
                {
                    yield return null;
                }


                                            
                bjNPC.EndDanceAnimation();

                bjNPCWinLosePlayer.winPlayer.StopPlaing();
            }
            else {

                bjNPCWinLosePlayer.losePlayer.PlayRandomClip();

               
                bjNPC.LoseAnimation();

                bjNPC.EndDanceAnimation();
                bjNPC.loseDanceBehavour.currentPercentage = 0;

                while (bjNPC.loseDanceBehavour.currentPercentage < 0.9)
                    yield return null;

                bjNPCWinLosePlayer.losePlayer.StopPlaing();
            }

            if (photonView.IsMine)
            {
                photonView.RPC("State_RPC", RpcTarget.All, (int)BlackJackGameStates.ResetGame);
                PhotonNetwork.SendAllOutgoingCommands();
            }

            while (BlackJackGameStates.ResetGame != gameState)
                yield return null;
           
            yield return null;
        }
        #region Buttons Functions
            #region RPCs
            [PunRPC]
            private void PlayerReady_RPC(string player)
            {
                playerReady = true;
                ActivateGameButtons(false, false, false, false, players.Find(p => p.ps.PlayerNick == player));
            }
            [PunRPC]
            private void TakeCard_RPC(string player)
            {
                if (!playerTakeCard)
                {
                    if (blackJackLogic.TakeCard(player, currentBJStackIndex))
                    {


                        if (blackJackLogic.IsPlunk(player, out lose, currentBJStackIndex))
                        {

                            playerLose = true;
                            ActivateGameButtons(false, false, false, false, players.Find(p => p.ps.PlayerNick == player));
                        }

                        playerTakeCard = true;

                        var playerPlace = playersInGame.Find(p => p.ps.PlayerNick == player);
                        var id = playerPlace.PlaceId;
                        var nick = playerPlace.ps.PlayerNick;

                        var bjPlayer = blackJackLogic.bjPlayers.Find(bjP => bjP.player.PlayerNick == nick);
                        takedCard = bjPlayer.BlackJackStaks[currentBJStackIndex].cards[bjPlayer.BlackJackStaks[currentBJStackIndex].cards.Count - 1];

                        DebugLog("Give me card button ->  card to " + player + " card face = " + takedCard.Face + " card sign=  " + takedCard.Sign);





                        //if (photonView.IsMine)
                        //    cardCurveAnimator.StartAnimCardToPlayerWithInstantiate(id, nick, card);
                    }
                }
            }
            [PunRPC]
            private void SkipTurn_RPC(string nick)
            {
                if (blackJackLogic.SkipTruns(nick, currentBJStackIndex))
                {
                    playerReady = true;
                    ActivateGameButtons(false, false, false, false, players.Find(p => p.ps.PlayerNick == nick));
                }
            }
            [PunRPC]
            private void SplitTurn_RPC(string nick)
            {
                needSplit = true;

               
                
        }
        #endregion


        public void PlayerReady(PlayerStats player)
        {
            photonView.RPC("PlayerReady_RPC", RpcTarget.All, player.PlayerNick);
        }
        public void TakeCard(PlayerStats player)
        {

            photonView.RPC("TakeCard_RPC", RpcTarget.All, player.PlayerNick);

        }
        public void SkipTurn(PlayerStats player)
        {
            photonView.RPC("SkipTurn_RPC", RpcTarget.All, player.PlayerNick);
        }
        public void Split(PlayerStats player)
        {
            photonView.RPC("SplitTurn_RPC", RpcTarget.All, player.PlayerNick);
        }
        #endregion
    }


}