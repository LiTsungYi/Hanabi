namespace Hanabi.Datas
{
    public enum Difficultly
    {
        None,
        Easy,
        Normal,
        Hard,
        Hill,
    }

    public class GameSetting
    {
        public int MinPlayer
        {
            get;
            set;
        }

        public int MaxPlayer
        {
            get;
            set;
        }

        public Difficultly Difficultly
        {
            get;
            set;
        }
    }
}