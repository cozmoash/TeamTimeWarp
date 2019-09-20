using System;
using System.Collections.Generic;
using System.Configuration;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Rest.Controllers;
using log4net;

namespace TeamTimeWarp.Rest.Authentication
{
    internal class TimeWarpAuthenticationManager //: IAuthenticationManager
    {
        private readonly object _instanceLocker = new object();
        private readonly ILog _log;
        private readonly Dictionary<string, AuthenticationSession> _authenticationSessionCache;
        private readonly IAccountRepository _accounts;

        private TimeWarpAuthenticationManager(IAccountRepository accounts)
        {
            _accounts = accounts;
            _log = LogManager.GetLogger(GetType());
            var sessionTimeOutMinutes = ConfigurationManager.AppSettings["SessionTimeOutMinutes"];
            SessionTimeOut = TimeSpan.FromMinutes(int.Parse(sessionTimeOutMinutes));
            TimeOutProvider = new NowProvider();
            SessionIdProvider = new SessionIdProvider();
            HeaderValueProvider = new HttpContextHeaderValueProvider();
            AccountOperatorProvider = new AccountIdsProvider();
            _authenticationSessionCache = new Dictionary<string, AuthenticationSession>();

            _log.Info("TimeWarpAuthenticationManager Instantiated");
        }

        public IAuthentication Authenticate(AuthenticationRequest request)
        {
            _log.DebugFormat("Authenticate called with: {0}", request.ToString());

            var token = SessionIdProvider.GetSessionId();
            
            //login here...
            //_accounts.


            //var authentication = new Authentication(0,converter.Convert<IAuthentication>(request, resultDto, token, lightstreamerConfiguration));
            //_log.DebugFormat("Login successful. Returning Authentication: {0}", authentication.ToDebugString());
            //lock (_instanceLocker)
            //{
            //    var authenticationSession = new AuthenticationSession(authentication, TimeOutProvider.Now);
            //    if (_authenticationSessionCache.ContainsKey(token))
            //    {
            //        _authenticationSessionCache[token] = authenticationSession;
            //    }
            //    else
            //    {
            //        _authenticationSessionCache.Add(token, authenticationSession);
            //    }
            //}
            

            //return authentication;
            return null;
        }

        public bool IsAuthenticated(string token)
        {
            var isAuthenticated = false;

            lock (_instanceLocker)
            {
                if (_authenticationSessionCache.ContainsKey(token))
                {
                    var authenticationSession = _authenticationSessionCache[token];
                    var now = TimeOutProvider.Now;
                    if(authenticationSession.LastValidation.Add(SessionTimeOut) > now)
                    {
                        authenticationSession.LastValidation = now;
                        isAuthenticated = true;
                    }
                    else
                    {
                        _authenticationSessionCache.Remove(token);
                    }
                }
            }

            return isAuthenticated;
        }

        public bool Invalidate(string token)
        {
            var invalidated = false;

            lock (_instanceLocker)
            {
                if (IsAuthenticated(token))
                {
                    _authenticationSessionCache.Remove(token);
                    invalidated = true;
                }
            }

            return invalidated;
        }

        public AuthenticationPrincipal GetPrincipal(string token, string authenticationType)
        {
            var principal = AuthenticationPrincipal.UnAuthenticatedPrincipal;

            lock (_instanceLocker)
            {
                if (IsAuthenticated(token))
                {
                    var authenticationSession = _authenticationSessionCache[token];
                    var authentication = (Authentication)authenticationSession.Authentication;
                    var identity = new AuthenticationIdentity(authentication.Username, authenticationType, true);
                    //var roles = RolesProvider.GetRoles(authentication.AccountOperatorId, authentication.AccountId);
                    principal = new AuthenticationPrincipal(authentication, identity,new List<string>());
                }
            }

            return principal;
        }

        internal TimeSpan SessionTimeOut { get; set; }
        internal INowProvider TimeOutProvider { get; set; }
        internal ISessionIdProvider SessionIdProvider { get; set; }

        internal IHeaderValueProvider HeaderValueProvider { get; set; }

        internal AccountIdsProvider AccountOperatorProvider { get; set; }
    }
}