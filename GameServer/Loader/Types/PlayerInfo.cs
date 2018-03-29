namespace GameCore.Types
{
    public class NicknameType : IdType<string>
    {
        public NicknameType( string nickname )
            : base( nickname )
        {
        }

        public string Value
        {
            get
            {
                return this.IdTypeValue;
            }
        }
    }
}
