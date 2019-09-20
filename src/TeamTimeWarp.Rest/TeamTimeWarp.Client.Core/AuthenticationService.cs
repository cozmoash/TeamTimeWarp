using System.Net;
using System.Security;
using Newtonsoft.Json;
using RestSharp;
using TeamTimeWarp.Client.Core.Services;

namespace TeamTimeWarp.Client.Core
{
    public class AuthenticationService : UiServiceBase, IUiAuthenticationService
    {
        public AuthenticationService(TokenStore tokenStore, IRestServiceUriFactory restServiceUriFactory)
            : base(tokenStore, restServiceUriFactory)
        {
            
        }

        public void Login(string emailAddress, string password)
        {
            var request = CreateLoginRequest(emailAddress, password);

            IRestResponse result = RestClient.Execute(request);

            if(result.StatusCode == HttpStatusCode.Forbidden)
                throw new SecurityException("unable to connect to service, incorrect credentials");

            var token = JsonConvert.DeserializeObject<string>(result.Content);
            TokenStore.Token = new LoginToken(token);
        }

        private static RestRequest CreateLoginRequest(string emailAddress, string password)
        {
            var request = new RestRequest("login", Method.POST);
            request.AddHeader("password", password);
            request.AddHeader("email-address", emailAddress);
            return request;
        }

        public void Logout()
        {
            ExecuteRequest("logout", Method.POST);
        }
    }
}