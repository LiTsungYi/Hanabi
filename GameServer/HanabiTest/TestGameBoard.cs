using System.Collections.Generic;
using System.Diagnostics;
using GameCore.Types;
using Hanabi;
using Hanabi.GameCore;
using Hanabi.Types;
using Moq;
using NUnit.Framework;

namespace HanabiTest
{
    [TestFixture]
    internal class TestGameBoard
    {
        [Test]
        public void TestDrawPileSize()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            DrawPile drawPile = new DrawPile( settings );
            Debug.Assert( drawPile.size == 50 );
        }

        [Test]
        public void TestDrawPileDrawCardSize()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            DrawPile drawPile = new DrawPile( settings );
            drawPile.Draw();
            Debug.Assert( drawPile.size == 49 );
        }

        [Test]
        public void TestDrawPileDrawCardEmpty()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            DrawPile drawPile = new DrawPile( settings );

            int size = drawPile.size;
            for ( int count = 0; count < size; ++count )
            {
                drawPile.Draw();
            }
            Card card = drawPile.Draw();
            Debug.Assert( card == null );
        }

        [Test]
        public void TestDiscard_Success()
        {
            DiscardPile discardPile = new DiscardPile();
            Card card = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( discardPile.Discard( card ) );
        }

        [Test]
        public void TestDiscard_SuccessTwo()
        {
            DiscardPile discardPile = new DiscardPile();
            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Card card2 = new Card( new CardIdType( 1 ), new CardIndexType( 1 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( discardPile.Discard( card1 ) );
            Debug.Assert( discardPile.Discard( card2 ) );
        }

        [Test]
        public void TestDiscard_Fail()
        {
            DiscardPile discardPile = new DiscardPile();
            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Card card2 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( discardPile.Discard( card1 ) );
            Debug.Assert( !discardPile.Discard( card2 ) );
        }

        [Test]
        public void TestPlayPile_Success()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            PlayPile played = new PlayPile( settings );
            Card card = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( played.Play( card ) );
        }

        [Test]
        public void TestPlayPile_SuccessTwo()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            PlayPile played = new PlayPile( settings );
            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Card card2 = new Card( new CardIdType( 1 ), new CardIndexType( 1 ), CardColorType.Blue, CardValueType.Value2 );
            Debug.Assert( played.Play( card1 ) );
            Debug.Assert( played.Play( card2 ) );
        }

        [Test]
        public void TestPlayPile_FailSameCardValue()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            PlayPile played = new PlayPile( settings );
            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Card card2 = new Card( new CardIdType( 1 ), new CardIndexType( 1 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( played.Play( card1 ) );
            Debug.Assert( !played.Play( card2 ) );
        }

        [Test]
        public void TestPlayPile_FailNotContinueNumber()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            PlayPile played = new PlayPile( settings );
            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Card card2 = new Card( new CardIdType( 1 ), new CardIndexType( 1 ), CardColorType.Blue, CardValueType.Value3 );
            Debug.Assert( played.Play( card1 ) );
            Debug.Assert( !played.Play( card2 ) );
        }

        [Test]
        public void TestPlayPile_FailColorUnknown()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            PlayPile played = new PlayPile( settings );
            Card card = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Unknown, CardValueType.Value1 );
            Debug.Assert( !played.Play( card ) );
        }

        [Test]
        public void TestPlayPile_FailNumberUnknown()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            PlayPile played = new PlayPile( settings );
            Card card = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Unknown );
            Debug.Assert( !played.Play( card ) );
        }

        [Test]
        public void TestStormToken_TakeOne()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            Tokens tokens = new Tokens( settings );

            int size = tokens.storm;
            Debug.Assert( tokens.Punish() );
            Debug.Assert( tokens.storm == size - 1 );
        }

        [Test]
        public void TestStormToken_TakeAll()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            Tokens tokens = new Tokens( settings );

            int size = tokens.storm;
            for ( int count = 0; count < size; ++count )
            {
                Debug.Assert( tokens.Punish() );
            }
            Debug.Assert( tokens.storm == 0 );
        }

        [Test]
        public void TestStormToken_TakeAllPlusOne()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            Tokens tokens = new Tokens( settings );

            int size = tokens.storm;
            for ( int count = 0; count < size; ++count )
            {
                Debug.Assert( tokens.Punish() );
            }
            Debug.Assert( !tokens.Punish() );
        }

        [Test]
        public void TestNoteToken_UseOne()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            Tokens tokens = new Tokens( settings );

            int size = tokens.notes;
            Debug.Assert( tokens.Use() );
            Debug.Assert( tokens.notes == size - 1 );
        }

        [Test]
        public void TestNoteToken_UseAll()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            Tokens tokens = new Tokens( settings );

            int size = tokens.notes;
            for ( int count = 0; count < size; ++count )
            {
                Debug.Assert( tokens.Use() );
            }
            Debug.Assert( tokens.notes == 0 );
        }

        [Test]
        public void TestNoteToken_UseAllPlusOne()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            Tokens tokens = new Tokens( settings );

            int size = tokens.notes;
            for ( int count = 0; count < size; ++count )
            {
                Debug.Assert( tokens.Use() );
            }
            Debug.Assert( tokens.notes == 0 );
            Debug.Assert( !tokens.Use() );
        }

        [Test]
        public void TestNoteToken_RewardOne()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            Tokens tokens = new Tokens( settings );

            int size = tokens.notes;
            Debug.Assert( tokens.Use() );
            Debug.Assert( tokens.Reward() );
            Debug.Assert( tokens.notes == size );
        }

        [Test]
        public void TestNoteToken_RewardFull()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            Tokens tokens = new Tokens( settings );

            int size = tokens.notes;
            Debug.Assert( !tokens.Reward() );
            Debug.Assert( tokens.notes == size );
        }

        [Test]
        public void TestNoteToken_RewardEmpty()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );
            Tokens tokens = new Tokens( settings );

            int size = tokens.notes;
            for ( int count = 0; count < size; ++count )
            {
                Debug.Assert( tokens.Use() );
            }
            Debug.Assert( tokens.notes == 0 );
            Debug.Assert( tokens.Reward() );
            Debug.Assert( tokens.notes == 1 );
        }

        [Test]
        public void TestBoard_DrawSuccess()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );

            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player1" ) );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player2" ) );

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );
            int sizeBefore = board.size;
            Debug.Assert( board.Draw() != null );
            int sizeAfter = board.size;
            Debug.Assert( sizeBefore == sizeAfter + 1 );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_DrawFailed()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );

            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player1" ) );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player2" ) );

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );
            while ( board.size > 0 )
            {
                Debug.Assert( board.Draw() != null );
            }
            Debug.Assert( board.Draw() == null );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_PlaySuccess()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );

            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player1" ) );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player2" ) );

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );
            Card card = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( board.Play( card ) );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_PlaySuccessTwoValue()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );

            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player1" ) );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player2" ) );

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );
            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Card card2 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value2 );
            Debug.Assert( board.Play( card1 ) );
            Debug.Assert( board.Play( card2 ) );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_PlaySuccessTwoColor()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );

            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player1" ) );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player2" ) );

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );
            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Card card2 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Green, CardValueType.Value1 );
            Debug.Assert( board.Play( card1 ) );
            Debug.Assert( board.Play( card2 ) );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_PlayFailed()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );

            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player1" ) );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player2" ) );

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );
            Card card = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value2 );
            Debug.Assert( !board.Play( card ) );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_PlayFailedSame()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );

            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player1" ) );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player2" ) );

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );
            Card card = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( board.Play( card ) );
            Debug.Assert( !board.Play( card ) );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_DiscardSuccess()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );

            int maxHand = settings.GetMaxHandCards( players.Count );
            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player1" ) );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player2" ) );
            for ( int count = 0; count < maxHand; ++count )
            {
                player1.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
                player2.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
            }

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );

            Card card = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( board.Discard( card ) );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_DiscardSuccessTwo()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );
            int maxHand = settings.GetMaxHandCards( players.Count );
            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player1" ) );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( new NicknameType( "player2" ) );
            for ( int count = 0; count < maxHand; ++count )
            {
                player1.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
                player2.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
            }

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );

            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( board.Discard( card1 ) );
            Card card2 = new Card( new CardIdType( 1 ), new CardIndexType( 1 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( board.Discard( card2 ) );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_DiscardFailed()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );
            int maxHand = settings.GetMaxHandCards( players.Count );
            NicknameType nickname1 = new NicknameType( "player1" );
            NicknameType nickname2 = new NicknameType( "player2" );
            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname1 );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname2 );
            for ( int count = 0; count < maxHand; ++count )
            {
                player1.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
                player2.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
            }

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );

            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( board.Discard( card1 ) );
            Card card2 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Debug.Assert( !board.Discard( card2 ) );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_UseSuccess()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );
            int maxHand = settings.GetMaxHandCards( players.Count );
            NicknameType nickname1 = new NicknameType( "player1" );
            NicknameType nickname2 = new NicknameType( "player2" );
            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname1 );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname2 );
            for ( int count = 0; count < maxHand; ++count )
            {
                player1.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
                player2.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
            }

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );

            int maxHint = settings.initialHint;
            for ( int count = 0; count < maxHint; ++count )
            {
                Debug.Assert( board.Use() );
            }

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_UseFailed()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );
            int maxHand = settings.GetMaxHandCards( players.Count );
            NicknameType nickname1 = new NicknameType( "player1" );
            NicknameType nickname2 = new NicknameType( "player2" );
            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname1 );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname2 );
            for ( int count = 0; count < maxHand; ++count )
            {
                player1.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
                player2.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
            }

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );

            int maxHint = settings.initialHint;
            for ( int count = 0; count < maxHint; ++count )
            {
                Debug.Assert( board.Use() );
            }
            Debug.Assert( !board.Use() );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_RewardSuccess()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );
            int maxHand = settings.GetMaxHandCards( players.Count );
            NicknameType nickname1 = new NicknameType( "player1" );
            NicknameType nickname2 = new NicknameType( "player2" );
            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname1 );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname2 );
            for ( int count = 0; count < maxHand; ++count )
            {
                player1.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
                player2.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
            }

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );
            Debug.Assert( board.Use() );
            Debug.Assert( board.Reward() );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_RewardFailed()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );
            int maxHand = settings.GetMaxHandCards( players.Count );
            NicknameType nickname1 = new NicknameType( "player1" );
            NicknameType nickname2 = new NicknameType( "player2" );
            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname1 );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname2 );
            for ( int count = 0; count < maxHand; ++count )
            {
                player1.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
                player2.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
            }

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );
            Debug.Assert( !board.Reward() );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_PunishSuccess()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );
            int maxHand = settings.GetMaxHandCards( players.Count );
            NicknameType nickname1 = new NicknameType( "player1" );
            NicknameType nickname2 = new NicknameType( "player2" );
            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname1 );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname2 );
            for ( int count = 0; count < maxHand; ++count )
            {
                player1.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
                player2.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
            }

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );
            Debug.Assert( board.Punish() );

            player1.VerifyAll();
            player2.VerifyAll();
        }

        [Test]
        public void TestBoard_PunishFailed()
        {
            GameSettings settings = new GameSettings( DifficultLevel.Middle );

            var player1 = new Mock<IHanabiPlayer>();
            var player2 = new Mock<IHanabiPlayer>();

            List<IHanabiPlayer> players = new List<IHanabiPlayer>();
            players.Add( player1.Object );
            players.Add( player2.Object );
            int maxHand = settings.GetMaxHandCards( players.Count );
            NicknameType nickname1 = new NicknameType( "player1" );
            NicknameType nickname2 = new NicknameType( "player2" );
            player1.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname1 );
            player2.SetupGet<NicknameType>( x => x.nickname ).Returns( nickname2 );
            for ( int count = 0; count < maxHand; ++count )
            {
                player1.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
                player2.Setup( x => x.DrawCard( It.IsAny<Card>() ) );
            }

            GameBoard board = new GameBoard( DifficultLevel.Middle, players );
            int maxError = settings.maxError;
            for ( int count = 0; count < maxError; ++count )
            {
                Debug.Assert( board.Punish() );
            }
            Debug.Assert( !board.Punish() );

            player1.VerifyAll();
            player2.VerifyAll();
        }
    }
}
