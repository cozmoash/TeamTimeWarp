using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace TeamTimeWarp.Rest.Tests.Controllers.Mocks
{
    public static class HttpRequestMock
    {        
        public static HttpRequestMessage MockRequest()
        {
            var request = new HttpRequestMessage();
            
            request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            return request;
        }
    }
}