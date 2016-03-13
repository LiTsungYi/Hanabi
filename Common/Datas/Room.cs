using System.Collections.Generic;

namespace Hanabi.Datas
{
    public class Room
    {
        public string GameName
        {
            get;
            set;
        }

        public GameSetting GameSetting
        {
            get;
            set;
        }

        public IList<Player> Players
        {
            get;
            set;
        }

        public Game Game
        {
            get;
            set;
        }
    }
}