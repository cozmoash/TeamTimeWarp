using System;
using System.Threading;
using RestSharp;

namespace TeamTimeWarp.Client.Core
{
    public abstract class UiServiceBase
    {
        protected readonly TokenStore TokenStore;
        protected RestClient RestClient;
        protected readonly SynchronizationContext SynchronizationContext;

        protected UiServiceBase(TokenStore tokenStore, IRestServiceUriFactory restServiceUriFactory,
                                SynchronizationContext synchronizationContext = null)
        {
            TokenStore = tokenStore;
            SynchronizationContext = synchronizationContext;
            RestClient = new RestClient(restServiceUriFactory.Get());
            
        }

        protected RestRequestAsyncHandle ExecuteRequestAsync(string uri, Method method, Action<IRestResponse> callback)
        {
            RestClient.UseSynchronizationContext = true;
            var request = CreateRequest(uri, method);
            return RestClient.ExecuteAsync(request, callback);
        }

        protected RestRequestAsyncHandle ExecuteRequestAsync(IRestRequest restRequest, Action<IRestResponse> callback)
        {
            RestClient.UseSynchronizationContext = true;
            return RestClient.ExecuteAsync(restRequest, callback);
        }

        protected IRestRequest CreateRequest(string uri, Method method)
        {
            var request =
                new RestRequest(uri, method);

            if(TokenStore.Token != null)
                request.AddHeader("login-token", TokenStore.Token.TokenStr);
            
            return request;
        }
    }
}