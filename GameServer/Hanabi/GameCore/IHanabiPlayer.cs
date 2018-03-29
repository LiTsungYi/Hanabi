using System.Collections.Generic;
using GameCore;
using Hanabi.Types;

namespace Hanabi.GameCore
{
    public enum PlayerRoomStatus
    {
        Idle,
        Joined,
        Ready,
        Playing,
    }

    public interface IHanabiPlayer : IPlayer
    {
        PlayerRoomStatus RoomStatus
        {
            get;
        }

        Room Room
        {
            get;
        }

        List<Card> Cards
        {
            get;
        }

        bool LastTurn
        {
            get;
            set;
        }

        void OnJoinRoom( Room room );
        void OnQuitRoom();

        void OnReady();

        void DrawCard( Card card );
        void PlayCard( CardIndexType index );
        void DiscardCard( CardIndexType index );

        void PromptCard( CardColorType color );
        void PromptCard( CardValueType number );

        Card GetCard( CardIndexType index );
    }
}
