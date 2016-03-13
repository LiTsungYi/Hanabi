using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;

namespace Hanabi.Server.Controllers.Helper
{
    public class JsonNetModelBinder : IModelBinder
    {
        public bool BindModel( HttpActionContext actionContext, ModelBindingContext bindingContext )
        {
            string data = actionContext.Request.Content.ReadAsStringAsync().Result;
            if ( data == null )
            {
                bindingContext.ModelState.AddModelError( bindingContext.ModelName, "Null data" );
                return false;
            }

            MethodInfo method = typeof( JsonConvert ).GetMethods().Where( m => m.Name == "DeserializeObject" && m.IsGenericMethod && m.GetParameters().Length == 1 ).First();
            if ( method == null )
            {
                bindingContext.ModelState.AddModelError( bindingContext.ModelName, "Cannot DeserializeObject" );
                return false;
            }

            method = method.MakeGenericMethod( bindingContext.ModelType );
            bindingContext.Model = method.Invoke( null, new[] { data } );

            return true;
        }
    }
}