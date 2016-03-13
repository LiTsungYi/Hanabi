using System.Net.Http;

namespace Hanabi.Server.Controllers.Helper
{
    public class JsonNetResponseMessage : HttpResponseMessage
    {
        public JsonNetResponseMessage( object value )
        {
            Content = new JsonNetContent( value );
        }
    }
}