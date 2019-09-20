using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace TeamTimeWarp.Rest.Controllers
{
    public class ClientVersionController : ApiController
    {
        public int Get(string clientType)
        {
            if (clientType == "windows.tray")
                return 1;
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "no client type" });
        }
    }

    public class VersionController : ApiController
    {
        public string Get()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}