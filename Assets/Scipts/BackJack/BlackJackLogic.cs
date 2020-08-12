using Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scipts.BackJack
{
    /*-------------------------Black-Jack-----------------  
     *--------------------------Очки----------------------
     * 1) Туз - 11 очков
     * 2) 10, дама,валет,король - 10 очков
     * 3) 9 - 9 очков
     * 4) 8 - 8 очков
     * ...
     * 10)2 - 2 очка
     * ---------------------Конец-игры-и-коефициенты--------
     * 1) После второй очереди 21 очко (коеф 1.5)
     * 2) Очков больше чем у дилера (коеф 1)
     * 3) очков равное кол с дилером (коеф 0)
     * 4) Очков меньше чем у дилера (коеф -1)
     * 4) Страховка  (коеф -0.5)
     * -------------------------Правила и нюансы--------------------
     *1) две очереди карт по 1 штуке (2 карта дилера скрываетьсяч от игроков)
     *2) "Дубль" - дабл ставка если 9-11 то нужно напомнить про нее 
     * если 4-8 или 12-19 то не нужно но далб ставка возможна
     * 3) "спли" - если при первой раздаче две карты по 10 очков - то 
     *  игрок имеет право разделить это краты по отдельности и получить по 1 карты на разделенные
     *  и добавить такую же ставку которая тояла
     *  4) отказ - если при раздаче карты не нравяться игроку то от может сделать отменту ставки,
     *  при это потеряв ее половину
     * 5) страховка если у дилера на верху при раздаче 10 очков или 11 
     * игрок может застраховать ставку от блекджека, страховка половина от ставки или меньше 
     *  коеф страховки 1
     *  6) если у тебя блек джек то ты должне заявить о нем иначе если у дилера блек джек то ничия
     */
    public class BlackJackPlayer
    {
        public PlayerStats player;
        public List<BlackJackCards> BlackJackStaks;

        public BlackJackPlayer(PlayerStats _player)
        {
            player = _player;
            BlackJackStaks = new List<BlackJackCards>();
        }
    }

    public class BlackJackCards
    {
        public int bet;
        public List<CardData> cards;

        public BlackJackCards()
        {
            cards = new List<CardData>();
        }      
        public int GetSumOfPoints()
        {
            int sum = 0;

            cards.ForEach(c => sum += PointsForCard(c.Face));

            return sum;
        }

        int PointsForCard(Card_Face face)
        {
            switch (face)
            {
                case Card_Face.Ace:
                    return 11;
                case Card_Face.Ten:
                case Card_Face.King:
                case Card_Face.Jack:
                case Card_Face.Queen:
                    return 10;
                case Card_Face.Nine:
                    return 9;
                case Card_Face.Eight:
                    return 8;
                case Card_Face.Seven:
                    return 7;
                case Card_Face.Six:
                    return 6;
                case Card_Face.Five:
                    return 5;
                case Card_Face.Four:
                    return 4;
                case Card_Face.Three:
                    return 3;
                case Card_Face.Two:
                    return 2;

            }
            return -1;
        }
    }
    class BlackJackLogic
    {
        private const int blackJack = 21;

        private const float blackJackCoef = 1.5f;
        private const float simpleWinCoef = 1f;
        private const float surrenderCoef = -0.5f;
        //private const float saveCoef = -0.5f;
        private const float nobodyCoef = 0f;
        private const float loseCoef = -1f;

        public List<BlackJackPlayer> bjPlayers;

        public BlackJackPlayer diler;
        public DeckData deckData { get; private set; }
        public BlackJackLogic(List<PlayerStats> players)
        {
            bjPlayers = new List<BlackJackPlayer>();

            diler = new BlackJackPlayer(new PlayerStats("Diler", 0));
            players.ForEach(p => bjPlayers.Add(new BlackJackPlayer(p)));
            deckData = new DeckData();
            InitGame();
        }

        /// <summary>
        /// по очереди каждому игроку выдаеться по 1 картепо очереди пока не будет 2
        /// 1 карду дилер кладет на верх 2 карду скрывает
        /// </summary>
        private void InitGame()
        {
            //первая очередь
            bjPlayers.ForEach(p => {
                p.BlackJackStaks.Add(new BlackJackCards());
                p.BlackJackStaks[0].cards.Add(deckData.Deck.Pop());
            });
            diler.BlackJackStaks.Add(new BlackJackCards());

            //вторая очередь
            bjPlayers.ForEach(p => {
                p.BlackJackStaks.Add(new BlackJackCards());
                p.BlackJackStaks[0].cards.Add(deckData.Deck.Pop());
            });
            diler.BlackJackStaks.Add(new BlackJackCards());
        }

        public bool CheckPlayer(PlayerStats player, out BlackJackPlayer bjPlayer)
        {
            if (bjPlayers.Exists(p => p.player == player))
            {
                bjPlayer = bjPlayers.Find(p => p.player == player);
                return true;
            }
            bjPlayer = null;
            return false;
        }
        /// <summary>
        /// Если со второй очереди 21 очко то блек джек 
        /// </summary>
        /// <param name="player"> игрок </param>
        /// <returns> был ли блек джек </returns>
        public bool CheckBlackJack(PlayerStats player, out int betWin)
        {
            BlackJackPlayer bjPlayer;
            betWin = -999999999;
            if (CheckPlayer(player, out bjPlayer))
            {
                if (bjPlayer.BlackJackStaks[0].GetSumOfPoints() == blackJack)
                {
                    betWin = Convert.ToInt32(bjPlayer.BlackJackStaks[0].bet * blackJackCoef);
                    player.AllMoney += betWin;
                }
            }

            return false;

        }
        public void AddBet(int bet, PlayerStats player)
        {
            BlackJackPlayer bjPlayer;
            if (CheckPlayer(player, out bjPlayer))
            {
                bjPlayer.BlackJackStaks[0].bet += bet;
            }
        }
        public bool IsPlunk(PlayerStats player, out int lose)
        {
            lose = -9999999;
            BlackJackPlayer bjPlayer;

            if (CheckPlayer(player, out bjPlayer))
            {
                if (bjPlayer.BlackJackStaks[0].GetSumOfPoints() > blackJack)
                {
                   
                    lose = Convert.ToInt32(bjPlayer.BlackJackStaks[0].bet * loseCoef);
                    player.AllMoney += lose;
                    return true;
                    
                }
              
            }
            return false;
        }
        bool? IsWinVersusDiler(PlayerStats player, out int betWin)
        {
            betWin = -9999999;
            BlackJackPlayer bjPlayer;

            if (CheckPlayer(player, out bjPlayer))
            {
                if (!IsPlunk(player, out betWin))
                {
                    if (diler.BlackJackStaks[0].GetSumOfPoints() < bjPlayer.BlackJackStaks[0].GetSumOfPoints())
                        betWin = Convert.ToInt32(bjPlayer.BlackJackStaks[0].bet * simpleWinCoef);
                    else if (diler.BlackJackStaks[0].GetSumOfPoints() == bjPlayer.BlackJackStaks[0].GetSumOfPoints())
                        betWin = Convert.ToInt32(bjPlayer.BlackJackStaks[0].bet * nobodyCoef);
                    else
                        betWin = Convert.ToInt32(bjPlayer.BlackJackStaks[0].bet * loseCoef);

                }

                player.AllMoney += betWin;         
            }
          
            return null;
        }
        public void ReciveBet(int bet, PlayerStats player)
        {

            BlackJackPlayer bjPlayer;

            if (CheckPlayer(player, out bjPlayer))
            {
                bjPlayer.BlackJackStaks[0].bet -= bet;
            }
        }

        public bool IsPlayerCanSurrender(PlayerStats player)
        {
            BlackJackPlayer bjPlayer;
            if (CheckPlayer(player, out bjPlayer))
            {
                if (bjPlayer.BlackJackStaks.Count == 1 && bjPlayer.BlackJackStaks[0].cards.Count == 2)
                    return true;
            }

            return false;
        }
        public void PlayerSurrender(PlayerStats player, out int lose)
        {
            BlackJackPlayer bjPlayer;
            lose = -999999999;
            if (CheckPlayer(player, out bjPlayer))
            {
                lose = Convert.ToInt32(bjPlayer.BlackJackStaks[0].bet * surrenderCoef);
                player.AllMoney += lose;
            }
        }
    }
}
