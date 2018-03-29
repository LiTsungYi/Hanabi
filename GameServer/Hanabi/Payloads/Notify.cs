using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hanabi.Payloads
{
    public sealed class ResponseNotifyTurn
    {
        public string Nickname
        {
            get;
            set;
        }
    }

    public sealed class ResponseNotifyRound
    {
        public int Score
        {
            get;
            set;
        }
    }
}
