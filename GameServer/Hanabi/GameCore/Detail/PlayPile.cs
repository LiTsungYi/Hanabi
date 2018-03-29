using System.Collections.Generic;
using System.Linq;
using Hanabi.Types;

namespace Hanabi.GameCore.Detail
{
    /// <summary>
    /// 出牌區
    /// </summary>
    internal sealed class PlayPile
    {
        /// <summary>
        /// 建立出牌區
        /// </summary>
        /// <param name="settings">遊戲設定</param>
        public PlayPile( GameSettings settings )
        {
            PlayedCards = new Dictionary<CardColorType, CardValueType>();
            PlayedCards.Add( CardColorType.Blue, CardValueType.Unknown );
            PlayedCards.Add( CardColorType.Green, CardValueType.Unknown );
            PlayedCards.Add( CardColorType.Yellow, CardValueType.Unknown );
            PlayedCards.Add( CardColorType.Red, CardValueType.Unknown );
            PlayedCards.Add( CardColorType.White, CardValueType.Unknown );
        }

        public int Score
        {
            get
            {
                return PlayedCards.Sum( x => ( int ) x.Value );
            }
        }

        /// <summary>
        /// 已經出的牌
        /// </summary>
        private Dictionary<CardColorType, CardValueType> PlayedCards
        {
            get;
            set;
        }

        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="card">要出的牌</param>
        /// <returns>傳回 true 表示出牌合法</returns>
        public bool Play( Card card )
        {
            if ( card.Color == CardColorType.Unknown || card.Value == CardValueType.Unknown )
            {
                return false;
            }

            if ( !PlayedCards.ContainsKey( card.Color ) )
            {
                return false;
            }

            if ( PlayedCards[ card.Color ] != card.Value - 1 )
            {
                return false;
            }
            PlayedCards[ card.Color ] = card.Value;
            return true;
        }
    }
}
