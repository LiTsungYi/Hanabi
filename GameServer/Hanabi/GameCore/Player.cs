using System.Collections.Generic;
using System.Diagnostics;
using Fleck;
using GameCore;
using GameCore.Types;
using Hanabi.Types;
using Newtonsoft.Json;

namespace Hanabi.GameCore
{
    public class Player : IHanabiPlayer
    {
        public Player( IGame game, IWebSocketConnection client )
        {
            this.Game = game;
            this.Client = client;
            this.GameStatus = PlayerGameStatus.Idle;
            this.RoomStatus = PlayerRoomStatus.Idle;
            this.Nickname = new NicknameType( string.Empty );
            this.Cards = new List<Card>();
            this.LastTurn = false;
        }

        public IWebSocketConnection Client
        {
            get;
            private set;
        }

        public PlayerGameStatus GameStatus
        {
            get;
            private set;
        }

        public PlayerRoomStatus RoomStatus
        {
            get;
            private set;
        }

        public NicknameType Nickname
        {
            get;
            private set;
        }

        public IGame Game
        {
            get;
            private set;
        }

        public Room Room
        {
            get;
            private set;
        }

        public List<Card> Cards
        {
            get;
            private set;
        }

        public bool LastTurn
        {
            get;
            set;
        }

        public void OnEnterGame( NicknameType nickname )
        {
            Debug.Assert( GameStatus == PlayerGameStatus.Idle );

            this.Nickname = nickname;
            GameStatus = PlayerGameStatus.Entered;
            this.LastTurn = false;
        }

        public void OnExitGame()
        {
            Debug.Assert( GameStatus == PlayerGameStatus.Entered );

            this.Game = null;
            GameStatus = PlayerGameStatus.Idle;
        }

        public void OnJoinRoom( Room room )
        {
            Debug.Assert( GameStatus == PlayerGameStatus.Entered );
            Debug.Assert( RoomStatus == PlayerRoomStatus.Idle );

            this.Room = room;
            RoomStatus = PlayerRoomStatus.Joined;
        }

        public void OnQuitRoom()
        {
            Debug.Assert( GameStatus == PlayerGameStatus.Entered );
            Debug.Assert( RoomStatus == PlayerRoomStatus.Joined || RoomStatus == PlayerRoomStatus.Ready || RoomStatus == PlayerRoomStatus.Playing );

            this.Room = null;
            RoomStatus = PlayerRoomStatus.Idle;
        }

        public void OnReady()
        {
            RoomStatus = PlayerRoomStatus.Ready;
        }

        public void SendCommand( Command command )
        {
            string commandString = JsonConvert.SerializeObject( command );
            Client.Send( commandString );
        }

        public void DrawCard( Card card )
        {
            AddCard( card );
        }

        public void PlayCard( CardIndexType index )
        {
            RemoveCard( index );

            CheckIsLastTurn();
        }

        public void DiscardCard( CardIndexType index )
        {
            RemoveCard( index );

            CheckIsLastTurn();
        }

        public void PromptCard( CardColorType color )
        {
            foreach ( var card in Cards )
            {
                if ( card.Color == color )
                {
                    card.Information.Color = color;
                }
                else
                {
                    card.Information.ImpossiblePrompt.Add( ( PromptInformation ) color );
                }
            }

            CheckIsLastTurn();
        }

        public void PromptCard( CardValueType value )
        {
            foreach ( var card in Cards )
            {
                if ( card.Value == value )
                {
                    card.Information.Value = value;
                }
                else
                {
                    card.Information.ImpossiblePrompt.Add( ( PromptInformation ) value );
                }
            }

            CheckIsLastTurn();
        }

        public Card GetCard( CardIndexType index )
        {
            foreach ( var card in Cards )
            {
                if ( card.Index == index )
                {
                    return card;
                }
            }
            return null;
        }

        private void AddCard( Card card )
        {
            Cards.Add( card );
        }

        private void RemoveCard( CardIndexType index )
        {
            Card card = GetCard( index );
            if ( card == null )
            {
                return;
            }
            Cards.Remove( card );
        }

        private void CheckIsLastTurn()
        {
            if ( LastTurn )
            {
                LastTurn = false;
            }
        }
    }
}
