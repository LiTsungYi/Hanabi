using Hanabi.Types;

namespace Hanabi.GameCore
{
    /// <summary>
    /// 出牌結果
    /// </summary>
    public enum PlayCardResult
    {
        /// <summary>
        /// 出牌成功
        /// </summary>
        Success,

        /// <summary>
        /// 出牌無效 (不是手牌)
        /// </summary>
        InvalidCardIndex,

        /// <summary>
        /// 不是行動玩家
        /// </summary>
        InvalidTurn,

        /// <summary>
        /// 出牌錯誤
        /// </summary>
        FailNoSlot,
    }

    /// <summary>
    /// 棄牌結果
    /// </summary>
    public enum DiscardCardResult
    {
        /// <summary>
        /// 棄牌成功
        /// </summary>
        Success,

        /// <summary>
        /// 棄牌無效
        /// </summary>
        InvalidCardIndex,

        /// <summary>
        /// 不是行動玩家
        /// </summary>
        InvalidTurn,
    }

    /// <summary>
    /// 提示玩家結果
    /// </summary>
    public enum PromptCardResult
    {
        /// <summary>
        /// 提示成功
        /// </summary>
        Success,

        /// <summary>
        /// 錯誤的顏色或數字
        /// </summary>
        InvalidPrompt,

        /// <summary>
        /// 沒有提示指示物
        /// </summary>
        PromptEmpty,

        /// <summary>
        /// 不是行動玩家
        /// </summary>
        InvalidTurn,

        /// <summary>
        /// 對象錯誤
        /// </summary>
        InvalidPlayer,
    }

    /// <summary>
    /// 遊戲規則的類別
    /// </summary>
    public class GameRule
    {
        /// <summary>
        /// 打出卡牌
        ///   從手牌中打出 1張卡牌，正面朝上置於桌面場內。
        ///   從牌堆抽取最上方一張牌，正面朝外，置於手牌上。
        ///   打出的卡牌必須符合下列原則：
        ///     牌上數字須為桌面場內同色牌未出現，且須接續在已出現同色牌數字後。
        ///   若打出的卡牌不符合以上原則，即算施放失敗。
        ///     取 1個紅色錯誤指示物，施放失敗的花火卡則正面朝上置於棄牌區
        ///   若玩家完成一組顏色花火卡牌施放(即數字1~5依照順序排列的「同花順」)
        ///     得到 1個藍色傳達指示物做為獎勵。
        ///     若已無藍色傳達指示物，則無此獎勵行動。
        /// </summary>
        /// <param name="cardIndex">打出的牌</param>
        /// <param name="player">出牌玩家</param>
        /// <param name="board">遊戲資訊</param>
        /// <param name="drawedCard">出牌後的補牌</param>
        /// <returns>出牌的結果</returns>
        public PlayCardResult PlayCard( CardIndexType cardIndex, IHanabiPlayer player, GameBoard board, out Card drawedCard )
        {
            Card playedCard = player.GetCard( cardIndex );
            if ( playedCard == null )
            {
                drawedCard = null;
                return PlayCardResult.InvalidCardIndex;
            }
            player.PlayCard( cardIndex );

            drawedCard = board.Draw();
            if ( drawedCard != null )
            {
                player.DrawCard( drawedCard );
            }

            if ( board.Play( playedCard ) )
            {
                if ( playedCard.Value == CardValueType.Value5 )
                {
                    board.Reward();
                }
                return PlayCardResult.Success;
            }
            else
            {
                board.Punish();
                board.Discard( playedCard );
                return PlayCardResult.FailNoSlot;
            }
        }

        /// <summary>
        /// 丟棄手牌
        ///   從手牌中丟棄一張牌，正面朝上置於棄牌區。
        ///   從牌堆抽取最上方一張牌，正面朝外，置於手牌上。
        ///   得到 1個藍色傳達指示物，若已無已消耗藍色傳達指示物，則不可執行此行動。
        /// </summary>
        /// <param name="cardIndex">丟棄的牌</param>
        /// <param name="player">丟牌玩家</param>
        /// <param name="board">遊戲資訊</param>
        /// <param name="drawedCard">出牌後的補牌</param>
        /// <returns>棄牌結果</returns>
        public DiscardCardResult DiscardCard( CardIndexType cardIndex, IHanabiPlayer player, GameBoard board, out Card drawedCard )
        {
            Card discardedCard = player.GetCard( cardIndex );
            if ( discardedCard == null )
            {
                drawedCard = null;
                return DiscardCardResult.InvalidCardIndex;
            }
            player.DiscardCard( cardIndex );

            drawedCard = board.Draw();
            if ( drawedCard != null )
            {
                player.DrawCard( drawedCard );
            }

            board.Discard( discardedCard );
            board.Reward();

            return DiscardCardResult.Success;
        }

        /// <summary>
        /// 傳遞訊息
        ///   消耗 1個藍色傳達指示物，若已無未消耗藍色傳達指示物，則不可執行此行動。
        ///   告訴另一玩家其手牌的線索
        ///   提供線索須遵循以下原則：
        ///     只能就玩家手牌內的某種顏色或某種數字提供線索
        /// </summary>
        /// <param name="color">提示的顏色</param>
        /// <param name="player">被提示的玩家</param>
        /// <param name="board">遊戲資訊</param>
        /// <returns>提示結果</returns>
        public PromptCardResult PromptCard( CardColorType color, IHanabiPlayer player, GameBoard board )
        {
            if ( color == CardColorType.Unknown )
            {
                return PromptCardResult.InvalidPrompt;
            }

            if ( !board.Use() )
            {
                return PromptCardResult.PromptEmpty;
            }

            player.PromptCard( color );
            return PromptCardResult.Success;
        }

        /// <summary>
        /// 傳遞訊息
        ///   消耗 1個藍色傳達指示物，若已無未消耗藍色傳達指示物，則不可執行此行動。
        ///   告訴另一玩家其手牌的線索
        ///   提供線索須遵循以下原則：
        ///     只能就玩家手牌內的某種顏色或某種數字提供線索
        /// </summary>
        /// <param name="value">提示的數字</param>
        /// <param name="player">被提示的玩家</param>
        /// <param name="board">遊戲資訊</param>
        /// <returns>提示結果</returns>
        public PromptCardResult PromptCard( CardValueType value, IHanabiPlayer player, GameBoard board )
        {
            if ( value == CardValueType.Unknown )
            {
                return PromptCardResult.InvalidPrompt;
            }

            if ( !board.Use() )
            {
                return PromptCardResult.PromptEmpty;
            }

            player.PromptCard( value );
            return PromptCardResult.Success;
        }
    }
}
