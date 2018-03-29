using System.Collections.Generic;
using Fleck;
using GameCore.Types;

namespace GameCore
{
    public enum GameStatus
    {
        /// <summary>
        /// 等待中
        /// </summary>
        Waiting,

        /// <summary>
        /// 遊戲中
        /// </summary>
        Playing,
    }

    public enum EnterGameResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,

        /// <summary>
        /// 失敗
        /// </summary>
        Fail,
    }

    public enum ExitGameResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,

        /// <summary>
        /// 失敗
        /// </summary>
        Fail,
    }

    public interface IGame
    {
        GameStatus Status
        {
            get;
        }

        IPlayer CreatePlayer( IWebSocketConnection client );

        void PlayerDisconnect( IPlayer player );

        void DispatchRequest( IPlayer player, string message );
    }
}
