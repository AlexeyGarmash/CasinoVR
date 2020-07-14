using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Exceptions;
using Unity;
namespace Cards
{

    class DeckData : MonoBehaviour
    {
        [SerializeField]
        private int numberOfDecks = 2;
        public Stack<CardData> Deck;

        IReadOnlyCollection<CardData> AllCardsInDeck = new List<CardData>()
        {
            //Clover
            new CardData(Card_Sign.Clover, Card_Face.Two),
            new CardData(Card_Sign.Clover, Card_Face.Three),
            new CardData(Card_Sign.Clover, Card_Face.Four),
            new CardData(Card_Sign.Clover, Card_Face.Five),
            new CardData(Card_Sign.Clover, Card_Face.Six),
            new CardData(Card_Sign.Clover, Card_Face.Seven),
            new CardData(Card_Sign.Clover, Card_Face.Eight),
            new CardData(Card_Sign.Clover, Card_Face.Nine),
            new CardData(Card_Sign.Clover, Card_Face.Ten),
            new CardData(Card_Sign.Clover, Card_Face.Ace),
            new CardData(Card_Sign.Clover, Card_Face.Jack),
            new CardData(Card_Sign.Clover, Card_Face.King),
            new CardData(Card_Sign.Clover, Card_Face.Queen),

            //Diamond
            new CardData(Card_Sign.Diamond, Card_Face.Two),
            new CardData(Card_Sign.Diamond, Card_Face.Three),
            new CardData(Card_Sign.Diamond, Card_Face.Four),
            new CardData(Card_Sign.Diamond, Card_Face.Five),
            new CardData(Card_Sign.Diamond, Card_Face.Six),
            new CardData(Card_Sign.Diamond, Card_Face.Seven),
            new CardData(Card_Sign.Diamond, Card_Face.Eight),
            new CardData(Card_Sign.Diamond, Card_Face.Nine),
            new CardData(Card_Sign.Diamond, Card_Face.Ten),
            new CardData(Card_Sign.Diamond, Card_Face.Ace),
            new CardData(Card_Sign.Diamond, Card_Face.Jack),
            new CardData(Card_Sign.Diamond, Card_Face.King),
            new CardData(Card_Sign.Diamond, Card_Face.Queen),


            //hearts
            new CardData(Card_Sign.Heart, Card_Face.Two),
            new CardData(Card_Sign.Heart, Card_Face.Three),
            new CardData(Card_Sign.Heart, Card_Face.Four),
            new CardData(Card_Sign.Heart, Card_Face.Five),
            new CardData(Card_Sign.Heart, Card_Face.Six),
            new CardData(Card_Sign.Heart, Card_Face.Seven),
            new CardData(Card_Sign.Heart, Card_Face.Eight),
            new CardData(Card_Sign.Heart, Card_Face.Nine),
            new CardData(Card_Sign.Heart, Card_Face.Ten),
            new CardData(Card_Sign.Heart, Card_Face.Ace),
            new CardData(Card_Sign.Heart, Card_Face.Jack),
            new CardData(Card_Sign.Heart, Card_Face.King),
            new CardData(Card_Sign.Heart, Card_Face.Queen),

            //Spades
            new CardData(Card_Sign.Spades, Card_Face.Two),
            new CardData(Card_Sign.Spades, Card_Face.Three),
            new CardData(Card_Sign.Spades, Card_Face.Four),
            new CardData(Card_Sign.Spades, Card_Face.Five),
            new CardData(Card_Sign.Spades, Card_Face.Six),
            new CardData(Card_Sign.Spades, Card_Face.Seven),
            new CardData(Card_Sign.Spades, Card_Face.Eight),
            new CardData(Card_Sign.Spades, Card_Face.Nine),
            new CardData(Card_Sign.Spades, Card_Face.Ten),
            new CardData(Card_Sign.Spades, Card_Face.Ace),
            new CardData(Card_Sign.Spades, Card_Face.Jack),
            new CardData(Card_Sign.Spades, Card_Face.King),
            new CardData(Card_Sign.Spades, Card_Face.Queen),

        };
        private void GenerateDeck()
        {
            var listOfCards = new List<CardData>();
            Deck.Clear();

            for (var i = 0; i < numberOfDecks; i++)
            {
                var copied = AllCardsInDeck.DeepClone<CardData>();
                listOfCards.AddRange(copied);
            }

            Deck = new Stack<CardData>();
          
            while (listOfCards.Count != 0)
            {
                int randIndex = UnityEngine.Random.Range(0, listOfCards.Count);

                Deck.Push(listOfCards[randIndex]);

                listOfCards.RemoveAt(randIndex);
            }


        }
        
    }
}
