using System;
using System.ComponentModel;
using System.Net;
using System.Security;
using System.Threading;
using RestSharp;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services
{
    public class AuthenticationService : UiServiceBase, IUiAuthenticationService
    {
        private readonly ITokenPersister _tokenPersister;
        private long _accountId;
        private bool _isQuickLogin;
        public event EventHandler<AsyncCompletedEventArgs> LoginCompleted;

        public AuthenticationService(TokenStore tokenStore, IRestServiceUriFactory restServiceUriFactory,ITokenPersister tokenPersister, SynchronizationContext synchronizationContext)
            : base(tokenStore, restServiceUriFactory,synchronizationContext)
        {
            _tokenPersister = tokenPersister;
        }

        public bool IsLoggedIn
        {
            get { return TokenStore.Token != null; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <exception cref="SecurityException">Thrown when trying to log in with the wrong credentials</exception>
        /// <exception cref="WebException">Thrown if cannot connect to the service.</exception>
        public void LoginAsync(string emailAddress, string password = null, bool rememberLogin = false)
        {
            _isQuickLogin = false;
            RestRequest request = CreateLoginRequest(emailAddress, password);
            ExecuteRequestAsync(request, restResponse =>
                {
                    var loginResponse = JsonHelper.SafeDeserializeObject<LoginResponse>(restResponse.Content);
                    
                    if (restResponse.StatusCode != HttpStatusCode.Forbidden && loginResponse != null &&
                        !string.IsNullOrEmpty(loginResponse.Token))
                    {
                        var token = new LoginToken(loginResponse.Token);
                        TokenStore.Token = token;
                        _accountId = loginResponse.AccountId;
                        RaiseLogonEvent(null);

                        if(rememberLogin)
                            ThreadPool.QueueUserWorkItem(PersistLoginToken, token);
                    }
                    else
                    {
                        RaiseLogonEvent(new SecurityException("unable to connect to service"));
                    }
                });
        }


        private void PersistLoginToken(object state)
        {
            try
            {
                _tokenPersister.PersistToken((LoginToken)state);
            }
            catch (Exception)
            {
                
            }
        }


        public void QuickLoginAsync(string username)
        {
            _isQuickLogin = true;
            var request = new RestRequest(string.Format("account/?name={0}", username), Method.POST);
            ExecuteRequestAsync(request,
                                restResponse =>
                                    {
                                        var token = JsonHelper.SafeDeserializeObject<AccountCreationResponse>(restResponse.Content);
                                        TokenStore.Token = new LoginToken(token.Token);
                                        _accountId = token.AccountId;
                                        AsyncCompletedEventArgsExtensions.Raise(LoginCompleted, restResponse, null);
                                    });
        }

        public long AccountId
        {
            get { return _accountId; }
        }

        public void Logout()
        {
            //if it's a quick login account, then logout once we are done here
            //if it's not a quick login account then stay logged in as other devices may
            //be using this.
            if (_isQuickLogin)
                ExecuteRequestAsync("logout", Method.POST,_=>{});
        }

        private void RaiseLogonEvent(Exception exception)
        {
            SynchronizationContext.Post(_ =>
                {
                    EventHandler<AsyncCompletedEventArgs> handler = LoginCompleted;
                    if (handler != null) handler(this, new AsyncCompletedEventArgs(exception,false,null));
                }, null);
        }

        private static RestRequest CreateLoginRequest(string emailAddress, string password = null)
        {
            var request = new RestRequest("logon", Method.POST);
            request.AddHeader("email-address", emailAddress);

            if(password != null)
                request.AddHeader("password", password);

            return request;
        }
    }
}