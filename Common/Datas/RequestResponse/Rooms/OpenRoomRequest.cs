using Hanabi.Datas;

namespace Hanabi.Datas.RequestResponse.Rooms
{
    public class OpenRoomRequest
    {
        public string GameName
        {
            get;
            set;
        }

        public string PlayerId
        {
            get;
            set;
        }

        public GameSetting GameSetting
        {
            get;
            set;
        }
    }
}