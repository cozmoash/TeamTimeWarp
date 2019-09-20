using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TeamTimeWarp.Rest.Controllers
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetToken(this HttpRequestMessage requestMessage)
        {
            IEnumerable<string> passwordHeaders;
            if (!requestMessage.Headers.TryGetValues("login-token", out passwordHeaders))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "No password in header" });

            return passwordHeaders.SingleOrDefault();
        }
    }
}