using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanabi.Datas
{
    public static class Rule
    {
        public static int MinPlayer
        {
            get
            {
                return 2;
            }
        }

        public static int MaxPlayer
        {
            get
            {
                return 5;
            }
        }

        public static int GetCardNumber( CardNumber number )
        {
            switch ( number )
            {
            case CardNumber.Number1:
                return 3;

            case CardNumber.Number2:
            case CardNumber.Number3:
            case CardNumber.Number4:
                return 2;

            case CardNumber.Number5:
                return 1;

            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        public static int GetHandLimit( int playerNumber )
        {
            if ( ( playerNumber < MinPlayer ) || ( playerNumber > MaxPlayer ) )
            {
                throw new ArgumentOutOfRangeException();
            }

            return ( playerNumber > 3 ) ? 4 : 5;
        }

        public static int GetMaxStorm( Difficultly difficultly )
        {
            switch ( difficultly )
            {
            case Difficultly.Easy:
                return 4;

            case Difficultly.Normal:
            case Difficultly.Hard:
            case Difficultly.Hill:
                return 3;

            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        public static int GetMaxScroll( Difficultly difficultly )
        {
            switch ( difficultly )
            {
            case Difficultly.Easy:
            case Difficultly.Normal:
                return 9;

            case Difficultly.Hard:
            case Difficultly.Hill:
                return 8;

            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        public static bool UseColorful( Difficultly difficultly )
        {
            switch ( difficultly )
            {
            case Difficultly.Easy:
            case Difficultly.Normal:
            case Difficultly.Hard:
                return false;

            case Difficultly.Hill:
                return true;

            default:
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
