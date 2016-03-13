using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hanabi.Server.Controllers.Helper
{
    /// <summary>
    /// Ref. http://stackoverflow.com/questions/14231877/how-to-return-json-object-on-web-api-controller
    /// </summary>
    public class JsonNetContent : HttpContent
    {
        private readonly MemoryStream stream = new MemoryStream();

        public JsonNetContent( object value )
        {
            Headers.ContentType = new MediaTypeHeaderValue( "application/json" );
            var jw = new JsonTextWriter( new StreamWriter( stream ) );
            jw.Formatting = Formatting.None;
            var serializer = new JsonSerializer();
            serializer.Serialize( jw, value );
            jw.Flush();
            stream.Position = 0;
        }

        protected override Task SerializeToStreamAsync( Stream toStream, TransportContext context )
        {
            return stream.CopyToAsync( toStream );
        }

        protected override bool TryComputeLength( out long length )
        {
            length = stream.Length;
            return true;
        }
    }
}