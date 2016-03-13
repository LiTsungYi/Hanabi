using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanabi.Datas
{
    public class CardInfo
    {
        public CardInfo( Difficultly difficultly )
        {
            ColorInfo = new Dictionary<CardColor, CardInfoHint>()
            {
                { CardColor.White, CardInfoHint.None },
                { CardColor.Red, CardInfoHint.None },
                { CardColor.Blue, CardInfoHint.None },
                { CardColor.Yellow, CardInfoHint.None },
                { CardColor.Green, CardInfoHint.None },
                { CardColor.Colorful, CardInfoHint.None },
            };

            NumberInfo = new Dictionary<CardNumber, CardInfoHint>()
            {
                {CardNumber.Number1, CardInfoHint.None },
                {CardNumber.Number2, CardInfoHint.None },
                {CardNumber.Number3, CardInfoHint.None },
                {CardNumber.Number4, CardInfoHint.None },
                {CardNumber.Number5, CardInfoHint.None },
            };
        }

        public IDictionary<CardColor, CardInfoHint> ColorInfo
        {
            get;
            private set;
        }

        public IDictionary<CardNumber, CardInfoHint> NumberInfo
        {
            get;
            private set;
        }

        public void PromptIs( CardColor color )
        {
            if ( color == CardColor.Colorful || color == CardColor.None )
            {
                throw new ArgumentException();
            }

            ColorInfo[ color ] = CardInfoHint.Sure;
            if ( ColorInfo[ CardColor.Colorful ] != CardInfoHint.None )
            {
                return;
            }

            if ( ColorInfo.Where( info => info.Key != CardColor.Colorful && info.Value == CardInfoHint.Sure ).Count() > 1 )
            {
                ColorInfo[ color ] = CardInfoHint.Sure;
            }
        }

        public void PromptIs( CardNumber number )
        {
            if ( number == CardNumber.None )
            {
                throw new ArgumentException();
            }

            foreach ( var key in NumberInfo.Keys )
            {
                NumberInfo[ key ] = ( key == number ) ? CardInfoHint.Sure : CardInfoHint.Not;
            }
        }
        
        public void PromptNot( CardColor color )
        {
            if ( color == CardColor.Colorful || color == CardColor.None )
            {
                throw new ArgumentException();
            }
            
            ColorInfo[ color ] = CardInfoHint.Not;
            ColorInfo[ CardColor.Colorful ] = CardInfoHint.Not;
        }
        
        public void PromptNot( CardNumber number )
        {
            if ( number == CardNumber.None )
            {
                throw new ArgumentException();
            }

            NumberInfo[ number ] = CardInfoHint.Not;
        }
    }
}
