using System;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Rest.Controllers;
using log4net;

namespace TeamTimeWarp.Rest.Authentication
{
    public class TimeWarpAuthenticationManager //: IAuthenticationManager
        : ITimeWarpAuthenticationManager
    {
        private readonly ILog _log;
        private readonly IAuthenticationSessionRepository _authenticationSessionRepository;
        private readonly IAccountPasswordRepository _accountPasswords;
        private readonly INowProvider _nowProvider;
        private readonly SessionIdProvider _sessionIdProvider;
        private readonly TimeSpan _sessionTimeOut = TimeSpan.FromDays(10);

        public TimeWarpAuthenticationManager(IAuthenticationSessionRepository authenticationSessionRepository, IAccountPasswordRepository accountPasswords, INowProvider nowProvider)
        {
            _accountPasswords = accountPasswords;
            _nowProvider = nowProvider;
            _log = LogManager.GetLogger(GetType());
            
            _sessionIdProvider = new SessionIdProvider();
            _authenticationSessionRepository = authenticationSessionRepository;
        }

        public bool TryAuthenticate(string emailAddress, string password, out ServiceLoginToken token)
        {
            AccountPassword accountPassword = _accountPasswords.GetByEmail(emailAddress);

            if (accountPassword == null)
            {
                token = null;
                return false;
            }

            if(accountPassword.Password != password)
            {
                token = null;
                return false;
            }

            var tokenStr = _sessionIdProvider.GetSessionId();
            var authenticationSession = new AuthenticationSession(accountPassword.Account, tokenStr,
                                                                  _nowProvider.Now);
            _authenticationSessionRepository.Add(authenticationSession);
            token = new ServiceLoginToken(tokenStr, accountPassword.Account.Id);
            return true;
        }

        public string AddUser(AccountPassword accountPassword)
        {
            var token = _sessionIdProvider.GetSessionId();

            var authenticationSession = new AuthenticationSession(accountPassword.Account, token,
                                                                  _nowProvider.Now);
            _authenticationSessionRepository.Add(authenticationSession);

            return token;
        }
        

        public bool TryAuthenticateForReadOperation(string token, out long accountId)
        {
            AuthenticationSession session;
            if (!_authenticationSessionRepository.TryGetByToken(token, out session))
            {
                accountId = -1;
                return false;
            }

            var now = _nowProvider.Now;

            if (TokenExpired(session, now))
            {
                accountId = -1;
                return false;
            }

            accountId = session.Account.Id;

            return true;
        }

        public bool TryAuthenticateForWriteOperation(string token, out long accountId)
        {
            AuthenticationSession session;
            if (!_authenticationSessionRepository.TryGetByToken(token, out session))
            {
                accountId = -1;
                return false;
            }
            
            var now = _nowProvider.Now;

            if (TokenExpired(session, now))
            {
                accountId = -1;
                return false;
            }

            session.LastValidation = _nowProvider.Now;
            _authenticationSessionRepository.Add(session);
            
            accountId = session.Account.Id;

            return true;
        }

        private bool TokenExpired(AuthenticationSession session, DateTime now)
        {
            return now.Subtract(session.LastValidation) > _sessionTimeOut;
        }

        public bool Invalidate(string token)
        {
            AuthenticationSession value;
            if (_authenticationSessionRepository.TryGetByToken(token, out value))
            {
                value.Token = null;
                _authenticationSessionRepository.Add(value);
                return true;
            }
            return false;
        }
    }
}