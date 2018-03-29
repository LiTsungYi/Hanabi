namespace GameCore.Types
{
    public class GameNameType : IdType<string>
    {
        public GameNameType( string gameName )
            : base( gameName )
        {
        }

        public string GameName
        {
            get
            {
                return this.IdTypeValue;
            }
        }
    }
}
