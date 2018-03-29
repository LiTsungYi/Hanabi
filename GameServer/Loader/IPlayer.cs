using Fleck;
using GameCore.Types;

namespace GameCore
{
    public enum PlayerGameStatus
    {
        Idle,
        Entered,
    }

    public interface IPlayer
    {
        IWebSocketConnection Client
        {
            get;
        }

        NicknameType Nickname
        {
            get;
        }

        IGame Game
        {
            get;
        }

        PlayerGameStatus GameStatus
        {
            get;
        }

        void OnEnterGame( NicknameType nickname );
        void OnExitGame();
        void SendCommand( Command command );
    }
}
