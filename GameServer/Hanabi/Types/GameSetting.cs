namespace Hanabi.Types
{
    /// <summary>
    /// 遊戲設定
    /// </summary>
    public sealed class GameSettings
    {
        /// <summary>
        /// 牌局最大玩家數
        /// </summary>
        public int MaxPlayers
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// 牌局最少玩家數
        /// </summary>
        public int MinPlayers
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// 最大錯誤數
        /// </summary>
        public int MaxError
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// 初始提示數
        /// </summary>
        public int InitialHint
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// 顏色數量
        /// </summary>
        public int ColorCount
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// 取得玩家手牌上限
        /// </summary>
        /// <param name="playerNumber">玩家人數</param>
        /// <returns>玩家手牌上限</returns>
        public int GetMaxHandCards( int playerNumber )
        {
            return ( playerNumber < 4 ) ? 5 : 4;
        }
    }
}
