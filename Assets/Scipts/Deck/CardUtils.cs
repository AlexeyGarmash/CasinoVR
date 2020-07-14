using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cards
{
   

    public class CardUtils : Singleton<CardUtils>
    {
        public static List<GameObject> CardsPrefabs;

        


        private void Awaik()
        {
            LoadClovers();
            LoadDiamond();
            LoadHearts();
            LoadSpades();
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

        public List<GameObject> GetCard(Card_Face face, Card_Sign sign)
        {
            return CardsPrefabs.Find(card => card.GetComponent<CardData>().Face == face && card.GetComponent<CardData>().Sign == sign);
        }

    }
}
