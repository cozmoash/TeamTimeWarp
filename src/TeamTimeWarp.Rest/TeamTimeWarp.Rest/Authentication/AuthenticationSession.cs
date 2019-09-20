using System;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Rest.Authentication
{
    internal class AuthenticationSession
    {
        public AuthenticationSession(Account account, string token, DateTime lastValidation)
        {
            LastValidation = lastValidation;
            Token = token;
            Account = account;
        }

        public Account Account { get; private set; }
        public string Token { get; private set; }
        public DateTime LastValidation { get; private set; }

       
    }
}