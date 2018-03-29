namespace Hanabi.Payloads
{
    #region Gmae
    public sealed class RequestEnterGame
    {
        public RequestEnterGame()
        {
            this.Nickname = string.Empty;
        }

        public string Nickname
        {
            get;
            set;
        }
    }

    public sealed class RequestExitGame
    {
        public RequestExitGame()
        {
            this.Nickname = string.Empty;
        }

        public string Nickname
        {
            get;
            set;
        }
    }
    #endregion

    #region Room
    public sealed class RequestJoinRoom
    {
        public RequestJoinRoom()
        {
            this.RoomIndex = 0;
        }

        public int RoomIndex
        {
            get;
            set;
        }
    }

    public sealed class RequestQuitRoom
    {
        public RequestQuitRoom()
        {
            this.RoomIndex = 0;
        }

        public int RoomIndex
        {
            get;
            set;
        }
    }
    #endregion

    #region Play
    public sealed class RequestPromptCard
    {
        public RequestPromptCard()
        {
            this.Nickname = string.Empty;
            this.PromptInformation = 0;
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
    }

    public sealed class RequestPlayCard
    {
        public RequestPlayCard()
        {
            this.CardIndex = 0;
        }

        public int CardIndex
        {
            get;
            set;
        }
    }

    public sealed class RequestDiscardCard
    {
        public RequestDiscardCard()
        {
            this.CardIndex = 0;
        }

        public int CardIndex
        {
            get;
            set;
        }
    }
    #endregion
}
