using System.Collections.Generic;
using System.Linq;
using Hanabi.Datas;

namespace Hanabi.Server.Models.Datas
{
    public class GameData
    {
        private GameData()
        {
            Rooms = new Dictionary<string, Room>();
            Players = new Dictionary<string, Player>();
        }

        public static GameData Instance
        {
            get
            {
                if ( m_instance == null )
                {
                    m_instance = new GameData();
                }

                return m_instance;
            }
        }

        private static GameData m_instance = null;

        public IDictionary<string, Room> Rooms
        {
            get;
            private set;
        }

        public IDictionary<string, Player> Players
        {
            get;
            private set;
        }

        public Room FindRoom( string gameName )
        {
            return Rooms.Where( r => r.Key == gameName ).FirstOrDefault().Value;
        }

        public bool HasRoom( string gameName )
        {
            return Rooms.Where( r => r.Key == gameName ).Any();
        }

        public Player FindPlayer( string playerId )
        {
            return Players.Where( p => p.Key == playerId ).FirstOrDefault().Value;
        }

        public bool HasPlayer( string playerId )
        {
            return Players.Where( p => p.Key == playerId ).Any();
        }
    }
}