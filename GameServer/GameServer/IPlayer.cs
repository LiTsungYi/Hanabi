using Fleck;
using GameServer.Types;

namespace GameServer
{
    public enum PlayerStatus
    {
        Waiting,
        Joined,
        Matched,
        Playing,
    }

    public interface IPlayer
    {
        IWebSocketConnection client
        {
            get;
        }
 
        PlayerStatus Status
        {
            get;
        }

        NicknameType Nickname
        {
            get;
        }
    }
}
