using System;
using System.Collections.Generic;
using GameCore.Types;
using Hanabi.GameCore.Detail;
using Hanabi.Types;

namespace Hanabi.GameCore
{
    public class GameBoard
    {
        public GameBoard( List<IHanabiPlayer> playerList )
        {
            Setting = new GameSettings();

            DrawPool = new DrawPile( Setting );
            DiscardedCards = new DiscardPile();
            PlayedCards = new PlayPile( Setting );
            Tokens = new GameTokens( Setting );
            Players = new Dictionary<NicknameType, IHanabiPlayer>();

            int maxHand = Setting.GetMaxHandCards( Players.Count );
            foreach ( var player in playerList )
            {
                this.Players.Add( player.Nickname, player );
                List<Card> handCard = new List<Card>();
                for ( int count = 0; count < maxHand; ++count )
                {
                    player.DrawCard( this.Draw() );
                }
            }
        }

        public GameSettings Setting
        {
            get;
            private set;
        }

        public int Size
        {
            get
            {
                return DrawPool.Size;
            }
        }

        public int Score
        {
            get
            {
                return ( Tokens.Storm == Setting.MaxError ) ? 0 : PlayedCards.Score;
            }
        }

        public GameTokens Tokens
        {
            get;
            private set;
        }

        private DrawPile DrawPool
        {
            get;
            set;
        }

        private DiscardPile DiscardedCards
        {
            get;
            set;
        }

        private PlayPile PlayedCards
        {
            get;
            set;
        }

        private Dictionary<NicknameType, IHanabiPlayer> Players
        {
            get;
            set;
        }

        /// <summary>
        /// 打亂陣列
        /// </summary>
        /// <typeparam name="T">陣列的型別</typeparam>
        /// <param name="array">要打亂的陣列</param>
        public static void RandomSort<T>( T[] array )
        {
            Random random = new Random();
            for ( int count = 0; count < array.Length; ++count )
            {
                int index = random.Next( count, array.Length );
                if ( count != index )
                {
                    T temp = array[ count ];
                    array[ count ] = array[ index ];
                    array[ index ] = temp;
                }
            }
        }

        public Card Draw()
        {
            return DrawPool.Draw();
        }

        public bool Play( Card card )
        {
            return PlayedCards.Play( card );
        }

        public bool Discard( Card card )
        {
            return DiscardedCards.Discard( card );
        }

        public bool Reward()
        {
            return Tokens.Reward();
        }

        /// <summary>
        /// 使用提示
        /// </summary>
        /// <returns>傳回 true 表示提示指示物足夠</returns>
        public bool Use()
        {
            return Tokens.Use();
        }

        /// <summary>
        /// 出牌錯誤懲罰
        /// </summary>
        /// <returns>傳回 false 表示錯誤太多</returns>
        public bool Punish()
        {
            return Tokens.Punish();
        }
    }
}
