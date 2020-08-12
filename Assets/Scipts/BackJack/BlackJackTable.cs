using Assets.Scipts.Animators;
using Cards;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scipts.BackJack
{
    public enum BlackJackGameStates{ CheckPlayer, CardsToPlayers, PlayersBetting, GameEnd }
    class BlackJackTable : MonoBehaviourPun
    {
        BlackJackGameStates gameState = BlackJackGameStates.CheckPlayer;
        BlackJackLogic blackJackLogic;
        [SerializeField]
        private List<PlayerPlace> players;

        List<PlayerPlace> playersInGame;
        BlackJackLogic blackJack;


        [SerializeField]
        BlackJackPlayerCardField BlackJackDilerCardField;
       
        [SerializeField]
        private CardCurveAnimator cardCurveAnimator;
        int waitTimeInSec = 5;
        int currWaitTimeforPlayers = 0;
        int currentPlayerBettingTime = 0;
        int OneSec = 1;
        private void Start()
        {
            playersInGame = new List<PlayerPlace>();
            StartCoroutine(BlackJackLoop());

        }
        void ResetGame()       
        {
             
            currWaitTimeforPlayers = 0;
            gameState = BlackJackGameStates.CheckPlayer;
        }
        IEnumerator BlackJackLoop()
        {
            while (true)
            {
                if (gameState == BlackJackGameStates.CheckPlayer)
                {
                    yield return WaitingForSitPlayers();
                }
                else if (gameState == BlackJackGameStates.CardsToPlayers)
                {
                    yield return CardsToPlyers();
                }
                else if (gameState == BlackJackGameStates.PlayersBetting)
                {
                    yield return PlayersBetting();
                }
                else if (gameState == BlackJackGameStates.GameEnd)
                {
                    ResetGame();
                }
            }
        }
        private IEnumerator WaitingForSitPlayers()
        {
            if (players.Exists(p => p.ps != null))
            {
               
                yield return new WaitForSeconds(OneSec);
                currWaitTimeforPlayers++;
                          
            }
            if (currWaitTimeforPlayers == waitTimeInSec)
            {
                var result = players.FindAll(p => p.ps != null);
                if (result != null)
                {
                    Debug.Log(" to next state -> BlackJackGameStates.CardsToPlayers. players=" + result.Count);
                    playersInGame.AddRange(result);
                    //получаем стату игроков
                    List<PlayerStats> p_stats = new List<PlayerStats>();
                    playersInGame.ForEach(p => p_stats.Add(p.ps));
                    blackJack = new BlackJackLogic(p_stats);

                    gameState = BlackJackGameStates.CardsToPlayers;
                    
                }
                else currWaitTimeforPlayers = 0;
            }
            yield return null;
        }
        private IEnumerator CardsToPlyers()
        {
            int id;
            string nick;
            CardData card;
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < playersInGame.Count; j++)
                {
                    yield return new WaitForSeconds(2f);
                    Debug.Log("card to " + playersInGame[j].ps.PlayerNick + "curveID="+ playersInGame[j].PlaceId);

                    id = playersInGame[j].PlaceId;
                    nick = playersInGame[j].ps.PlayerNick;
                    card = blackJack.bjPlayers[j].BlackJackStaks[0].cards[0];

                    cardCurveAnimator.StartAnimCardToPlayer(id, nick, card);
                    
                }

                yield return new WaitForSeconds(2f);
                Debug.Log("card to Diler curveindex=" + players.Count);

                id = players.Count;
                nick = "Diler";
                card = blackJack.diler.BlackJackStaks[0].cards[0];

                cardCurveAnimator.StartAnimCardToPlayer(players.Count, nick, card);
                
            }

            gameState = BlackJackGameStates.PlayersBetting;
        }

        private IEnumerator PlayersBetting()
        {
            List<PlayerPlace> toRemove = new List<PlayerPlace>();
            for (var j = 0; j < playersInGame.Count; j++)
            {
                currentPlayerBettingTime = 0;

                while (currentPlayerBettingTime != waitTimeInSec)
                {
                    currentPlayerBettingTime++;
                    yield return new WaitForSeconds(OneSec);

                }

                if (currentPlayerBettingTime == waitTimeInSec)
                {
                    if (playersInGame[j].GetComponent<PlayerBlackJackFields>().bettingField.Stacks[0].Objects.Count == 0)
                        toRemove.Add(playersInGame[j]);
                }

            }

            //выкинуть игрока со стола
            toRemove.ForEach(r =>
            {
                playersInGame.Remove(r);
                r.GoOutFromPlace();
            });

            gameState = BlackJackGameStates.PlayersBetting;
        }


    }
}
