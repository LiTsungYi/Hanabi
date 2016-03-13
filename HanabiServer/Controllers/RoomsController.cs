using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Hanabi.Datas;
using Hanabi.Datas.RequestResponse.Rooms;
using Hanabi.Server.Controllers.Helper;
using Hanabi.Server.Models.Datas;

namespace Hanabi.Server.Controllers
{
    public class RoomsController : ApiController
    {
        private static GameData Data
        {
            get;
            set;
        }

        static RoomsController()
        {
            Data = GameData.Instance;
        }

        [HttpGet]
        public IEnumerable<Room> List()
        {
            return Data.Rooms.Values;
        }

        [HttpPost]
        public HttpResponseMessage Open( [ModelBinder( typeof( JsonNetModelBinder ) )] OpenRoomRequest request )
        {
            if ( Data.HasRoom( request.GameName ) || Data.HasPlayer( request.PlayerId ) )
            {
                var openRoomFailedResponse = new OpenRoomResponse()
                {
                    Result = false
                };
                return new JsonNetResponseMessage( openRoomFailedResponse );
            }

            var room = new Room()
            {
                GameName = request.GameName,
                GameSetting = request.GameSetting,
                Players = new List<Player>()
            };

            var player = new Player()
            {
                Name = request.PlayerId,
                RoomName = room.GameName
            };

            room.Players.Add( player );

            Data.Rooms.Add( room.GameName, room );
            Data.Players.Add( player.Name, player );

            var response = new OpenRoomResponse()
            {
                Result = true,
                Room = room
            };
            return new JsonNetResponseMessage( response );
        }

        [HttpPost]
        public HttpResponseMessage Join( [ModelBinder( typeof( JsonNetModelBinder ) )] JoinRoomRequest request )
        {
            var room = Data.FindRoom( request.GameName );
            if ( room == null )
            {
                var joinRoomFailedResponse = new JoinRoomResponse()
                {
                    Result = false
                };
                return new JsonNetResponseMessage( joinRoomFailedResponse );
            }

            var player = Data.FindPlayer( request.PlayerId );
            if ( player == null )
            {
                player = new Player()
                {
                    Name = request.PlayerId,
                    RoomName = request.GameName
                };

                Data.Players.Add( player.Name, player );
            }
            room.Players.Add( player );

            var response = new JoinRoomResponse()
            {
                Result = true,
                Room = room
            };
            return new JsonNetResponseMessage( response );
        }

        [HttpPost]
        public HttpResponseMessage Leave( [ModelBinder( typeof( JsonNetModelBinder ) )] LeaveRoomRequest request )
        {
            if ( !Data.HasPlayer( request.PlayerId ) )
            {
                var leaveRoomFailedResponse = new LeaveRoomResponse()
                {
                    Result = false
                };
                return new JsonNetResponseMessage( leaveRoomFailedResponse );
            }

            var player = Data.FindPlayer( request.PlayerId );
            if ( player == null )
            {
                var leaveRoomFailedResponse = new LeaveRoomResponse()
                {
                    Result = false
                };
                return new JsonNetResponseMessage( leaveRoomFailedResponse );
            }

            var roomName = player.RoomName;
            Room room = null;
            if ( !Data.Rooms.TryGetValue( roomName, out room ) )
            {
                var leaveRoomFailedResponse = new LeaveRoomResponse()
                {
                    Result = false
                };
                return new JsonNetResponseMessage( leaveRoomFailedResponse );
            }

            room.Players.Remove( player );

            var response = new LeaveRoomResponse()
            {
                Result = true
            };
            return new JsonNetResponseMessage( response );
        }
    }
}
