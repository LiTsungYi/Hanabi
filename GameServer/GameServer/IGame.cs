using System.Collections.Generic;
using GameServer.Types;

namespace GameServer
{
    public enum GameStatus
    {
        Waiting,
        Playing,
    }

    public enum JoinResult
    {
        Success,
        Fail,
    }

    public enum LeaveResult
    {
        Success,
        Fail,
    }

    public interface IGame
    {
        GameStatus Status
        {
            get;
        }

        GameSettings Settings
        {
            get;
        }

        Dictionary<NicknameType, IPlayer> Players
        {
            get;
        }

        JoinResult PlayerJoin(IPlayer player);
        LeaveResult PlayerLeave(IPlayer player);

        void DispatchRequest(IPlayer player, string message);
    }
}
