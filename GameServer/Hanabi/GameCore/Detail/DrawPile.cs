using System.Collections.Generic;
using Hanabi.Types;

namespace Hanabi.GameCore.Detail
{
    /// <summary>
    /// 牌庫
    /// </summary>
    internal sealed class DrawPile
    {
        /// <summary>
        /// 建立抽牌堆
        /// </summary>
        /// <param name="settings">遊戲設定</param>
        public DrawPile( GameSettings settings )
        {
            // NOTE: 各色牌都有 3張1 2張2,3,4 1張5
            CardValueType[] values =
            {
                CardValueType.Value1, CardValueType.Value1, CardValueType.Value1,
                CardValueType.Value2, CardValueType.Value2,
                CardValueType.Value3, CardValueType.Value3,
                CardValueType.Value4, CardValueType.Value4,
                CardValueType.Value5
            };

            int cardNumber = ( int ) settings.ColorCount * values.Length;
            CardIndexType[] indeies = new CardIndexType[ cardNumber ];
            for ( int count = 0; count < cardNumber; ++count )
            {
                indeies[ count ] = new CardIndexType( count );
            }
            GameBoard.RandomSort( indeies );

            Card[] tempCards = new Card[ cardNumber ];
            for ( int count = 0; count < cardNumber; ++count )
            {
                CardColorType color = ( CardColorType ) ( ( ( count / 10 ) + 1 ) * 10 );
                CardValueType number = values[ count % 10 ];
                tempCards[ count ] = new Card( new CardIdType( count ), indeies[ count ], color, number );
            }
            GameBoard.RandomSort( tempCards );

            Pile = new Queue<Card>( tempCards );
        }

        /// <summary>
        /// 牌庫大小
        /// </summary>
        public int Size
        {
            get
            {
                return Pile.Count;
            }
        }

        /// <summary>
        /// 牌庫內的牌
        /// </summary>
        private Queue<Card> Pile
        {
            get;
            set;
        }

        /// <summary>
        /// 抽牌
        /// </summary>
        /// <returns>傳回 null 表示牌庫已經空了</returns>
        public Card Draw()
        {
            if ( Size == 0 )
            {
                return null;
            }
            return Pile.Dequeue();
        }
    }
}
