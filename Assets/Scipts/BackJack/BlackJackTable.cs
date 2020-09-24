using Assets.Scipts.Animators;
using Assets.Scipts.BackJack.Buttons;
using Cards;
using OVRTouchSample;
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

    class BlackJackTable : MonoBehaviourPun
    {     

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

        List<PlayerPlace> sittedPlayers;



        [SerializeField]
        private CardCurveAnimator cardCurveAnimator;


        const int waitTimeInSec = 30;
        const int OneSec = 1;

        int currWaitTime = 0;
        int playersReady;

        private bool needSplit;
        private bool playerSurrendered;
        int currentPlayerTurn = 0;
        int i = 0;
        int currentBJStackIndex;

        CardData takedCard;

        #region SetPlace

        [PunRPC]
        public void AddPlayerIngame_RPC(int PlaceID)
        {
            var player = players.FirstOrDefault(p => p.PlaceId == PlaceID);
            if (!sittedPlayers.Contains(player))
                sittedPlayers.Add(players.FirstOrDefault(p => p.PlaceId == PlaceID));
        }
        public void AddPlayerInGame(PlayerStats PlaceID)
        {
            var player = players.FirstOrDefault(p => p.ps.PlayerNick == PlaceID.PlayerNick);


            photonView.RPC("AddPlayerIngame_RPC", RpcTarget.All, player.PlaceId);
            DebugLog("AddPlayerInGame clinet can controll player place-> " + player.photonView.IsMine);
            var handMenu = player.handMenu;

            if (handMenu.IsNotNull())
            {
               
                var animatorHolder = handMenu.GetComponent<AnimatorHolder>();
                var watches = handMenu.GetComponent<WatchController>();
                watches.watchIndicator.StartIndicatorAnimation(waitTimeInSec);
                //handMenu.RevokeMenu();
                handMenu.AddAction(new RadialActionInfo(() =>
                {
                    PlayerReadyToPlay(player.PlaceId);

                    animatorHolder.hand.SetPose(animatorHolder.ready);
                }, "Ready"));


                handMenu.InvokeMenu();

            }
        }

        [PunRPC]
        public void RemovePlayerFromGame_RPC(int PlaceID)
        {
            sittedPlayers.Remove(sittedPlayers.FirstOrDefault(p => p.PlaceId == PlaceID));

        }
        public void RemovePlayerFromGame(PlayerStats PlaceID)
        {
            var player = sittedPlayers.FirstOrDefault(p => p.ps == PlaceID);
            photonView.RPC("RemovePlayerFromGame_RPC", RpcTarget.All, player.PlaceId);

            if (player.photonView.IsMine)
            {
                var handMenu = player.handMenu;


                var animatorHolder = handMenu.GetComponent<AnimatorHolder>();
                var watches = handMenu.GetComponent<WatchController>();
                watches.watchIndicator.StopAnimation();

                StartCoroutine(ClearPoseWithDilay(animatorHolder.hand, 1f));

                player.handMenu.RevokeMenu();
            }
        }
        #endregion

        private void Start()
        {
            sittedPlayers = new List<PlayerPlace>();
            bjNPCWinLosePlayer = bjNPC.GetComponent<WinLoseAudioPlayer>();
            playersOutFromGame = new List<PlayerPlace>();
            playersInGame = new List<PlayerPlace>();
            ResetGame();

        }

        IEnumerator ResetGameCourotine(float delay)
        {
            yield return new WaitForSeconds(delay);
            ResetGame();

        }

        void ResetGame()
        {

            bjNPC.ResetValues();
            playersReady = 0;
            deckSetedRemote = 0;
            currentBJStackIndex = 0;
           
                sittedPlayers.ForEach(sp => {
                    if(sp.photonView.IsMine)
                    AddPlayerInGame(sp.ps);
                });


            players.ForEach(p => {
                var buttons = p.GetComponentInChildren<ButtonsHolder>();
                ActivateTakePlaceButton(true, p);
                var fields = p.GetComponent<PlayerBlackJackFields>();

                fields.bettingField.ClearStacks();
                fields.bettingField.BlockField(true);

                fields.bettingFieldForSplit.BlockField(true);
                fields.bettingFieldForSplit.ClearStacks();

                fields.blackJackCardField.ClearStacks();
                fields.blackJackCardField.BlockField(false);

                fields.blackJackCardFieldForSplit.BlockField(true);
                fields.blackJackCardFieldForSplit.ClearStacks();


            });


            playersInGame.Clear();
            playersOutFromGame.Clear();       

        }


        IEnumerator ClearPoseWithDilay(CustomHand hand, float delay)
        {
            yield return new WaitForSeconds(delay);
            hand.ClearPose();
        }

        void NextPlayerTurn()
        {
            if (splited && playersInGame.Count == 1)
            {
                splited = false;
                return;
            }
            else
            if (blackJackLogic.PlayerSplited(playersInGame[currentPlayerTurn].ps.PlayerNick) && currentBJStackIndex == 1 ||
                    !blackJackLogic.PlayerSplited(playersInGame[currentPlayerTurn].ps.PlayerNick))
            {
                if (playersInGame.Count - 1 == currentPlayerTurn)
                {
                    currentBJStackIndex = 0;
                    currentPlayerTurn = 0;
                }
                else {
                    currentBJStackIndex = 0;
                    currentPlayerTurn++;
                }
                return;
            }

            currentBJStackIndex++;

        }

        bool PlayersCanTurn()
        {
            bool canTrun = false;

            playersInGame.ForEach(p =>
            {
                if (blackJackLogic.CanTakeCard(p.ps.PlayerNick, 0))
                    canTrun = true;
            });

            return canTrun;
        }

        private void PlayersTurns()
        {
            if (playersInGame.Count > 0)
            {
                var p = playersInGame[currentPlayerTurn];


                var bjPlayer = blackJackLogic.bjPlayers.Find(bjp => bjp.player.PlayerNick == p.ps.PlayerNick);

                if (bjPlayer.IsNotNull())
                {


                    if (blackJackLogic.CanTakeCard(p.ps.PlayerNick, currentBJStackIndex))
                    {

                        WatchController watches = null;
                        RadialMenuHandV2 handMenu = null;
                        AnimatorHolder animatorHolder = null;

                        handMenu = p?.handMenu;

                        if (handMenu.IsNotNull())
                        {
                            handMenu = p.handMenu;

                            animatorHolder = handMenu.GetComponent<AnimatorHolder>();
                            watches = handMenu.GetComponent<WatchController>();
                            watches.watchIndicator.StartIndicatorAnimation(waitTimeInSec);

                            handMenu.AddAction(
                                new RadialActionInfo(() =>
                                {
                                    TakeCard(playersInGame[currentPlayerTurn].ps);

                                    animatorHolder.hand.SetPose(animatorHolder.give);
                                },
                                "Give card")
                            );

                            if (blackJackLogic.CanSplit(playersInGame[currentPlayerTurn].ps.PlayerNick))
                            {

                                handMenu.AddAction(new RadialActionInfo(() =>
                                {
                                    Split(playersInGame[currentPlayerTurn].ps);

                                    animatorHolder.hand.SetPose(animatorHolder.split);
                                }, "Split"));
                            }
                            handMenu.AddAction(new RadialActionInfo(() =>
                            {
                                SkipTurn(playersInGame[currentPlayerTurn].ps);

                                animatorHolder.hand.SetPose(animatorHolder.stop);
                            }, "Skip"));

                            handMenu.InvokeMenu();
                        }
                    }
                }
            }

            if (!PlayersCanTurn())
            {
                
                StartCoroutine(CheckResults());

            }

        }
        void ActivateTakePlaceButton(bool activateTakePlaceButton, PlayerPlace pp)
        {
            if (pp != null)
            {
                var buttonsHolder = pp.GetComponentInChildren<ButtonsHolder>();

                buttonsHolder.takePlace.gameObject.SetActive(activateTakePlaceButton);
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
        public void FindPlayers()
        {
            playersInGame = players.FindAll(p => p.PlayerOnPlace);
        }


        int deckSetedRemote = 0;
        [PunRPC]
        protected void SetDeck(int[] indexes)
        {         
            deckSetedRemote++;
            DebugLog("Deck seted" + deckSetedRemote);
            List<PlayerStats> p_stats = new List<PlayerStats>();
            playersInGame.ForEach(p => p_stats.Add(p.ps));
            blackJackLogic = new BlackJackLogic(p_stats, indexes);

            DebugLog(deckSetedRemote + " <-> " + playersInGame.Count);
            if(deckSetedRemote == playersInGame.Count)
            {
                PlayersBetting();
            }              
        }

        private IEnumerator CardsToPlyers()
        {
            int id;
            string nick;
            CardData card;


            playersInGame.ForEach(p =>
            {
                var handMenu = p?.handMenu;
                if (handMenu.IsNotNull())
                {
                   


                    var animatorHolder = handMenu.GetComponent<AnimatorHolder>();
                    var watches = handMenu.GetComponent<WatchController>();
                    watches.watchIndicator.StopAnimation();

                    StartCoroutine(ClearPoseWithDilay(animatorHolder.hand, 1f));

                    handMenu.RevokeMenu();
                }
            });



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


                    }
                    id = players.Count;
                    nick = "Diler";
                    card = blackJackLogic.diler.BlackJackStaks[0].cards[i];
                    bjNPC.AddCardToHand(5, card);

                    DebugLog("card to " + nick + " card face = " + card.Face + " card sign=  " + card.Sign);

                }

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

                if (playersInGame[currentPlayerTurn].handMenu.IsNotNull())
                    PlayersTurns();
                

            }
            else if (photonView.IsMine)
            {
                photonView.StartCoroutine(ResetGameCourotine(3f));
            }


            yield return null;
        }

        private void DebugLog(string text)
        {
            Debug.Log(text);
            tms.SetText(text);
        }

        private void PlayersBetting()
        {
            DebugLog("Player betting");

            List<PlayerPlace> toRemove = new List<PlayerPlace>();
            for (var j = 0; j < playersInGame.Count; j++)
            {
                
                var fields = playersInGame[j].GetComponent<PlayerBlackJackFields>();
                fields.bettingField.BlockField(false);


                AnimatorHolder animatorHolder = null;
                RadialMenuHandV2 handMenu = null;
                WatchController watches = null;

                handMenu = playersInGame[j]?.handMenu;

                if (handMenu.IsNotNull())
                {
                    DebugLog(j + " Player Update events");


                    handMenu = playersInGame[j].handMenu;

                    

                    animatorHolder = handMenu.GetComponent<AnimatorHolder>();
                    watches = handMenu.GetComponent<WatchController>();
                    watches.watchIndicator.StartIndicatorAnimation(waitTimeInSec);

                    

                    var ps = playersInGame[j].ps;
                    handMenu.AddAction(new RadialActionInfo(() =>
                    {
                        PlayerReady(ps);

                        animatorHolder.hand.SetPose(animatorHolder.ready);
                    }, "Play"));


                    handMenu.InvokeMenu();
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

                    var nick = "Diler";
                    var card = blackJackLogic.diler.BlackJackStaks[0].cards[i];
                    bjNPC.AddCardToHand(5, card);

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
                    if (blackJackLogic.IsWinVersusDiler(p.ps.PlayerNick, j, out win, out bet, out pointsPlayer, out pointsDiller))
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
                photonView.StartCoroutine(ResetGameCourotine(3f));
            }          

            yield return null;
        }
        #region Buttons Functions
        #region RPCs      
        [PunRPC]
        private void PlayerReady_RPC(string player)
        {

            playersReady++;

            if (playersReady == playersInGame.Count)
            {
               
                List<PlayerPlace> toRemove = new List<PlayerPlace>();
                for (var j = 0; j < playersInGame.Count; j++)
                {
                    var fields = playersInGame[j].GetComponent<PlayerBlackJackFields>();

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
                    
                    fields.bettingField.BlockField(true);
                }

                playersOutFromGame.AddRange(toRemove);

                toRemove.ForEach(r =>
                {
                    playersInGame.Remove(r);
                });



                StartCoroutine(CardsToPlyers());

            }
        }

        [PunRPC]
        private void TakeCard_RPC(string player)
        {
            StartCoroutine(TakeCard(player));
        }
        IEnumerator TakeCard(string player)
        {
            if (blackJackLogic.CanTakeCard(player, currentBJStackIndex))
            {

                blackJackLogic.TakeCard(player, currentBJStackIndex);

                var playerPlace = playersInGame.Find(p => p.ps.PlayerNick == player);
                var id = playerPlace.PlaceId;
                var nick = playerPlace.ps.PlayerNick;

                var bjPlayer = blackJackLogic.bjPlayers.Find(bjP => bjP.player.PlayerNick == nick);
                takedCard = bjPlayer.BlackJackStaks[currentBJStackIndex].cards[bjPlayer.BlackJackStaks[currentBJStackIndex].cards.Count - 1];

                DebugLog("Give me card button ->  card to " + player + " card face = " + takedCard.Face + " card sign=  " + takedCard.Sign);

                var field = playerPlace.GetComponent<PlayerBlackJackFields>();

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

                bjNPC.AddCardToHand(playerPlace.PlaceId, takedCard);

                yield return bjNPC.TakeCardsToPlayers(false);

                takedCard = null;


            }


            NextPlayerTurn();
            PlayersTurns();
        }
        [PunRPC]
        private void SkipTurn_RPC(string nick)
        {
            blackJackLogic.SkipTruns(nick, currentBJStackIndex);

            var playerPlace = playersInGame.Find(p => p.ps.PlayerNick == nick);

            NextPlayerTurn();
            PlayersTurns();
        }
        bool splited = false;
        [PunRPC]
        private void SplitTurn_RPC(string nick)
        {

            var playerPlace = playersInGame.Find(p => p.ps.PlayerNick == nick);

            var field = playerPlace.GetComponent<PlayerBlackJackFields>();

            blackJackLogic.UseSplit(playerPlace.ps.PlayerNick);


            //добавляем ставку для сплита
            foreach (var chip in field.bettingField.Stacks[0].Objects)
            {
                GameObject extractedChip;
                if (playerPlace.ExtractChipByCost(chip.GetComponent<ChipData>().Cost, out extractedChip))
                {
                    field.bettingFieldForSplit.MagnetizeObject(extractedChip, field.bettingFieldForSplit.Stacks[0]);
                }
            }
            //разделяем карты
            var player = blackJackLogic.bjPlayers.Find(bjp => playerPlace.ps.PlayerNick == bjp.player.PlayerNick);
            var card = field.blackJackCardField.ExtractObject(player.BlackJackStaks[1].cards[0]);
            field.blackJackCardFieldForSplit.MagnetizeObject(card, field.blackJackCardFieldForSplit.Stacks[0]);

           
            //добавляем по 2 карыт
            StartCoroutine(Split(field, player, playerPlace));



        }

        IEnumerator Split(PlayerBlackJackFields field, BlackJackPlayer player, PlayerPlace playerPlace)
        {
            for (var k = 0; k < 2; k++)
            {
                if (k == 0)
                {
                    field.blackJackCardFieldForSplit.BlockField(true);
                    field.blackJackCardField.BlockField(false);

                    bjNPC.AddCardToHand(playerPlace.PlaceId, player.BlackJackStaks[0].cards[1]);
                }
                else if (k == 1)
                {
                    field.blackJackCardFieldForSplit.BlockField(false);
                    field.blackJackCardField.BlockField(true);

                    bjNPC.AddCardToHand(playerPlace.PlaceId, player.BlackJackStaks[1].cards[1]);
                }

                yield return bjNPC.TakeCardsToPlayers(false);

            }

            splited = true;
            NextPlayerTurn();
            PlayersTurns();




        }

        IEnumerator WaitAllPlayers(int value, Action action)
        {
            while (PhotonNetwork.CurrentRoom.PlayerCount != value)
                yield return null;

            action();


        }
        [PunRPC]
        private void PlayerReadyToPlay_RPC(int PlaceID)
        {
           

            var playerPlace = sittedPlayers.FirstOrDefault(p => p.PlaceId == PlaceID);

            playersInGame.Add(playerPlace);

            if (sittedPlayers.Count == playersInGame.Count)
            {

                if (photonView.IsMine)
                {
                    DeckData dd = new DeckData();

                    var indexesRnd = dd.GenerateDeck();

                    photonView.RPC("SetDeck", RpcTarget.All, indexesRnd);              
                    PhotonNetwork.SendAllOutgoingCommands();

                    //next state

                    playersInGame.ForEach(p =>
                    {
                        ActivateTakePlaceButton(false, p);
                    });

                }

              

            }

        }
        #endregion


        public void PlayerReady(PlayerStats player)
        {
            var pp = players.FirstOrDefault(p => p.ps.PlayerNick == player.PlayerNick);
            ClearHandRadialMenu(pp);

            photonView.RPC("PlayerReady_RPC", RpcTarget.All, player.PlayerNick);

           

        }
        public void PlayerReadyToPlay(int PlaceID)
        {
            var player = players.FirstOrDefault(p => p.PlaceId == PlaceID);
            ClearHandRadialMenu(player);

            photonView.RPC("PlayerReadyToPlay_RPC", RpcTarget.All, PlaceID);
        }

        void ClearHandRadialMenu(PlayerPlace player)
        {

            if (player.photonView.IsMine)
            {
                AnimatorHolder animatorHolder = null;
                RadialMenuHandV2 handMenu = null;
                WatchController watches = null;


                handMenu = player.handMenu;


                animatorHolder = handMenu.GetComponent<AnimatorHolder>();
                watches = handMenu.GetComponent<WatchController>();
                watches.watchIndicator.StopAnimation();

                StartCoroutine(ClearPoseWithDilay(animatorHolder.hand, 1f));

                player.handMenu.RevokeMenu();
            }
        }
        public void TakeCard(PlayerStats player)
        {

            photonView.RPC("TakeCard_RPC", RpcTarget.All, player.PlayerNick);

            var pp = players.FirstOrDefault(p => p.ps.PlayerNick == player.PlayerNick);
            ClearHandRadialMenu(pp);
        }
        public void SkipTurn(PlayerStats player)
        {
            photonView.RPC("SkipTurn_RPC", RpcTarget.All, player.PlayerNick);

            var pp = players.FirstOrDefault(p => p.ps.PlayerNick == player.PlayerNick);
            ClearHandRadialMenu(pp);
        }
        public void Split(PlayerStats player)
        {
            photonView.RPC("SplitTurn_RPC", RpcTarget.All, player.PlayerNick);

            var pp = players.FirstOrDefault(p => p.ps.PlayerNick == player.PlayerNick);
            ClearHandRadialMenu(pp);
        }
        //public void Surrentered(PlayerStats player)
        //{
        //    photonView.RPC("SurrenderedTurn_RPC", RpcTarget.All, player.PlayerNick);
        //}
        #endregion
    }


}