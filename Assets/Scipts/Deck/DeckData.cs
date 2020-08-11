﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;

namespace Cards
{
    class DeckData : MonoBehaviour
    {
        [SerializeField]
        private int numberOfDecks = 10;
        public Stack<CardData> Deck;

        private void Start()
        {
            Deck = new Stack<CardData>();

            GenerateDeck();

           
        }
        
        private void GenerateDeck()
        {
            var listOfCards = new List<CardData>();
            Deck.Clear();

            for (var i = 0; i < numberOfDecks; i++)
            {
                var newDeck = CardUtils.Instance.CreateDefaultDeck();
                listOfCards.AddRange(newDeck);
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
