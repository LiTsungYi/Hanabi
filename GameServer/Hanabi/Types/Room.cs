using System.Collections.Generic;
using System.Linq;
using GameCore.Types;
using Hanabi.GameCore;
using Hanabi.Payloads;
using Newtonsoft.Json;

namespace Hanabi.Types
{
    public enum JoinRoomResult
    {
        Success,
        Fail,
    }

    public enum QuitRoomResult
    {
        Success,
        Fail,
    }

    public enum RoomStatus
    {
        Wait,
        Play
    }

    public class RoomIndexType : IdType<int>
    {
        public RoomIndexType( int index )
            : base( index )
        {
        }

        public int Index
        {
            get
            {
                return this.IdTypeValue;
            }
        }
    }

    public class Room
    {
        public Room( int index )
        {
            this.Status = RoomStatus.Wait;
            this.RoomIndex = new RoomIndexType( index );
            this.Setting = new GameSettings();
            this.Players = new List<IHanabiPlayer>();
        }

        public RoomStatus Status
        {
            get;
            private set;
        }

        public RoomIndexType RoomIndex
        {
            get;
            private set;
        }

        public GameSettings Setting
        {
            get;
            private set;
        }

        public List<IHanabiPlayer> Players
        {
            get;
            private set;
        }

        public RoomInfo Info
        {
            get
            {
                RoomInfo info = new RoomInfo();
                info.RoomIndex = RoomIndex.Index;
                info.Status = ( int ) Status;
                info.Players = new List<string>();
                foreach ( var player in Players )
                {
                    info.Players.Add( player.Nickname.Value );
                }
                return info;
            }
        }

        public GameBoard Board
        {
            get;
            private set;
        }

        public GameRule Rule
        {
            get;
            private set;
        }

        public int CurrentPlayer
        {
            get;
            private set;
        }

        private bool LastRound
        {
            get;
            set;
        }

        public void Clear()
        {
            Status = RoomStatus.Wait;
            Board = null;
            Rule = null;
            CurrentPlayer = 0;
            LastRound = false;
        }

        public void BeginGame()
        {
            this.Status = RoomStatus.Play;
            Board = new GameBoard( Players );
            Rule = new GameRule();
            foreach ( var player in Players )
            {
                ResponseGameData initialData = new ResponseGameData();
                initialData.Token.Note = Setting.InitialHint;
                initialData.DrawPileCount = Board.Size;

                foreach ( var otherPlayer in Players )
                {
                    List<CardInfo> cards = new List<CardInfo>();
                    foreach ( var card in otherPlayer.Cards )
                    {
                        CardInfo info = card.Info;
                        if ( otherPlayer.Nickname == player.Nickname )
                        {
                            info.Color = ( int ) CardColorType.Unknown;
                            info.Value = ( int ) CardValueType.Unknown;
                        }
                        cards.Add( info );
                    }
                    initialData.Cards.Add( otherPlayer.Nickname.Value, cards );
                }

                SendCommandToPlayer( player, ActionType.NotifyBoard, initialData );
            }

            CurrentPlayer = 0;
            LastRound = false;
            NotifyTurn();
        }

        /// <summary>
        /// 提示玩家卡牌資訊
        /// </summary>
        /// <param name="player">行動的玩家</param>
        /// <param name="prompt">提示資訊</param>
        public void Prompt( IHanabiPlayer player, RequestPromptCard prompt )
        {
            if ( !IsCurrentPlayer( player ) )
            {
                ResponsePromptCard invalidResponse = new ResponsePromptCard();
                invalidResponse.Result = PromptCardResult.InvalidTurn;
                invalidResponse.PromptInformation = prompt.PromptInformation;
                SendCommandToPlayer( player, ActionType.PromptCard, invalidResponse );
                return;
            }

            IHanabiPlayer targetPlayer = this.GetPlayer( prompt.Nickname );
            if ( targetPlayer == null )
            {
                ResponsePromptCard invalidResponse = new ResponsePromptCard();
                invalidResponse.Result = PromptCardResult.InvalidPlayer;
                invalidResponse.PromptInformation = prompt.PromptInformation;
                SendCommandToPlayer( player, ActionType.PromptCard, invalidResponse );
                return;
            }

            PromptCardResult result = PromptCardResult.InvalidPrompt;
            switch ( prompt.PromptInformation )
            {
                case ( int ) CardValueType.Value1:
                case ( int ) CardValueType.Value2:
                case ( int ) CardValueType.Value3:
                case ( int ) CardValueType.Value4:
                case ( int ) CardValueType.Value5:
                    result = Rule.PromptCard( ( CardValueType ) prompt.PromptInformation, targetPlayer, Board );
                    break;

                case ( int ) CardColorType.Blue:
                case ( int ) CardColorType.Green:
                case ( int ) CardColorType.Yellow:
                case ( int ) CardColorType.Red:
                case ( int ) CardColorType.White:
                    result = Rule.PromptCard( ( CardColorType ) prompt.PromptInformation, targetPlayer, Board );
                    break;
            }

            if ( result != PromptCardResult.Success )
            {
                ResponsePromptCard invalidResponse = new ResponsePromptCard();
                invalidResponse.Result = result;
                invalidResponse.PromptInformation = prompt.PromptInformation;
                SendCommandToPlayer( player, ActionType.PromptCard, invalidResponse );
                return;
            }

            // 通知被提示玩家的命令
            ResponsePromptCard notify = new ResponsePromptCard();
            notify.Result = result;
            notify.Nickname = prompt.Nickname;
            notify.PromptInformation = prompt.PromptInformation;
            notify.Token = Board.Tokens.Info;

            // 通知其他玩家的命令
            ResponsePromptCard response = new ResponsePromptCard();
            response.Result = result;
            response.Nickname = prompt.Nickname;
            response.PromptInformation = prompt.PromptInformation;
            response.Token = Board.Tokens.Info;
            foreach ( var handCard in targetPlayer.Cards )
            {
                CardInfo responseInfo = handCard.Info;
                response.Cards.Add( responseInfo );

                CardInfo notifyInfo = handCard.Info;
                notifyInfo.Color = ( int ) CardColorType.Unknown;
                notifyInfo.Value = ( int ) CardValueType.Unknown;
                notify.Cards.Add( notifyInfo );
            }

            foreach ( var notifyPlayer in Players )
            {
                if ( notifyPlayer == targetPlayer )
                {
                    SendCommandToPlayer( notifyPlayer, ActionType.PromptCard, notify );
                }
                else
                {
                    SendCommandToPlayer( notifyPlayer, ActionType.PromptCard, response );
                }
            }

            NextActivePlayer();
            NotifyNextPlayer();
        }

        public void Play( IHanabiPlayer player, RequestPlayCard card )
        {
            if ( !IsCurrentPlayer( player ) )
            {
                ResponsePlayCard invalidResponse = new ResponsePlayCard();
                invalidResponse.Result = PlayCardResult.InvalidTurn;
                SendCommandToPlayer( player, ActionType.PlayCard, invalidResponse );
                return;
            }

            CardIndexType cardIndex = new CardIndexType( card.CardIndex );
            Card oldCard = player.GetCard( cardIndex );

            Card newCard = null;
            PlayCardResult result = Rule.PlayCard( cardIndex, player, Board, out newCard );
            if ( result != PlayCardResult.Success && result != PlayCardResult.FailNoSlot )
            {
                ResponsePlayCard invalidResponse = new ResponsePlayCard();
                invalidResponse.Result = result;
                SendCommandToPlayer( player, ActionType.PlayCard, invalidResponse );
                return;
            }

            ResponsePlayCard response = new ResponsePlayCard();
            response.Result = result;
            response.Nickname = player.Nickname.Value;
            response.OldCard = oldCard.Info;
            response.NewCard = ( newCard != null ) ? newCard.Info : null;
            response.Token = Board.Tokens.Info;
            response.DrawPileCount = Board.Size;

            CardInfo info = ( newCard != null ) ? newCard.Info : null;
            if ( info != null )
            {
                info.Color = ( int ) CardColorType.Unknown;
                info.Value = ( int ) CardValueType.Unknown;
            }

            ResponsePlayCard notify = new ResponsePlayCard();
            notify.Result = result;
            notify.Nickname = player.Nickname.Value;
            notify.OldCard = oldCard.Info;
            notify.NewCard = info;
            notify.Token = Board.Tokens.Info;
            notify.DrawPileCount = Board.Size;

            foreach ( var notifyPlayer in Players )
            {
                if ( notifyPlayer == player )
                {
                    SendCommandToPlayer( notifyPlayer, ActionType.PlayCard, notify );
                }
                else
                {
                    SendCommandToPlayer( notifyPlayer, ActionType.PlayCard, response );
                }
            }

            CheckLastRound();

            NextActivePlayer();
            NotifyNextPlayer();
        }

        public void Discard( IHanabiPlayer player, RequestDiscardCard card )
        {
            if ( !IsCurrentPlayer( player ) )
            {
                ResponseDiscardCard invalidResponse = new ResponseDiscardCard();
                invalidResponse.Result = DiscardCardResult.InvalidTurn;
                SendCommandToPlayer( player, ActionType.DiscardCard, invalidResponse );
                return;
            }

            CardIndexType cardIndex = new CardIndexType( card.CardIndex );
            Card oldCard = player.GetCard( cardIndex );

            Card newCard = null;
            DiscardCardResult result = Rule.DiscardCard( cardIndex, player, Board, out newCard );
            if ( result != DiscardCardResult.Success )
            {
                ResponseDiscardCard invalidResponse = new ResponseDiscardCard();
                invalidResponse.Result = result;
                SendCommandToPlayer( player, ActionType.DiscardCard, invalidResponse );
                return;
            }

            ResponseDiscardCard response = new ResponseDiscardCard();
            response.Result = result;
            response.Nickname = player.Nickname.Value;
            response.OldCard = oldCard.Info;
            response.NewCard = ( newCard != null ) ? newCard.Info : null;
            response.Token = Board.Tokens.Info;
            response.DrawPileCount = Board.Size;

            CardInfo info = ( newCard != null ) ? newCard.Info : null;
            if ( info != null )
            {
                info.Color = ( int ) CardColorType.Unknown;
                info.Value = ( int ) CardValueType.Unknown;
            }

            ResponseDiscardCard notify = new ResponseDiscardCard();
            notify.Result = result;
            notify.Nickname = player.Nickname.Value;
            notify.OldCard = oldCard.Info;
            notify.NewCard = info;
            notify.Token = Board.Tokens.Info;
            notify.DrawPileCount = Board.Size;

            foreach ( var notifyPlayer in Players )
            {
                if ( notifyPlayer == player )
                {
                    SendCommandToPlayer( notifyPlayer, ActionType.DiscardCard, notify );
                }
                else
                {
                    SendCommandToPlayer( notifyPlayer, ActionType.DiscardCard, response );
                }
            }

            CheckLastRound();

            NextActivePlayer();
            NotifyNextPlayer();
        }

        private bool IsCurrentPlayer( IHanabiPlayer player )
        {
            return Players[ CurrentPlayer ] == player;
        }

        private IHanabiPlayer GetPlayer( string nickname )
        {
            return Players.Where( p => p.Nickname.Value == nickname ).First();
        }

        private void NextActivePlayer()
        {
            CurrentPlayer = ( ++CurrentPlayer ) % Players.Count;
        }

        private void CheckLastRound()
        {
            if ( LastRound )
            {
                return;
            }

            if ( Board.Size == 0 )
            {
                LastRound = true;
                foreach ( var player in Players )
                {
                    player.LastTurn = true;
                }
            }
        }

        private bool IsGameEnd()
        {
            if ( !LastRound )
            {
                if ( Board.Tokens.Storm == Setting.MaxError )
                {
                    return true;
                }
                return false;
            }

            foreach ( var player in Players )
            {
                if ( player.LastTurn )
                {
                    return false;
                }
            }
            return true;
        }

        private void NotifyNextPlayer()
        {
            if ( IsGameEnd() )
            {
                NotifyRound();
            }
            else
            {
                NotifyTurn();
            }
        }

        private void NotifyRound()
        {
            ResponseNotifyRound response = new ResponseNotifyRound();
            response.Score = Board.Score;
            foreach ( var player in Players )
            {
                SendCommandToPlayer( player, ActionType.NotifyRound, response );
            }
        }

        private void NotifyTurn()
        {
            ResponseNotifyTurn response = new ResponseNotifyTurn();
            response.Nickname = Players[ CurrentPlayer ].Nickname.Value;
            foreach ( var player in Players )
            {
                SendCommandToPlayer( player, ActionType.NotifyTurn, response );
            }
        }

        private void SendCommandToPlayer<T>( IHanabiPlayer player, ActionType action, T response )
        {
            Command command = new Command();
            command.Action = action.ToString();
            command.Payload = JsonConvert.SerializeObject( response );
            player.SendCommand( command );
        }
    }
}
