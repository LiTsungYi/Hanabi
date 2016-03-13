using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanabi.Datas
{
    public enum CardColor
    {
        None,
        White,
        Red,
        Blue,
        Yellow,
        Green,
        Colorful,
    }

    public enum CardInfoHint
    {
        None,
        Not,
        Sure,
    }

    public enum CardNumber
    {
        None,
        Number1,
        Number2,
        Number3,
        Number4,
        Number5,
    }

    public class Card
    {
        public Card( Difficultly difficultly )
            : this( CardColor.None, CardNumber.None, difficultly )
        {
        }

        public Card( CardColor color, CardNumber number, Difficultly difficultly )
        {
            Color = color;
            Number = number;
            CardId = 0;
            Info = new CardInfo( difficultly );
        }

        public CardColor Color
        {
            get;
            set;
        }

        public CardNumber Number
        {
            get;
            set;
        }

        public int CardId
        {
            get;
            set;
        }

        public CardInfo Info
        {
            get;
            set;
        }
    }
}
