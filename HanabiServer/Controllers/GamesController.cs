using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Hanabi.Datas;
using Hanabi.Datas.RequestResponse.Games;
using Hanabi.Server.Controllers.Helper;
using Hanabi.Server.Models.Datas;

namespace Hanabi.Server.Controllers
{
    public class GamesController : ApiController
    {
        private static GameData Data
        {
            get;
            set;
        }

        static GamesController()
        {
            Data = GameData.Instance;
        }

        [HttpPost]
        public HttpResponseMessage Start( [ModelBinder( typeof( JsonNetModelBinder ) )] StartGameRequest request )
        {
            var room = Data.FindRoom( request.GameName );
            if ( room == null )
            {
                var startGameFailedResponse = new StartGameResponse()
                {
                    Result = false
                };
                return new JsonNetResponseMessage( startGameFailedResponse );
            }

            if ( room.Game != null )
            {
                var startGameFailedResponse = new StartGameResponse()
                {
                    Result = false
                };
                return new JsonNetResponseMessage( startGameFailedResponse );
            }

            room.Game = new Game( room.Players, room.GameSetting.Difficultly );

            var response = new StartGameResponse()
            {
                Result = true
            };
            return new JsonNetResponseMessage( response );
        }

        [HttpPost]
        public HttpResponseMessage Draw( [ModelBinder( typeof( JsonNetModelBinder ) )] DrawCardRequest request )
        {
            var response = new DrawCardResponse()
            {
                Result = true
            };
            return new JsonNetResponseMessage( response );
        }

        [HttpPost]
        public HttpResponseMessage Play( [ModelBinder( typeof( JsonNetModelBinder ) )] PlayCardRequest request )
        {
            var response = new PlayCardResponse()
            {
                Result = true
            };
            return new JsonNetResponseMessage( response );
        }

        [HttpPost]
        public HttpResponseMessage Discard( [ModelBinder( typeof( JsonNetModelBinder ) )] DiscardCardRequest request )
        {
            var response = new DiscardCardResponse()
            {
                Result = true
            };
            return new JsonNetResponseMessage( response );
        }

        [HttpPost]
        public HttpResponseMessage Prompt( [ModelBinder( typeof( JsonNetModelBinder ) )] PromptCardRequest request )
        {
            var response = new PromptCardResponse()
            {
                Result = true
            };
            return new JsonNetResponseMessage( response );
        }

        // NOTE: Server Send Event
        //       http://blogs.microsoft.co.il/gilf/2012/04/10/using-html5-server-sent-events-with-json-and-aspnet-mvc/
        /*
        public ActionResult Message()
        {
            var result = string.Empty;
            var sb = new StringBuilder();
            if ( _data.TryTake( out result, TimeSpan.FromMilliseconds( 1000 ) ) )
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var serializedObject = ser.Serialize( new
                {
                    item = result,
                    message = "hello"
                } );
                sb.AppendFormat( "data: {0}\n\n", serializedObject );
            }
            return Content( sb.ToString(), "text/event-stream" );
        }
        */
    }
}
