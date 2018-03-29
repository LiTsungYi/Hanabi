using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using GameCore;
using Fleck;
using Hanabi;
using Hanabi.Types;
using System.Diagnostics;
using Hanabi.GameCore;

namespace HanabiTest
{
    [TestFixture]
    internal class TestPlayer
    {
        [Test]
        public void TestPlayer_DrawCard()
        {
            Mock<IGame> game = new Mock<IGame>();
            Mock<IWebSocketConnection> socket = new Mock<IWebSocketConnection>();

            Player player = new Player( game.Object, socket.Object );

            Card card = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            player.DrawCard( card );

            Debug.Assert( player.handCards.Count == 1 );
            Debug.Assert( player.handCards[ 0 ].index == card.Index );
            Debug.Assert( player.handCards[ 0 ].card == card );
            Debug.Assert( player.handCards[ 0 ].hint.color == CardColorType.Unknown );
            Debug.Assert( player.handCards[ 0 ].hint.number == CardValueType.Unknown );
        }

        [Test]
        public void TestPlayer_PlayCard()
        {
            Mock<IGame> game = new Mock<IGame>();
            Mock<IWebSocketConnection> socket = new Mock<IWebSocketConnection>();

            Player player = new Player( game.Object, socket.Object );

            Card card = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            player.DrawCard( card );
            player.PlayCard( card.Index );

            Debug.Assert( player.handCards.Count == 0 );
        }

        [Test]
        public void TestPlayer_DiscardCard()
        {
            Mock<IGame> game = new Mock<IGame>();
            Mock<IWebSocketConnection> socket = new Mock<IWebSocketConnection>();

            Player player = new Player( game.Object, socket.Object );

            Card card = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            player.DrawCard( card );
            player.DiscardCard( card.Index );

            Debug.Assert( player.handCards.Count == 0 );
        }

        [Test]
        public void TestPlayer_HintCardColor1()
        {
            Mock<IGame> game = new Mock<IGame>();
            Mock<IWebSocketConnection> socket = new Mock<IWebSocketConnection>();

            Player player = new Player( game.Object, socket.Object );

            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Card card2 = new Card( new CardIdType( 1 ), new CardIndexType( 1 ), CardColorType.Blue, CardValueType.Value1 );
            Card card3 = new Card( new CardIdType( 2 ), new CardIndexType( 2 ), CardColorType.Blue, CardValueType.Value2 );
            Card card4 = new Card( new CardIdType( 3 ), new CardIndexType( 3 ), CardColorType.Green, CardValueType.Value1 );
            Card card5 = new Card( new CardIdType( 4 ), new CardIndexType( 4 ), CardColorType.Yellow, CardValueType.Value2 );
            player.DrawCard( card1 );
            player.DrawCard( card2 );
            player.DrawCard( card3 );
            player.DrawCard( card4 );
            player.DrawCard( card5 );
            player.HintCard( CardColorType.Blue );

            Debug.Assert( player.handCards.Count == 5 );
            CardInformation hint1 = player.GetHandCard( card1.Index ).hint;
            Debug.Assert( hint1.colorHints[CardColorType.Blue] == HintState.Possible );
            Debug.Assert( hint1.colorHints[CardColorType.Green] == HintState.Unknown );
            Debug.Assert( hint1.colorHints[ CardColorType.Yellow ] == HintState.Unknown );
            CardInformation hint2 = player.GetHandCard( card2.Index ).hint;
            Debug.Assert( hint2.colorHints[ CardColorType.Blue ] == HintState.Possible );
            Debug.Assert( hint2.colorHints[ CardColorType.Green ] == HintState.Unknown );
            Debug.Assert( hint2.colorHints[ CardColorType.Yellow ] == HintState.Unknown );
            CardInformation hint3 = player.GetHandCard( card3.Index ).hint;
            Debug.Assert( hint3.colorHints[ CardColorType.Blue ] == HintState.Possible );
            Debug.Assert( hint3.colorHints[ CardColorType.Green ] == HintState.Unknown );
            Debug.Assert( hint3.colorHints[ CardColorType.Yellow ] == HintState.Unknown );
            CardInformation hint4 = player.GetHandCard( card4.Index ).hint;
            Debug.Assert( hint4.colorHints[ CardColorType.Blue ] == HintState.Impossible );
            Debug.Assert( hint4.colorHints[ CardColorType.Green ] == HintState.Unknown );
            Debug.Assert( hint4.colorHints[ CardColorType.Yellow ] == HintState.Unknown );
            CardInformation hint5 = player.GetHandCard( card5.Index ).hint;
            Debug.Assert( hint5.colorHints[ CardColorType.Blue ] == HintState.Impossible );
            Debug.Assert( hint5.colorHints[ CardColorType.Green ] == HintState.Unknown );
            Debug.Assert( hint5.colorHints[ CardColorType.Yellow ] == HintState.Unknown );
        }

        [Test]
        public void TestPlayer_HintCardColor2()
        {
            Mock<IGame> game = new Mock<IGame>();
            Mock<IWebSocketConnection> socket = new Mock<IWebSocketConnection>();

            Player player = new Player( game.Object, socket.Object );

            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Card card2 = new Card( new CardIdType( 1 ), new CardIndexType( 1 ), CardColorType.Blue, CardValueType.Value1 );
            Card card3 = new Card( new CardIdType( 2 ), new CardIndexType( 2 ), CardColorType.Blue, CardValueType.Value2 );
            Card card4 = new Card( new CardIdType( 3 ), new CardIndexType( 3 ), CardColorType.Green, CardValueType.Value1 );
            Card card5 = new Card( new CardIdType( 4 ), new CardIndexType( 4 ), CardColorType.Yellow, CardValueType.Value2 );
            player.DrawCard( card1 );
            player.DrawCard( card2 );
            player.DrawCard( card3 );
            player.DrawCard( card4 );
            player.DrawCard( card5 );
            player.HintCard( CardColorType.Blue );
            player.HintCard( CardColorType.Green );

            Debug.Assert( player.handCards.Count == 5 );
            CardInformation hint1 = player.GetHandCard( card1.Index ).hint;
            Debug.Assert( hint1.colorHints[ CardColorType.Blue ] == HintState.Sure );
            Debug.Assert( hint1.colorHints[ CardColorType.Green ] == HintState.Impossible );
            Debug.Assert( hint1.colorHints[ CardColorType.Yellow ] == HintState.Impossible );
            CardInformation hint2 = player.GetHandCard( card2.Index ).hint;
            Debug.Assert( hint2.colorHints[ CardColorType.Blue ] == HintState.Sure );
            Debug.Assert( hint2.colorHints[ CardColorType.Green ] == HintState.Impossible );
            Debug.Assert( hint2.colorHints[ CardColorType.Yellow ] == HintState.Impossible );
            CardInformation hint3 = player.GetHandCard( card3.Index ).hint;
            Debug.Assert( hint3.colorHints[ CardColorType.Blue ] == HintState.Sure );
            Debug.Assert( hint3.colorHints[ CardColorType.Green ] == HintState.Impossible );
            Debug.Assert( hint3.colorHints[ CardColorType.Yellow ] == HintState.Impossible );
            CardInformation hint4 = player.GetHandCard( card4.Index ).hint;
            Debug.Assert( hint4.colorHints[ CardColorType.Blue ] == HintState.Impossible );
            Debug.Assert( hint4.colorHints[ CardColorType.Green ] == HintState.Sure );
            Debug.Assert( hint4.colorHints[ CardColorType.Yellow ] == HintState.Impossible );
            CardInformation hint5 = player.GetHandCard( card5.Index ).hint;
            Debug.Assert( hint5.colorHints[ CardColorType.Blue ] == HintState.Impossible );
            Debug.Assert( hint5.colorHints[ CardColorType.Green ] == HintState.Impossible );
            Debug.Assert( hint5.colorHints[ CardColorType.Yellow ] == HintState.Unknown );
        }

        [Test]
        public void TestPlayer_HintCardNumber()
        {
            Mock<IGame> game = new Mock<IGame>();
            Mock<IWebSocketConnection> socket = new Mock<IWebSocketConnection>();

            Player player = new Player( game.Object, socket.Object );

            Card card1 = new Card( new CardIdType( 0 ), new CardIndexType( 0 ), CardColorType.Blue, CardValueType.Value1 );
            Card card2 = new Card( new CardIdType( 1 ), new CardIndexType( 1 ), CardColorType.Blue, CardValueType.Value1 );
            Card card3 = new Card( new CardIdType( 2 ), new CardIndexType( 2 ), CardColorType.Blue, CardValueType.Value2 );
            Card card4 = new Card( new CardIdType( 3 ), new CardIndexType( 3 ), CardColorType.Green, CardValueType.Value1 );
            Card card5 = new Card( new CardIdType( 4 ), new CardIndexType( 4 ), CardColorType.Yellow, CardValueType.Value2 );
            player.DrawCard( card1 );
            player.DrawCard( card2 );
            player.DrawCard( card3 );
            player.DrawCard( card4 );
            player.DrawCard( card5 );
            player.HintCard( CardValueType.Value1 );

            Debug.Assert( player.handCards.Count == 5 );
            CardInformation hint1 = player.GetHandCard( card1.Index ).hint;
            Debug.Assert( hint1.numberHints[ CardValueType.Value1 ] == HintState.Sure );
            Debug.Assert( hint1.numberHints[ CardValueType.Value2 ] == HintState.Impossible );
            Debug.Assert( hint1.numberHints[ CardValueType.Value3 ] == HintState.Impossible );
            Debug.Assert( hint1.numberHints[ CardValueType.Value4 ] == HintState.Impossible );
            Debug.Assert( hint1.numberHints[ CardValueType.Value5 ] == HintState.Impossible );
            CardInformation hint2 = player.GetHandCard( card2.Index ).hint;
            Debug.Assert( hint2.numberHints[ CardValueType.Value1 ] == HintState.Sure );
            Debug.Assert( hint2.numberHints[ CardValueType.Value2 ] == HintState.Impossible );
            Debug.Assert( hint2.numberHints[ CardValueType.Value3 ] == HintState.Impossible );
            Debug.Assert( hint2.numberHints[ CardValueType.Value4 ] == HintState.Impossible );
            Debug.Assert( hint2.numberHints[ CardValueType.Value5 ] == HintState.Impossible );
            CardInformation hint3 = player.GetHandCard( card3.Index ).hint;
            Debug.Assert( hint3.numberHints[ CardValueType.Value1 ] == HintState.Impossible );
            Debug.Assert( hint3.numberHints[ CardValueType.Value2 ] == HintState.Unknown );
            Debug.Assert( hint3.numberHints[ CardValueType.Value3 ] == HintState.Unknown );
            Debug.Assert( hint3.numberHints[ CardValueType.Value4 ] == HintState.Unknown );
            Debug.Assert( hint3.numberHints[ CardValueType.Value5 ] == HintState.Unknown );
            CardInformation hint4 = player.GetHandCard( card4.Index ).hint;
            Debug.Assert( hint4.numberHints[ CardValueType.Value1 ] == HintState.Sure );
            Debug.Assert( hint4.numberHints[ CardValueType.Value2 ] == HintState.Impossible );
            Debug.Assert( hint4.numberHints[ CardValueType.Value3 ] == HintState.Impossible );
            Debug.Assert( hint4.numberHints[ CardValueType.Value4 ] == HintState.Impossible );
            Debug.Assert( hint4.numberHints[ CardValueType.Value5 ] == HintState.Impossible );
            CardInformation hint5 = player.GetHandCard( card5.Index ).hint;
            Debug.Assert( hint5.numberHints[ CardValueType.Value1 ] == HintState.Impossible );
            Debug.Assert( hint5.numberHints[ CardValueType.Value2 ] == HintState.Unknown );
            Debug.Assert( hint5.numberHints[ CardValueType.Value3 ] == HintState.Unknown );
            Debug.Assert( hint5.numberHints[ CardValueType.Value4 ] == HintState.Unknown );
            Debug.Assert( hint5.numberHints[ CardValueType.Value5 ] == HintState.Unknown );
        }
    }
}
