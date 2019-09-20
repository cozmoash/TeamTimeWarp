using System.Security;
using RestSharp;

namespace TimeManager.Client.Tray
{
    public class UiAuthenticationService : IUiAuthenticationService
    {
        private readonly IRestServiceUriFactory _restServiceUriFactory;

        public UiAuthenticationService(IRestServiceUriFactory restServiceUriFactory)
        {
            _restServiceUriFactory = restServiceUriFactory;
        }

        public LoginToken Login(string usernamd, SecureString password)
        {
            var uri = _restServiceUriFactory.Get();
            var restClient = new RestClient(uri);
            restClient
        }

        public void Logout()
        {
            
        }

    }
}