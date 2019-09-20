using System;
using System.Configuration;
using System.Web;

namespace TeamTimeWarp.Rest.Controllers
{
    internal class ApiConstants
    {
        public const string ApiAuthorizationHeader = "Authorization";
     //   public const string ApiAuthorizationBrokerHeader = "Broker";

        public const string ApiVersionHeader = "X-API-Version";
        //public const string ApiProductTypeHeader = "X-API-ProductType";
       // public const string ApiTradingAccoutIdHeader = "X-API-TradingAccoutId";
    }

    internal enum ApiVersion
    {
        v001,
    }

    internal interface IHeaderValueProvider
    {
        string GetValue(string name);

        ApiVersion GetVersion();

        string GetHostHeaderValue();

        bool IsHttps();
    }

    internal class HttpContextHeaderValueProvider : IHeaderValueProvider
    {
        public string GetValue(string name)
        {
            string value = null;

            try
            {
                value = HttpContext.Current.Request.Headers[name];
            }
            catch (HttpException)
            {
                //Use the default if exception occurs when accessing HttpContext
            }

            return value;
        }

        public ApiVersion GetVersion()
        {
            ApiVersion version;

            var versionHeaderValue = GetValue(ApiConstants.ApiVersionHeader);

            var parsed = Enum.TryParse(versionHeaderValue, out version);
            if (!parsed)
            {
                throw new ArgumentException(string.Format("The {0} Header value of {1} was not recognised", ApiConstants.ApiVersionHeader, versionHeaderValue));
            }

            return version;
        }

        public string GetHostHeaderValue()
        {
            var hostHeader = ConfigurationManager.AppSettings["DefaultHostHeader"];

            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Url.Host != "localhost")
                {
                    hostHeader = httpRequest.Url.Host;
                }
            }
            catch (HttpException)
            {
                //Use the default if exception occurs when accessing HttpContext
            }

            hostHeader = hostHeader.ToLower();
            return hostHeader;
        }

        public bool IsHttps()
        {
            var isHttps = false;

            try
            {
                var httpRequest = HttpContext.Current.Request;
                isHttps = httpRequest.IsSecureConnection;
            }
            catch (HttpException)
            {
                //Use the default if exception occurs when accessing HttpContext
            }

            return isHttps;
        }
    }
}