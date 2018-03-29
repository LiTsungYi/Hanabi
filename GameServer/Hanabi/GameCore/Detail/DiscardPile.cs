using System.Collections.Generic;
using Hanabi.Types;

namespace Hanabi.GameCore.Detail
{
    /// <summary>
    /// 棄牌區
    /// </summary>
    internal sealed class DiscardPile
    {
        /// <summary>
        /// 建立棄牌區
        /// </summary>
        public DiscardPile()
        {
            DiscardedCards = new List<Card>();
        }

        /// <summary>
        /// 棄掉的牌
        /// </summary>
        private List<Card> DiscardedCards
        {
            get;
            set;
        }

        /// <summary>
        /// 棄牌
        /// </summary>
        /// <param name="card">要棄的牌</param>
        /// <returns>傳回 true 表示棄牌成功</returns>
        public bool Discard( Card card )
        {
            foreach ( var discardedCard in DiscardedCards )
            {
                if ( discardedCard.Id == card.Id || discardedCard.Index == card.Index )
                {
                    return false;
                }
            }
            DiscardedCards.Add( card );
            return true;
        }
    }
}
