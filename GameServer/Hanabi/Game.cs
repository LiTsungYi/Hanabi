using System;
using System.Collections.Generic;
using Fleck;
using GameCore;
using GameCore.Types;
using Hanabi.GameCore;
using Hanabi.Payloads;
using Hanabi.Types;
using Newtonsoft.Json;

namespace Hanabi
{
    public class Game : IGame
    {
        public Game( GameNameType gameName )
        {
            this.GameName = gameName;
            this.Players = new Dictionary<NicknameType, IHanabiPlayer>();
            Rooms = new Dictionary<RoomIndexType, Room>();
            for ( int count = 0; count < 10; ++count )
            {
                Room room = new Room( count );
                Rooms.Add( room.RoomIndex, room );
            }
        }

        public GameNameType GameName
        {
            get;
            private set;
        }

        public GameStatus Status
        {
            get;
            private set;
        }

        public Dictionary<NicknameType, IHanabiPlayer> Players
        {
            get;
            private set;
        }

        public Dictionary<RoomIndexType, Room> Rooms
        {
            get;
            private set;
        }

        public IPlayer CreatePlayer( IWebSocketConnection client )
        {
            return new Player( this, client );
        }

        public void DispatchRequest( IPlayer player, string message )
        {
            IHanabiPlayer hanabiPlayer = player as IHanabiPlayer;
            if ( hanabiPlayer == null )
            {
                return;
            }

            Command command = JsonConvert.DeserializeObject<Command>( message );
            ActionType action = ( ActionType ) Enum.Parse( typeof( ActionType ), command.Action, false );
            switch ( action )
            {
                case ActionType.EnterGame:
                    {
                        RequestEnterGame request = JsonConvert.DeserializeObject<RequestEnterGame>( command.Payload );
                        PlayerEnterGame( hanabiPlayer, request );
                        break;
                    }

                case ActionType.ExitGame:
                    {
                        RequestExitGame request = JsonConvert.DeserializeObject<RequestExitGame>( command.Payload );
                        PlayerExitGame( hanabiPlayer, request );
                        break;
                    }

                case ActionType.GetRoomList:
                    {
                        PlayerGetRoomList( hanabiPlayer );
                        break;
                    }

                case ActionType.JoinRoom:
                    {
                        RequestJoinRoom request = JsonConvert.DeserializeObject<RequestJoinRoom>( command.Payload );
                        PlayerJoinRoom( hanabiPlayer, request );
                        break;
                    }

                case ActionType.QuitRoom:
                    {
                        RequestQuitRoom request = JsonConvert.DeserializeObject<RequestQuitRoom>( command.Payload );
                        PlayerQuitRoom( hanabiPlayer, request );
                        break;
                    }

                case ActionType.Message:
                    {
                        // TODO:
                        break;
                    }

                case ActionType.Ready:
                    {
                        PlayerReady( hanabiPlayer );
                        break;
                    }

                case ActionType.PromptCard:
                    {
                        RequestPromptCard request = JsonConvert.DeserializeObject<RequestPromptCard>( command.Payload );
                        PlayerPrompt( hanabiPlayer, request );
                        break;
                    }

                case ActionType.PlayCard:
                    {
                        RequestPlayCard request = JsonConvert.DeserializeObject<RequestPlayCard>( command.Payload );
                        PlayerPlayCard( hanabiPlayer, request );
                        break;
                    }

                case ActionType.DiscardCard:
                    {
                        RequestDiscardCard request = JsonConvert.DeserializeObject<RequestDiscardCard>( command.Payload );
                        PlayerDiscardCard( hanabiPlayer, request );
                        break;
                    }

                default:
                    break;
            }
        }

        public void PlayerDisconnect( IPlayer player )
        {
            Player hanabiPlayer = player as Player;
            if ( hanabiPlayer == null )
            {
                return;
            }

            if ( hanabiPlayer.Room != null )
            {
                RequestQuitRoom request = new RequestQuitRoom();
                request.RoomIndex = hanabiPlayer.Room.RoomIndex.Index;
                PlayerQuitRoom( hanabiPlayer, request );
            }

            if ( player.GameStatus != PlayerGameStatus.Idle )
            {
                RequestExitGame request = new RequestExitGame();
                request.Nickname = player.Nickname.Value;
                PlayerExitGame( hanabiPlayer, request );
            }

            Players.Remove( player.Nickname );
        }

        private void PlayerEnterGame( IHanabiPlayer player, RequestEnterGame request )
        {
            ExitGameResult result = ExitGameResult.Fail;
            NicknameType nickname = new NicknameType( request.Nickname );

            if ( !Players.ContainsKey( nickname ) )
            {
                result = ExitGameResult.Success;

                player.OnEnterGame( nickname );
                Players.Add( player.Nickname, player );
            }

            ResponseEnterGame response = new ResponseEnterGame( result );

            SendCommand( player, ActionType.EnterGame, response );
        }

        private void PlayerExitGame( IHanabiPlayer player, RequestExitGame request )
        {
            ExitGameResult result = ExitGameResult.Success;
            if ( Players.ContainsKey( player.Nickname ) )
            {
                player.OnExitGame();
                result = ExitGameResult.Success;
            }

            ResponseExitGame response = new ResponseExitGame( result );

            SendCommand( player, ActionType.ExitGame, response );
        }

        private void PlayerGetRoomList( IHanabiPlayer player )
        {
            List<RoomInfo> roomList = new List<RoomInfo>();
            foreach ( var room in Rooms.Values )
            {
                roomList.Add( room.Info );
            }

            ResponseRoomList response = new ResponseRoomList();
            response.Rooms = roomList;

            SendCommand( player, ActionType.GetRoomList, response );
        }

        private void PlayerJoinRoom( IHanabiPlayer player, RequestJoinRoom request )
        {
            RoomInfo roomInfo = new RoomInfo();
            JoinRoomResult result = JoinRoomResult.Fail;

            if ( player.RoomStatus == PlayerRoomStatus.Idle )
            {
                RoomIndexType roomIndex = new RoomIndexType( request.RoomIndex );
                if ( Rooms.ContainsKey( roomIndex ) )
                {
                    Room room = Rooms[ roomIndex ];
                    if ( room.Players.Count < room.Setting.MaxPlayers )
                    {
                        room.Players.Add( player );
                        roomInfo = room.Info;
                        player.OnJoinRoom( room );

                        result = JoinRoomResult.Success;
                    }
                }
            }

            ResponseJoinRoom response = new ResponseJoinRoom();
            response.Result = result;
            response.Room = roomInfo;

            SendCommand( player, ActionType.JoinRoom, response );
        }

        private void PlayerQuitRoom( IHanabiPlayer player, RequestQuitRoom request )
        {
            RoomInfo roomInfo = new RoomInfo();
            QuitRoomResult result = QuitRoomResult.Fail;

            if ( player.RoomStatus != PlayerRoomStatus.Idle )
            {
                RoomIndexType roomIndex = new RoomIndexType( request.RoomIndex );
                if ( Rooms.ContainsKey( roomIndex ) )
                {
                    Room room = Rooms[ roomIndex ];
                    room.Players.Remove( player );
                    roomInfo = room.Info;
                    player.OnQuitRoom();

                    if ( room.Players.Count == 0 )
                    {
                        room.Clear();
                    }
                    result = QuitRoomResult.Success;
                }
            }

            ResponseQuitRoom response = new ResponseQuitRoom();
            response.Result = result;
            response.Room = roomInfo;

            SendCommand( player, ActionType.QuitRoom, response );
        }

        private void PlayerReady( IHanabiPlayer player )
        {
            player.OnReady();

            SendCommand( player, ActionType.Ready );

            if ( Players.Count <= 1 )
            {
                return;
            }

            bool allReady = true;
            foreach ( var playerPair in Players )
            {
                if ( playerPair.Value.RoomStatus != PlayerRoomStatus.Ready )
                {
                    allReady = false;
                    break;
                }
            }

            if ( allReady )
            {
                GameBegin( player.Room.RoomIndex );
            }
        }

        private void GameBegin( RoomIndexType roomIndex )
        {
            Room room = Rooms[ roomIndex ];
            if ( room != null )
            {
                Status = GameStatus.Playing;

                room.BeginGame();
            }
        }

        private void PlayerPrompt( IHanabiPlayer player, RequestPromptCard prompt )
        {
            player.Room.Prompt( player, prompt );
        }

        private void PlayerPlayCard( IHanabiPlayer player, RequestPlayCard card )
        {
            player.Room.Play( player, card );
        }

        private void PlayerDiscardCard( IHanabiPlayer player, RequestDiscardCard card )
        {
            player.Room.Discard( player, card );
        }

        private void SendCommand( IHanabiPlayer player, ActionType action )
        {
            Command command = new Command();
            command.Action = action.ToString();
            command.Payload = null;
            player.SendCommand( command );
        }

        private void SendCommand<T>( IHanabiPlayer player, ActionType action, T response )
        {
            Command command = new Command();
            command.Action = action.ToString();
            command.Payload = JsonConvert.SerializeObject( response );
            player.SendCommand( command );
        }
    }
}
