using GameCore;
using GameCore.Types;
using Hanabi;
using Hanabi.Types;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Hanabi.Payloads;
using Hanabi.GameCore;

namespace HanabiTest
{
    [TestFixture]
    internal class TestCommand
    {
        [Test]
        public void TestGame_EnterGameSuccess()
        {
            NicknameType nickname = new NicknameType( "Caesar" );
            Mock<IHanabiPlayer> player = new Mock<IHanabiPlayer>();
            player.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname );
            player.Setup( x => x.OnEnterGame( nickname ) );
            player.Setup( x => x.SendCommand( It.Is<Command>( c => VerifyEnterGame( c, ExitGameResult.Success ) ) ) );

            GameNameType gameName = new GameNameType( "Hanabi" );
            Game game = new Game( gameName );

            PlayerEnter( game, player.Object, nickname );

            player.VerifyAll();
        }

        [Test]
        public void TestGame_EnterGameFail()
        {
            NicknameType nickname = new NicknameType( "Caesar" );
            Mock<IHanabiPlayer> player1 = new Mock<IHanabiPlayer>();
            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname );
            player1.Setup( x => x.OnEnterGame( nickname ) );
            player1.Setup( x => x.SendCommand( It.Is<Command>( c => VerifyEnterGame( c, ExitGameResult.Success ) ) ) );

            Mock<IHanabiPlayer> player2 = new Mock<IHanabiPlayer>();
            player2.Setup( x => x.SendCommand( It.Is<Command>( c => VerifyEnterGame( c, ExitGameResult.Fail ) ) ) );

            GameNameType gameName = new GameNameType( "Hanabi" );
            Game game = new Game( gameName );

            PlayerEnter( game, player1.Object, nickname );
            PlayerEnter( game, player2.Object, nickname );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestGame_ExitGameSuccess()
        {
            NicknameType nickname = new NicknameType( "Caesar" );
            Mock<IHanabiPlayer> player = new Mock<IHanabiPlayer>();
            player.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname );
            player.Setup( x => x.OnEnterGame( nickname ) );
            player.Setup( x => x.SendCommand( It.Is<Command>( c => VerifyEnterGame( c, ExitGameResult.Success ) ) ) );
            player.Setup( x => x.OnExitGame() );
            player.Setup( x => x.SendCommand( It.Is<Command>( c => VerifyExitGame( c, ExitGameResult.Success ) ) ) );

            GameNameType gameName = new GameNameType( "Hanabi" );
            Game game = new Game( gameName );

            PlayerEnter( game, player.Object, nickname );
            PlayerExit( game, player.Object, nickname );

            player.VerifyAll();
        }

        [Test]
        public void TestGame_ExitGameFailed()
        {
            NicknameType nickname = new NicknameType( "Caesar" );
            Mock<IHanabiPlayer> player = new Mock<IHanabiPlayer>();
            player.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname );
            player.Setup( x => x.SendCommand( It.Is<Command>( c => VerifyExitGame( c, ExitGameResult.Fail ) ) ) );

            GameNameType gameName = new GameNameType( "Hanabi" );
            Game game = new Game( gameName );

            PlayerExit( game, player.Object, nickname );

            player.VerifyAll();
        }

        [Test]
        public void TestGame_GetRoomListEmptyList()
        {
            NicknameType nickname = new NicknameType( "Caesar" );
            Mock<IHanabiPlayer> player = new Mock<IHanabiPlayer>();
            player.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname );
            player.Setup( x => x.OnEnterGame( nickname ) );
            player.Setup( x => x.SendCommand( It.Is<Command>( c => VerifyEnterGame( c, ExitGameResult.Success ) ) ) );
            player.Setup( x => x.SendCommand( It.Is<Command>( c => VerifyGameList( c, 0 ) ) ) );

            GameNameType gameName = new GameNameType( "Hanabi" );
            Game game = new Game( gameName );

            PlayerEnter( game, player.Object, nickname );
            PlayerGetRoomList( game, player.Object );

            player.VerifyAll();
        }

        [Test]
        public void TestGame_GetRoomListOneRoom()
        {
            NicknameType nickname = new NicknameType( "Caesar" );
            Mock<IHanabiPlayer> player = new Mock<IHanabiPlayer>();
            player.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname );
            player.Setup( x => x.OnEnterGame( nickname ) );
            player.Setup( x => x.OnJoinRoom( It.IsAny<Room>() ) );
            int count = 0;
            player.Setup( x => x.SendCommand( It.IsAny<Command>() ) )
                  .Callback<Command>( c =>
            {
                switch ( ++count )
                {
                    case 1:
                        VerifyEnterGame( c, ExitGameResult.Success );
                        break;

                    case 2:
                        VerifyGameList( c, 1 );
                        break;
                }
            } );

            GameNameType gameName = new GameNameType( "Hanabi" );
            Game game = new Game( gameName );

            PlayerEnter( game, player.Object, nickname );
            PlayerGetRoomList( game, player.Object );

            player.VerifyAll();
        }

        [Test]
        public void TestGame_CreateRoomSuccess()
        {
            NicknameType nickname = new NicknameType( "Caesar" );
            Mock<IHanabiPlayer> player = new Mock<IHanabiPlayer>();
            player.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname );

            player.Setup( x => x.OnEnterGame( nickname ) );
            player.Setup( x => x.OnJoinRoom( It.IsAny<Room>() ) );
            int count = 0;
            player.Setup( x => x.SendCommand( It.IsAny<Command>() ) )
                  .Callback<Command>( c =>
                  {
                      switch ( ++count )
                      {
                          case 1:
                              VerifyEnterGame( c, ExitGameResult.Success );
                              break;
                          case 2:
                              VerifyCreateRoom( c, CreateRoomResult.Success );
                              break;
                      }
                  } );
            
            GameNameType gameName = new GameNameType( "Hanabi" );
            Game game = new Game( gameName );

            PlayerEnter( game, player.Object, nickname );

            player.VerifyAll();
        }

        [Test]
        public void TestGame_JoinRoomSuccess()
        {
            NicknameType nickname1 = new NicknameType( "Caesar" );
            NicknameType nickname2 = new NicknameType( "Jenny" );
            Mock<IHanabiPlayer> player1 = new Mock<IHanabiPlayer>();
            Mock<IHanabiPlayer> player2 = new Mock<IHanabiPlayer>();
            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname1 );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname2 );

            RoomIndexType index = new RoomIndexType( 0 );

            player1.Setup( x => x.OnEnterGame( nickname1 ) );
            player1.Setup( x => x.OnJoinRoom( It.Is<Room>( r => r.RoomIndex == index ) ) );
            int count1 = 0;
            player1.Setup( x => x.SendCommand( It.IsAny<Command>() ) )
                  .Callback<Command>( c =>
                  {
                      switch ( ++count1 )
                      {
                          case 1:
                              VerifyEnterGame( c, ExitGameResult.Success );
                              break;
                      }
                  } );

            player2.Setup( x => x.OnEnterGame( nickname2 ) );
            player2.Setup( x => x.OnJoinRoom( It.Is<Room>( r => r.RoomIndex == index ) ) );
            int count2 = 0;
            player2.Setup( x => x.SendCommand( It.IsAny<Command>() ) )
                  .Callback<Command>( c =>
                  {
                      switch ( ++count2 )
                      {
                          case 1:
                              VerifyEnterGame( c, ExitGameResult.Success );
                              break;
                          case 2:
                              VerifyJoinRoom( c, JoinRoomResult.Success, 2 );
                              break;
                      }
                  } );

            GameNameType gameName = new GameNameType( "Hanabi" );
            Game game = new Game( gameName );

            PlayerEnter( game, player1.Object, nickname1 );

            PlayerEnter( game, player2.Object, nickname2 );
            PlayerJoinRoom( game, player2.Object, index );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestGame_JoinRoomFail()
        {
            NicknameType nickname = new NicknameType( "Caesar" );
            Mock<IHanabiPlayer> player = new Mock<IHanabiPlayer>();
            player.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname );

            RoomIndexType index = new RoomIndexType( 0 );

            player.Setup( x => x.OnEnterGame( nickname ) );
            int count = 0;
            player.Setup( x => x.SendCommand( It.IsAny<Command>() ) )
                  .Callback<Command>( c =>
                  {
                      switch ( ++count )
                      {
                          case 1:
                              VerifyEnterGame( c, ExitGameResult.Success );
                              break;
                          case 2:
                              VerifyJoinRoom( c, JoinRoomResult.Fail, 0 );
                              break;
                      }
                  } );

            GameNameType gameName = new GameNameType( "Hanabi" );
            Game game = new Game( gameName );

            PlayerEnter( game, player.Object, nickname );
            PlayerJoinRoom( game, player.Object, index );

            player.VerifyAll();
        }

        [Test]
        public void TestGame_LeaveRoomSuccess()
        {
            NicknameType nickname = new NicknameType( "Caesar" );
            Mock<IHanabiPlayer> player = new Mock<IHanabiPlayer>();
            player.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname );

            player.Setup( x => x.OnEnterGame( nickname ) );
            player.Setup( x => x.OnJoinRoom( It.IsAny<Room>() ) );
            int count = 0;
            player.Setup( x => x.SendCommand( It.IsAny<Command>() ) )
                  .Callback<Command>( c =>
                  {
                      switch ( ++count )
                      {
                          case 1:
                              VerifyEnterGame( c, ExitGameResult.Success );
                              break;

                          case 2:
                              VerifyQuitRoom( c, QuitRoomResult.Success, 0 );
                              break;
                      }
                  } );

            GameNameType gameName = new GameNameType( "Hanabi" );
            Game game = new Game( gameName );

            PlayerEnter( game, player.Object, nickname );

            player.VerifyAll();
        }

        #region Private Functions
        private bool VerifyEnterGame( Command c, ExitGameResult result )
        {
            if ( c.action != ActionType.EnterGame.ToString() )
            {
                return false;
            }

            ResponseEnterGame response = JsonConvert.DeserializeObject<ResponseEnterGame>( c.payload );
            return response.result == result;
        }

        private bool VerifyExitGame( Command c, ExitGameResult result )
        {
            if ( c.action != ActionType.ExitGame.ToString() )
            {
                return false;
            }

            ResponseExitGame response = JsonConvert.DeserializeObject<ResponseExitGame>( c.payload );
            return response.result == result;
        }

        private bool VerifyGameList( Command c, int roomNumber )
        {
            if ( c.action != ActionType.GetRoomList.ToString() )
            {
                return false;
            }

            ResponseRoomList response = JsonConvert.DeserializeObject<ResponseRoomList>( c.payload );
            return ( response.rooms.Count == roomNumber );
        }

        private bool VerifyJoinRoom( Command c, JoinRoomResult result, int userCount )
        {
            if ( c.action != ActionType.JoinRoom.ToString() )
            {
                return false;
            }

            ResponseJoinRoom response = JsonConvert.DeserializeObject<ResponseJoinRoom>( c.payload );
            if ( response.result != result )
            {
                return false;
            }

            return ( response.result == JoinRoomResult.Fail ) 
                ? true : ( response.room.players.Count == userCount );
        }

        private bool VerifyQuitRoom( Command c, QuitRoomResult result, int userCount )
        {
            if ( c.action != ActionType.QuitRoom.ToString() )
            {
                return false;
            }

            ResponseQuitRoom response = JsonConvert.DeserializeObject<ResponseQuitRoom>( c.payload );
            if ( response.result != result )
            {
                return false;
            }

            return ( response.result == QuitRoomResult.Fail )
                ? true : ( response.room.players.Count == userCount );
        }

        private void PlayerEnter( Game game, IHanabiPlayer player, NicknameType nickname )
        {
            RequestEnterGame request = new RequestEnterGame();
            request.Nickname = nickname.nickname;

            Command command = new Command();
            command.action = ActionType.EnterGame.ToString();
            command.payload = JsonConvert.SerializeObject( request );
            game.DispatchRequest( player, JsonConvert.SerializeObject( command ) );
        }

        private void PlayerExit( Game game, IHanabiPlayer player, NicknameType nickname )
        {
            RequestExitGame request = new RequestExitGame();
            request.Nickname = nickname.nickname;

            Command command = new Command();
            command.action = ActionType.ExitGame.ToString();
            command.payload = JsonConvert.SerializeObject( request );
            game.DispatchRequest( player, JsonConvert.SerializeObject( command ) );
        }

        private void PlayerGetRoomList( Game game, IHanabiPlayer player )
        {
            RequestGetRoomList request = new RequestGetRoomList();

            Command command = new Command();
            command.action = ActionType.GetRoomList.ToString();
            command.payload = JsonConvert.SerializeObject( request );
            game.DispatchRequest( player, JsonConvert.SerializeObject( command ) );
        }

        private void PlayerJoinRoom( Game game, IHanabiPlayer player, RoomIndexType roomIndex )
        {
            RequestJoinRoom request = new RequestJoinRoom();
            request.RoomIndex = roomIndex.Index;

            Command command = new Command();
            command.action = ActionType.JoinRoom.ToString();
            command.payload = JsonConvert.SerializeObject( request );
            game.DispatchRequest( player, JsonConvert.SerializeObject( command ) );
        }
        #endregion
    }
}
