using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanabi.Datas
{
    public enum TokenType
    {
        None,
        Storm,
        Scroll
    }
    
    public enum PlayCardResult
    {
        None,
        Invalid,
        Error,
        Success,
    }

    public class Game
    {
        public Game( IList<Player> players, Difficultly difficultly )
        {
            Players = players;
            Difficultly = difficultly;
            DrawPile = new List<Card>();
            DiscardPile = new List<Card>();

            PlayerHand = new Dictionary<string, IList<Card>>();
            LastActionPlayers = new List<string>();
            foreach ( var player in Players )
            {
                PlayerHand.Add( player.Name, new List<Card>() );
                LastActionPlayers.Add( player.Name );
            }

            PlayArea = new Dictionary<CardColor, IList<Card>>()
            {
                { CardColor.White, new List<Card>() },
                { CardColor.Red, new List<Card>() },
                { CardColor.Blue, new List<Card>() },
                { CardColor.Yellow, new List<Card>() },
                { CardColor.Green, new List<Card>() },
            };

            if ( Rule.UseColorful( difficultly ) )
            {
                PlayArea.Add( CardColor.Colorful, new List<Card>() );
            }

            Tokens = new Dictionary<TokenType, int>()
            {
                { TokenType.Storm, 0 },
                { TokenType.Scroll, Rule.GetMaxScroll( difficultly ) }
            };

            LastRound = false;
            GameEnd = false;

            DispatchCards();
        }

        public IList<Player> Players
        {
            get;
            private set;
        }

        public Difficultly Difficultly
        {
            get;
            private set;
        }

        public IList<Card> DrawPile
        {
            get;
            private set;
        }

        public IList<Card> DiscardPile
        {
            get;
            private set;
        }

        public IDictionary<string, IList<Card>> PlayerHand
        {
            get;
            private set;
        }

        public IDictionary<CardColor, IList<Card>> PlayArea
        {
            get;
            private set;
        }

        public IDictionary<TokenType, int> Tokens
        {
            get;
            private set;
        }

        public bool LastRound
        {
            get;
            private set;
        }

        public bool GameEnd
        {
            get;
            private set;
        }

        private IList<string> LastActionPlayers
        {
            get;
            set;
        }

        private void DispatchCards()
        {
            var numbers = Enum.GetValues( typeof( CardNumber ) );
            foreach ( var color in PlayArea.Keys )
            {
                foreach ( CardNumber number in numbers )
                {
                    if ( number == CardNumber.None )
                    {
                        continue;
                    }

                    for ( var count = 0; count < Rule.GetCardNumber( number ); ++ count )
                    {
                        var card = new Card( color, number, Difficultly );
                        DrawPile.Add( card );
                    }
                }
            }

            var random = new Random();
            for ( var count = 0; count < 10000; ++ count )
            {
                var index = random.Next( 0, DrawPile.Count );

                var card = DrawPile[ index ];
                DrawPile.RemoveAt( index );
                DrawPile.Add( card );
            }
            
            for ( var count = 0; count < DrawPile.Count; ++ count )
            {
                var card = DrawPile[ count ];
                card.CardId = count;
            }

            foreach ( var player in Players )
            {
                for ( var count = 0; count < Rule.GetHandLimit( Players.Count ); ++ count )
                {
                    DrawCard( player );
                }
            }
        }

        private Card DrawCard( Player player )
        {
            if ( PlayerHand.Count > Rule.GetHandLimit( Players.Count ) )
            {
                return null;
            }

            if ( DrawPile.Count == 0 )
            {
                return null;
            }

            var card = DrawPile[ 0 ];
            DrawPile.RemoveAt( 0 );

            if ( DrawPile.Count == 0 )
            {
                LastRound = true;
            }

            PlayerHand[ player.Name ].Add( card );
            return card;
        }

        public PlayCardResult PlayCard( Player player, int cardId )
        {
            if ( GameEnd )
            {
                return PlayCardResult.Invalid;
            }

            IList<Card> cards;
            if ( !PlayerHand.TryGetValue( player.Name, out cards ) )
            {
                return PlayCardResult.Invalid;
            }

            var card = cards.Where( c => c.CardId == cardId ).FirstOrDefault();
            if ( card == null )
            {
                return PlayCardResult.Invalid;
            }

            cards.Remove( card );
            var playedCards = PlayArea[ card.Color ].LastOrDefault();
            if ( ( playedCards == null && card.Number != CardNumber.Number1 ) ||
                ( playedCards.Number + 1 != card.Number ) )
            {
                DiscardPile.Add( card );
                ++Tokens[ TokenType.Storm ];

                return PlayCardResult.Error;
            }
            
            PlayArea[ card.Color ].Add( card );
            if ( card.Number == CardNumber.Number5 )
            {
                ++Tokens[ TokenType.Scroll ];
                if ( Tokens[ TokenType.Scroll ] > Rule.GetMaxScroll( Difficultly ) )
                {
                    Tokens[ TokenType.Scroll ] = Rule.GetMaxScroll( Difficultly );
                }
            }

            if ( LastRound )
            {
                LastActionPlayers.Remove( player.Name );
                if ( LastActionPlayers.Count == 0 )
                {
                    GameEnd = true;
                }
            }

            DrawCard( player );
            return PlayCardResult.Success;
        }

        public PlayCardResult DiscardCard( Player player, int cardId )
        {
            if ( GameEnd )
            {
                return PlayCardResult.Invalid;
            }

            IList<Card> cards;
            if ( !PlayerHand.TryGetValue( player.Name, out cards ) )
            {
                return PlayCardResult.Invalid;
            }

            var card = cards.Where( c => c.CardId == cardId ).FirstOrDefault();
            if ( card == null )
            {
                return PlayCardResult.Invalid;
            }

            cards.Remove( card );
            DiscardPile.Add( card );

            ++Tokens[ TokenType.Scroll ];
            if ( Tokens[ TokenType.Scroll ] > Rule.GetMaxScroll( Difficultly ) )
            {
                Tokens[ TokenType.Scroll ] = Rule.GetMaxScroll( Difficultly );
            }

            if ( LastRound )
            {
                LastActionPlayers.Remove( player.Name );
                if ( LastActionPlayers.Count == 0 )
                {
                    GameEnd = true;
                }
            }

            DrawCard( player );
            return PlayCardResult.Success;
        }

        public PlayCardResult Prompt( Player player, Player partner, CardColor color )
        {
            if ( GameEnd )
            {
                return PlayCardResult.Invalid;
            }

            IList<Card> cards;
            if ( !PlayerHand.TryGetValue( partner.Name, out cards ) )
            {
                return PlayCardResult.Invalid;
            }

            if ( Tokens[ TokenType.Scroll ] == 0 )
            {
                return PlayCardResult.Invalid;
            }

            --Tokens[ TokenType.Scroll ];

            foreach ( var card in cards )
            {
                if ( card.Color == color )
                {
                    card.Info.PromptIs( color );
                }
                else
                {
                    card.Info.PromptNot( color );
                }
            }

            if ( LastRound )
            {
                LastActionPlayers.Remove( player.Name );
                if ( LastActionPlayers.Count == 0 )
                {
                    GameEnd = true;
                }
            }

            return PlayCardResult.Success;
        }

        public PlayCardResult Prompt( Player player, Player partner, CardNumber number )
        {
            if ( GameEnd )
            {
                return PlayCardResult.Invalid;
            }

            IList<Card> cards;
            if ( !PlayerHand.TryGetValue( partner.Name, out cards ) )
            {
                return PlayCardResult.Invalid;
            }

            if ( Tokens[ TokenType.Scroll ] == 0 )
            {
                return PlayCardResult.Invalid;
            }

            --Tokens[ TokenType.Scroll ];

            foreach ( var card in cards )
            {
                if ( card.Number == number )
                {
                    card.Info.PromptIs( number );
                }
                else
                {
                    card.Info.PromptNot( number );
                }
            }

            if ( LastRound )
            {
                LastActionPlayers.Remove( player.Name );
                if ( LastActionPlayers.Count == 0 )
                {
                    GameEnd = true;
                }
            }

            return PlayCardResult.Success;
        }
    }
}
