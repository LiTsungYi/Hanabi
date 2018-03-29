using System.Diagnostics;
using Hanabi;
using Hanabi.GameCore;
using Hanabi.Types;
using Moq;
using NUnit.Framework;

namespace HanabiTest
{
    [TestFixture]
    internal class TestGameRule
    {
        [Test]
        public void TestGameRule_PlayCardSuccess()
        {
            var player = new Mock<IHanabiPlayer>();
            CardIdType id = new CardIdType( 0 );
            CardIndexType index = new CardIndexType( 0 );
            Card card = new Card( id, index, CardColorType.Blue, CardValueType.Value1 );
            HandCard handCard = new HandCard( card );
            player.Setup( x => x.GetHandCard( index ) ).Returns( handCard );
            player.Setup( x => x.PlayCard( index ) );

            var board = new Mock<IGameBoard>();
            board.Setup( x => x.Draw() ).Returns<Card>( null );
            board.Setup( x => x.Play( card ) ).Returns( true );

            GameRule rule = new GameRule();
            Debug.Assert( rule.PlayCard( index, player.Object, board.Object ) == PlayCardResult.Success );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_PlayCardSuccessDrawCard()
        {
            var player = new Mock<IHanabiPlayer>();
            CardIdType id = new CardIdType( 0 );
            CardIndexType index = new CardIndexType( 0 );
            Card card = new Card( id, index, CardColorType.Blue, CardValueType.Value1 );
            HandCard handCard = new HandCard( card );
            player.Setup( x => x.GetHandCard( index ) ).Returns( handCard );
            player.Setup( x => x.PlayCard( index ) );

            var board = new Mock<IGameBoard>();
            CardIdType newId = new CardIdType( 1 );
            CardIndexType newIndex = new CardIndexType( 1 );
            Card newCard = new Card( newId, newIndex, CardColorType.Blue, CardValueType.Value1 );
            board.Setup( x => x.Draw() ).Returns( newCard );
            board.Setup( x => x.Play( card ) ).Returns( true );

            GameRule rule = new GameRule();
            Debug.Assert( rule.PlayCard( index, player.Object, board.Object ) == PlayCardResult.Success );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_PlayCardSuccessWithBonus()
        {
            var player = new Mock<IHanabiPlayer>();
            CardIdType id = new CardIdType( 0 );
            CardIndexType index = new CardIndexType( 0 );
            Card card = new Card( id, index, CardColorType.Blue, CardValueType.Value5 );
            HandCard handCard = new HandCard( card );
            player.Setup( x => x.GetHandCard( index ) ).Returns( handCard );
            player.Setup( x => x.PlayCard( index ) );

            var board = new Mock<IGameBoard>();
            board.Setup( x => x.Draw() ).Returns<Card>( null );
            board.Setup( x => x.Play( card ) ).Returns( true );
            board.Setup( x => x.Reward() );

            GameRule rule = new GameRule();
            Debug.Assert( rule.PlayCard( index, player.Object, board.Object ) == PlayCardResult.SuccessWithBonus );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_PlayCardFailedInvalidIndex()
        {
            var player = new Mock<IHanabiPlayer>();
            CardIndexType index = new CardIndexType( 0 );
            player.Setup( x => x.GetHandCard( index ) ).Returns<HandCard>( null );

            var board = new Mock<IGameBoard>();

            GameRule rule = new GameRule();
            Debug.Assert( rule.PlayCard( index, player.Object, board.Object ) == PlayCardResult.InvalidCardIndex );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_PlayCardFailedNoSlot()
        {
            var player = new Mock<IHanabiPlayer>();
            CardIdType id = new CardIdType( 0 );
            CardIndexType index = new CardIndexType( 0 );
            Card card = new Card( id, index, CardColorType.Blue, CardValueType.Value1 );
            HandCard handCard = new HandCard( card );
            player.Setup( x => x.GetHandCard( index ) ).Returns( handCard );
            player.Setup( x => x.PlayCard( index ) );

            var board = new Mock<IGameBoard>();
            board.Setup( x => x.Draw() ).Returns<Card>( null );
            board.Setup( x => x.Play( card ) ).Returns( false );
            board.Setup( x => x.Discard( card ) );
            board.Setup( x => x.Punish() );

            GameRule rule = new GameRule();
            Debug.Assert( rule.PlayCard( index, player.Object, board.Object ) == PlayCardResult.FailNoSlot );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_DiscardCardSuccess()
        {
            var player = new Mock<IHanabiPlayer>();
            CardIdType id = new CardIdType( 0 );
            CardIndexType index = new CardIndexType( 0 );
            Card card = new Card( id, index, CardColorType.Blue, CardValueType.Value1 );
            HandCard handCard = new HandCard( card );
            player.Setup( x => x.GetHandCard( index ) ).Returns( handCard );
            player.Setup( x => x.PlayCard( index ) );

            var board = new Mock<IGameBoard>();
            board.Setup( x => x.Draw() ).Returns<Card>( null );
            board.Setup( x => x.Discard( handCard.card ) );
            board.Setup( x => x.Reward() );

            GameRule rule = new GameRule();
            Debug.Assert( rule.DiscardCard( index, player.Object, board.Object ) == DiscardResult.Success );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_DiscardCardSuccessDrawCard()
        {
            var player = new Mock<IHanabiPlayer>();
            CardIdType id = new CardIdType( 0 );
            CardIndexType index = new CardIndexType( 0 );
            Card card = new Card( id, index, CardColorType.Blue, CardValueType.Value1 );
            HandCard handCard = new HandCard( card );
            player.Setup( x => x.GetHandCard( index ) ).Returns( handCard );
            player.Setup( x => x.PlayCard( index ) );

            var board = new Mock<IGameBoard>();
            CardIdType newId = new CardIdType( 1 );
            CardIndexType newIndex = new CardIndexType( 1 );
            Card newCard = new Card( newId, newIndex, CardColorType.Blue, CardValueType.Value1 );
            board.Setup( x => x.Draw() ).Returns( newCard );
            board.Setup( x => x.Discard( handCard.card ) );
            board.Setup( x => x.Reward() );

            GameRule rule = new GameRule();
            Debug.Assert( rule.DiscardCard( index, player.Object, board.Object ) == DiscardResult.Success );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_DiscardCardFailed()
        {
            var player = new Mock<IHanabiPlayer>();
            CardIndexType index = new CardIndexType( 0 );
            player.Setup( x => x.GetHandCard( index ) ).Returns<Card>( null );

            var board = new Mock<IGameBoard>();

            GameRule rule = new GameRule();
            Debug.Assert( rule.DiscardCard( index, player.Object, board.Object ) == DiscardResult.InvalidCardIndex );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_HintCardColorSuccess()
        {
            var player = new Mock<IHanabiPlayer>();
            player.Setup( x => x.HintCard( CardColorType.Blue ) );

            var board = new Mock<IGameBoard>();
            GameSettings settings = new GameSettings();
            board.SetupGet<GameSettings>( x => x.settings ).Returns( settings );
            board.Setup( x => x.Use() ).Returns( true );

            GameRule rule = new GameRule();
            Debug.Assert( rule.HintCard( CardColorType.Blue, player.Object, board.Object ) == HintCardResult.Success );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_HintCardColorFailedNoHint()
        {
            var player = new Mock<IHanabiPlayer>();

            var board = new Mock<IGameBoard>();
            GameSettings settings = new GameSettings();
            board.SetupGet<GameSettings>( x => x.settings ).Returns( settings );
            board.Setup( x => x.Use() ).Returns( false );

            GameRule rule = new GameRule();
            Debug.Assert( rule.HintCard( CardColorType.Blue, player.Object, board.Object ) == HintCardResult.HintEmpty );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_HintCardColorFailedUnknown()
        {
            var player = new Mock<IHanabiPlayer>();
            var board = new Mock<IGameBoard>();

            GameRule rule = new GameRule();
            Debug.Assert( rule.HintCard( CardColorType.Unknown, player.Object, board.Object ) == HintCardResult.InvalidColor );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_HintCardNumberSuccess()
        {
            var player = new Mock<IHanabiPlayer>();
            player.Setup( x => x.HintCard( CardValueType.Value1 ) );

            var board = new Mock<IGameBoard>();
            board.Setup( x => x.Use() ).Returns( true );

            GameRule rule = new GameRule();
            Debug.Assert( rule.HintCard( CardValueType.Value1, player.Object, board.Object ) == HintCardResult.Success );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_HintCardNumberFailedNoHint()
        {
            var player = new Mock<IHanabiPlayer>();

            var board = new Mock<IGameBoard>();
            board.Setup( x => x.Use() ).Returns( false );

            GameRule rule = new GameRule();
            Debug.Assert( rule.HintCard( CardValueType.Value1, player.Object, board.Object ) == HintCardResult.HintEmpty );

            player.VerifyAll();
            board.VerifyAll();
        }

        [Test]
        public void TestGameRule_HintCardNumberFailedUnknown()
        {
            var player = new Mock<IHanabiPlayer>();
            var board = new Mock<IGameBoard>();

            GameRule rule = new GameRule();
            Debug.Assert( rule.HintCard( CardValueType.Unknown, player.Object, board.Object ) == HintCardResult.InvalidNumber );

            player.VerifyAll();
            board.VerifyAll();
        }
    }
}
