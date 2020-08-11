﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cards
{

    public class CardUtils : Singleton<CardUtils>
    { 

        public List<GameObject> CardsPrefabs;



        public IReadOnlyCollection<CardData> DefaultDeck = new List<CardData>()
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

        public IEnumerable<CardData> CreateDefaultDeck()
        {
            var list = new List<CardData>();

            foreach (var card in DefaultDeck)
            {
                list.Add(new CardData(card.Sign, card.Face));
            }

            return list;
        }

        
        private void Start()
        {
            CardsPrefabs = new List<GameObject>();

            LoadClovers();
            LoadDiamond();
            LoadHearts();
            LoadSpades();
        }
        public string GetPathToCard(CardData data)
        {
            switch (data.Sign)
            {
                case Card_Sign.Clover:
                    return GetCloverCard(data, "B", "Clover");
                    
                case Card_Sign.Diamond:
                    return GetCloverCard(data, "B", "Diamond");
                    
                case Card_Sign.Heart:
                    return GetCloverCard(data, "B", "Heart");
                    
                case Card_Sign.Spades:
                    return GetCloverCard(data, "B", "Spade");
                   

            }

            return null;
        }
        private string GetCloverCard(CardData data, string color, string sign)
        {          
            switch (data.Face)
            {
                case Card_Face.Ace:
                    return "Cards/" + sign + "s/Card_"+ sign + "_"+ color + "_Ace";    
                case Card_Face.King:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_King";
                case Card_Face.Jack:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_Jack";
                case Card_Face.Queen:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_Queen";
                case Card_Face.Ten:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_10";
                case Card_Face.Nine:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_9";
                case Card_Face.Eight:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_8";
                case Card_Face.Seven:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_7";
                case Card_Face.Six:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_6";
                case Card_Face.Five:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_5";
                case Card_Face.Four:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_4";
                case Card_Face.Three:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_3";
                case Card_Face.Two:
                    return "Cards/" + sign + "s/Card_" + sign + "_" + color + "_2";


            }

            return null;
        }
        private void LoadClovers()
        {
             CardsPrefabs.Add(Resources.Load("Cards/Clover/Card_Clover_B_02") as GameObject);
             CardsPrefabs.Add( Resources.Load("Cards/Clover/Card_Clover_B_03") as GameObject);
             CardsPrefabs.Add( Resources.Load("Cards/Clover/Card_Clover_B_04") as GameObject);
             CardsPrefabs.Add( Resources.Load("Cards/Clover/Card_Clover_B_05") as GameObject);
             CardsPrefabs.Add(Resources.Load("Cards/Clover/Card_Clover_B_06") as GameObject);
             CardsPrefabs.Add( Resources.Load("Cards/Clover/Card_Clover_B_07") as GameObject);
             CardsPrefabs.Add( Resources.Load("Cards/Clover/Card_Clover_B_08") as GameObject);
             CardsPrefabs.Add( Resources.Load("Cards/Clover/Card_Clover_B_09") as GameObject);
             CardsPrefabs.Add(Resources.Load("Cards/Clover/Card_Clover_B_10") as GameObject);
             CardsPrefabs.Add( Resources.Load("Cards/Clover/Card_Clover_B_King") as GameObject);
             CardsPrefabs.Add(Resources.Load("Cards/Clover/Card_Clover_B_Ace") as GameObject);
             CardsPrefabs.Add( Resources.Load("Cards/Clover/Card_Clover_B_Jack") as GameObject);
             CardsPrefabs.Add( Resources.Load("Cards/Clover/Card_Clover_B_Queen") as GameObject);
        }

        private void LoadDiamond()
        {
           CardsPrefabs.Add(Resources.Load("Cards/Diamonds/Card_Diamond_B_02") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Diamonds/Card_Diamond_B_03") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Diamonds/Card_Diamond_B_04") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Diamonds/Card_Diamond_B_05") as GameObject);
           CardsPrefabs.Add(Resources.Load("Cards/Diamonds/Card_Diamond_B_06") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Diamonds/Card_Diamond_B_07") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Diamonds/Card_Diamond_B_08") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Diamonds/Card_Diamond_B_09") as GameObject);
           CardsPrefabs.Add(Resources.Load("Cards/Diamonds/Card_Diamond_B_10") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Diamonds/Card_Diamond_B_King") as GameObject);
           CardsPrefabs.Add(Resources.Load("Cards/Diamonds/Card_Diamond_B_Ace") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Diamonds/Card_Diamond_B_Jack") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Diamonds/Card_Diamond_B_Queen") as GameObject);
        }

        private void LoadHearts()
        {
           CardsPrefabs.Add(Resources.Load("Cards/Hearts/Card_Heart_B_02") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Hearts/Card_Heart_B_03") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Hearts/Card_Heart_B_04") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Hearts/Card_Heart_B_05") as GameObject);
           CardsPrefabs.Add(Resources.Load("Cards/Hearts/Card_Heart_B_06") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Hearts/Card_Heart_B_07") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Hearts/Card_Heart_B_08") as GameObject);
           CardsPrefabs.Add(Resources.Load("Cards/Hearts/Card_Heart_B_09") as GameObject);
           CardsPrefabs.Add(Resources.Load("Cards/Hearts/Card_Heart_B_10") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Hearts/Card_Heart_B_King") as GameObject);
           CardsPrefabs.Add(Resources.Load("Cards/Hearts/Card_Heart_B_Ace") as GameObject);
           CardsPrefabs.Add( Resources.Load("Cards/Hearts/Card_Heart_B_Jack") as GameObject);
            CardsPrefabs.Add( Resources.Load("Cards/Hearts/Card_Heart_B_Queen") as GameObject);
        }

        private void LoadSpades()
        {
            CardsPrefabs.Add(Resources.Load("Cards/Spades/Card_Spades_B_02") as GameObject);
            CardsPrefabs.Add( Resources.Load("Cards/Spades/Card_Spades_B_03") as GameObject);
            CardsPrefabs.Add( Resources.Load("Cards/Spades/Card_Spades_B_04") as GameObject);
            CardsPrefabs.Add( Resources.Load("Cards/Spades/Card_Spades_B_05") as GameObject);
            CardsPrefabs.Add(Resources.Load("Cards/Spades/Card_Spades_B_06") as GameObject);
            CardsPrefabs.Add( Resources.Load("Cards/Spades/Card_Spades_B_07") as GameObject);
            CardsPrefabs.Add( Resources.Load("Cards/Spades/Card_Spades_B_08") as GameObject);
            CardsPrefabs.Add(Resources.Load("Cards/Spades/Card_Spades_B_09") as GameObject);
            CardsPrefabs.Add(Resources.Load("Cards/Spades/Card_Spades_B_10") as GameObject);
            CardsPrefabs.Add( Resources.Load("Cards/Spades/Card_Spades_B_King") as GameObject);
            CardsPrefabs.Add(Resources.Load("Cards/Spades/Card_Spades_B_Ace") as GameObject);
            CardsPrefabs.Add( Resources.Load("Cards/Spades/Card_Spades_B_Jack") as GameObject);
            CardsPrefabs.Add( Resources.Load("Cards/Spades/Card_Spades_B_Queen") as GameObject);
        }

        public GameObject GetCard(Card_Face face, Card_Sign sign)
        {
            return CardsPrefabs.Find(card => card.GetComponent<CardData>().Face == face && card.GetComponent<CardData>().Sign == sign);
        }

        public float yOffset = 0.0002f;
        public Stack<CardData> InstantiateDeck(Stack<CardData> deck, Transform spawnPointTransform)
        {
            CardData peek = new CardData(Card_Sign.Clover, Card_Face.Ace);
            try {
                if (deck.Count != 0)
                {
                    float currY = 0;
                    var reverseStack = new Stack<CardData>();
                    var returnStack = new Stack<CardData>();
                    while (deck.Count != 0)
                    {
                        var data = deck.Pop();
                        reverseStack.Push(data);

                        
                    }

                    while (reverseStack.Count != 0)
                    {
                        peek = reverseStack.Pop();
                        var cardPrefab = GetCard(peek.Face, peek.Sign);

                        var createdGameObj = Instantiate(cardPrefab, spawnPointTransform, false);
                        createdGameObj.GetComponent<Rigidbody>().isKinematic = true;


                        createdGameObj.transform.rotation = new Quaternion();
                        createdGameObj.transform.localPosition = new Vector3(0, currY, 0);
                        currY += yOffset;

                        returnStack.Push(createdGameObj.GetComponent<CardData>());

                    }

                    return returnStack;

                }

                return new Stack<CardData>();
            }
            catch (Exception ex)
            {
                Debug.Log(Enum.GetName(typeof(Card_Face), peek.Face) + " " + Enum.GetName(typeof(Card_Sign), peek.Sign));
                return new Stack<CardData>(); ;
            }
         }
            
        

    }
}
