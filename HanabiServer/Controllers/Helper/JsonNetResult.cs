using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Hanabi.Server.Controllers.Helper
{
    public class JsonNetResult<T> : ActionResult
    {
        public Encoding ContentEncoding
        {
            get;
            private set;
        }

        public string ContentType
        {
            get;
            private set;
        }

        public T Data
        {
            get;
            private set;
        }

        public JsonSerializerSettings SerializerSettings
        {
            get;
            private set;
        }

        public Formatting Formatting
        {
            get;
            private set;
        }

        public JsonNetResult( T data )
        {
            ContentEncoding = Encoding.UTF8;
            ContentType = "application/json";
            SerializerSettings = new JsonSerializerSettings();
            Formatting = Formatting.None;
            Data = data;
        }

        public override void ExecuteResult( ControllerContext context )
        {
            if ( context == null )
            {
                throw new ArgumentNullException( "context" );
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = ContentType;
            response.ContentEncoding = ContentEncoding;

            if ( Data != null )
            {
                JsonTextWriter writer = new JsonTextWriter( response.Output )
                {
                    Formatting = Formatting
                };

                JsonSerializer serializer = JsonSerializer.Create( SerializerSettings );
                serializer.Serialize( writer, Data );

                writer.Flush();
            }
        }
    }
}