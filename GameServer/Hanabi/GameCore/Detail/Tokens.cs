using Hanabi.Payloads;
using Hanabi.Types;

namespace Hanabi.GameCore.Detail
{
    /// <summary>
    /// 指示物類別
    /// </summary>
    public sealed class GameTokens
    {
        /// <summary>
        /// 建構指示物
        /// </summary>
        /// <param name="settings">遊戲設定</param>
        public GameTokens( GameSettings settings )
        {
            MaxPrompts = settings.InitialHint;
            MaxError = settings.MaxError;
            Storm = 0;
            Note = settings.InitialHint;
        }

        /// <summary>
        /// 提示指示物數量
        /// </summary>
        public int Note
        {
            get;
            private set;
        }

        /// <summary>
        /// 錯誤指示物數量
        /// </summary>
        public int Storm
        {
            get;
            private set;
        }

        public TokenInfo Info
        {
            get
            {
                TokenInfo info = new TokenInfo();
                info.Note = Note;
                info.Storm = Storm;
                return info;
            }
        }

        /// <summary>
        /// 最大提示數
        /// </summary>
        private int MaxPrompts
        {
            get;
            set;
        }

        /// <summary>
        /// 最大錯誤數
        /// </summary>
        private int MaxError
        {
            get;
            set;
        }

        /// <summary>
        /// 消耗提示
        /// </summary>
        /// <returns>傳回 true 表示提示指示物足夠</returns>
        public bool Use()
        {
            if ( Note == 0 )
            {
                return false;
            }
            --Note;
            return true;
        }

        /// <summary>
        /// 獲得提示
        /// </summary>
        /// <returns>傳回 true 表示成功獲得提示指示物</returns>
        public bool Reward()
        {
            if ( Note == MaxPrompts )
            {
                return false;
            }
            ++Note;
            return true;
        }

        /// <summary>
        /// 出牌錯誤懲罰
        /// </summary>
        /// <returns>傳回 false 表示錯誤太多</returns>
        public bool Punish()
        {
            if ( Storm == MaxError )
            {
                return false;
            }
            ++Storm;
            return true;
        }
    }
}
