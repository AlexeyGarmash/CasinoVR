using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;

namespace Cards
{
    class DeckData 
    {     
        private int numberOfDecks = 5;
        public Stack<CardData> Deck;

        public DeckData(int[] indexes)
        {
            Deck = new Stack<CardData>();
            GenerateDeck(indexes);

        }
        public DeckData()
        {
           
        }


        public int[] GenerateDeck()
        {
            var listOfCards = new List<CardData>();
            Deck = new Stack<CardData>();

            for (var i = 0; i < numberOfDecks; i++)
            {
                var newDeck = CardUtils.Instance.CreateDefaultDeck();
                listOfCards.AddRange(newDeck);
            }

            Deck = new Stack<CardData>();
            List<int> deckIndexes = new List<int>();

           
            while (listOfCards.Count != 0)
            {
                int randIndex = UnityEngine.Random.Range(0, listOfCards.Count);

                Deck.Push(listOfCards[randIndex]);
                deckIndexes.Add(randIndex);
                listOfCards.RemoveAt(randIndex);
            }

            return deckIndexes.ToArray();
        }
        public void GenerateDeck(int[] indexes)
        {
            var listOfCards = new List<CardData>();
            Deck.Clear();

            for (var i = 0; i < numberOfDecks; i++)
            {
                var newDeck = CardUtils.Instance.CreateDefaultDeck();
                listOfCards.AddRange(newDeck);
            }

            Deck = new Stack<CardData>();



            foreach (int index in indexes)
            {
                Deck.Push(listOfCards[index]);             
                listOfCards.RemoveAt(index);
            }
           
        }


    }
}
