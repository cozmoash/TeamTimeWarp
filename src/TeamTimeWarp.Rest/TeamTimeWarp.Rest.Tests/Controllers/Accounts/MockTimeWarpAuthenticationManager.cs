using System;
using System.Collections.Generic;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Rest.Authentication;

namespace TeamTimeWarp.Rest.Tests.Controllers.Accounts
{
    public class MockTimeWarpAuthenticationManager : ITimeWarpAuthenticationManager
    {
        private readonly Dictionary<string, long> _authenticatedTokens = new Dictionary<string, long>();

        public bool TryAuthenticate(string emailAddress, string password, out ServiceLoginToken token)
        {
           token = new ServiceLoginToken(Guid.NewGuid().ToString(),0);
            return true;
        }

        public string AddUser(AccountPassword accountPassword)
        {
            var guid =  Guid.NewGuid().ToString();
            _authenticatedTokens.Add(guid, accountPassword.Account.Id);
            return guid;
        }

        public bool TryAuthenticateForWriteOperation(string token, out long accountId)
        {
            if(_authenticatedTokens.ContainsKey(token))
            {
                accountId =  _authenticatedTokens[token];
                return true;

            }
            accountId = -1;
            return false;
        }

        public bool TryAuthenticateForReadOperation(string token, out long accountId)
        {
            return TryAuthenticateForWriteOperation(token, out accountId);
        }

        public bool Invalidate(string token)
        {
            return _authenticatedTokens.Remove(token);
        }
    }
}