using System.Collections.Generic;
using GameCore;
using Hanabi.GameCore;
using Hanabi.Types;

namespace Hanabi.Payloads
{
    #region Game
    public sealed class ResponseEnterGame
    {
        public ResponseEnterGame( ExitGameResult result )
        {
            this.Result = result;
        }

        public ExitGameResult Result
        {
            get;
            set;
        }
    }

    public sealed class ResponseExitGame
    {
        public ResponseExitGame( ExitGameResult result )
        {
            this.Result = result;
        }

        public ExitGameResult Result
        {
            get;
            set;
        }
    }
    #endregion

    #region Room
    public sealed class PlayerInfo
    {
        public string Nickname
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }
    }

    public sealed class RoomInfo
    {
        public int RoomIndex
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        public List<Status> Players
        {
            get;
            set;
        }
    }

    public sealed class ResponseRoomList
    {
        public List<RoomInfo> Rooms
        {
            get;
            set;
        }
    }

    public sealed class ResponseJoinRoom
    {
        public JoinRoomResult Result
        {
            get;
            set;
        }

        public RoomInfo Room
        {
            get;
            set;
        }
    }

    public sealed class ResponseQuitRoom
    {
        public QuitRoomResult Result
        {
            get;
            set;
        }

        public RoomInfo Room
        {
            get;
            set;
        }
    }
    #endregion

    #region Play
    public sealed class CardPrompt
    {
        public int Color
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public List<int> ImpossibleSet
        {
            get;
            set;
        }
    }

    public sealed class CardInfo
    {
        public CardInfo()
        {
        }

        public int Index
        {
            get;
            set;
        }

        public int Color
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public CardPrompt Prompt
        {
            get;
            set;
        }
    }

    public sealed class TokenInfo
    {
        public TokenInfo()
        {
            this.Note = 0;
            this.Storm = 0;
        }

        public int Note
        {
            get;
            set;
        }

        public int Storm
        {
            get;
            set;
        }
    }

    public sealed class ResponseGameData
    {
        public ResponseGameData()
        {
            this.Token = new TokenInfo();
            this.DrawPileCount = 0;
            this.Cards = new Dictionary<string, List<CardInfo>>();
        }

        public TokenInfo Token
        {
            get;
            set;
        }

        public int DrawPileCount
        {
            get;
            set;
        }

        public Dictionary<string, List<CardInfo>> Cards
        {
            get;
            set;
        }
    }

    public sealed class ResponsePromptCard
    {
        public ResponsePromptCard()
        {
            this.Result = 0;
            this.Nickname = string.Empty;
            this.PromptInformation = 0;
            this.Cards = new List<CardInfo>();
            this.Token = new TokenInfo();
        }

        public PromptCardResult Result
        {
            get;
            set;
        }

        public string Nickname
        {
            get;
            set;
        }

        public int PromptInformation
        {
            get;
            set;
        }

        public List<CardInfo> Cards
        {
            get;
            set;
        }

        public TokenInfo Token
        {
            get;
            set;
        }
    }

    public sealed class ResponsePlayCard
    {
        public ResponsePlayCard()
        {
            this.Nickname = string.Empty;
            this.OldCard = null;
            this.NewCard = null;
            this.Token = new TokenInfo();
            this.DrawPileCount = 0;
        }

        public PlayCardResult Result
        {
            get;
            set;
        }

        public string Nickname
        {
            get;
            set;
        }

        public CardInfo OldCard
        {
            get;
            set;
        }

        public CardInfo NewCard
        {
            get;
            set;
        }

        public TokenInfo Token
        {
            get;
            set;
        }

        public int DrawPileCount
        {
            get;
            set;
        }
    }

    public sealed class ResponseDiscardCard
    {
        public ResponseDiscardCard()
        {
            this.Nickname = string.Empty;
            this.OldCard = null;
            this.NewCard = null;
            this.Token = new TokenInfo();
            this.DrawPileCount = 0;
        }

        public DiscardCardResult Result
        {
            get;
            set;
        }

        public string Nickname
        {
            get;
            set;
        }

        public CardInfo OldCard
        {
            get;
            set;
        }

        public CardInfo NewCard
        {
            get;
            set;
        }

        public TokenInfo Token
        {
            get;
            set;
        }

        public int DrawPileCount
        {
            get;
            set;
        }
    }
    #endregion
}
