using Assets.Scipts.Chips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cards 
{

    public enum Card_Sign { Clover, Diamond, Heart, Spades }
    public enum Card_Face { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Ace, Jack, King, Queen }
    public class CardData : OwnerData
    {
        public Card_Sign Sign;
        public Card_Face Face;

        public CardData(Card_Sign sign, Card_Face face)
        {
            Sign = sign;
            Face = face;
        }

    }
}
