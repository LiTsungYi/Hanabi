namespace GameCore.Types
{
    public abstract class IdType<T>
    {
        public IdType( T value )
        {
            this.IdTypeValue = value;
        }

        protected T IdTypeValue
        {
            get;
            private set;
        }

        public static bool operator ==( IdType<T> lhs, IdType<T> rhs )
        {
            if ( ( object ) lhs == null && ( object ) rhs == null )
            {
                return true;
            }
            else if ( ( object ) lhs == null || ( object ) rhs == null )
            {
                return false;
            }
            return lhs.Equals( rhs );
        }

        public static bool operator !=( IdType<T> lhs, IdType<T> rhs )
        {
            return !( lhs == rhs );
        }

        public override bool Equals( object o )
        {
            IdType<T> that = o as IdType<T>;
            if ( ( object ) that == null )
            {
                return false;
            }
            return this.IdTypeValue.Equals( that.IdTypeValue );
        }

        public override int GetHashCode()
        {
            return this.IdTypeValue.GetHashCode();
        }
    }
}
