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
            const string headerName = "login-token";
            return GetHeader(requestMessage, headerName);
        }

        public static bool TryGetPassword(this HttpRequestMessage requestMessage,out string password)
        {
            const string headerName = "password";
            IEnumerable<string> passwordHeaders;
            if (requestMessage.Headers.TryGetValues(headerName, out passwordHeaders))
            {
                password = passwordHeaders.SingleOrDefault();
                return true;
            }
            password = null;
            return false;
        }

        public static string GetEmailAddress(this HttpRequestMessage requestMessage)
        {
            const string headerName = "email-address";
            return GetHeader(requestMessage, headerName);
        }

        public static string GetHeader(this HttpRequestMessage requestMessage, string headerName)
        {
            IEnumerable<string> passwordHeaders;
            if (!requestMessage.Headers.TryGetValues(headerName, out passwordHeaders))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {ReasonPhrase = "No password in header"});

            return passwordHeaders.SingleOrDefault();
        }
    }
}