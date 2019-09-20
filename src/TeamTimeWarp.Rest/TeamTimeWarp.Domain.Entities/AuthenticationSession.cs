using System;

namespace TeamTimeWarp.Domain.Entities
{
    public class AuthenticationSession
    {
        protected AuthenticationSession()
        {
            
        }

        public AuthenticationSession(Account account, string token, DateTime lastValidation)
        {
            LastValidation = lastValidation;
            Token = token;
            Account = account;
        }

        public virtual long Id { get; protected set; }

        public virtual Account Account { get; protected set; }
        public virtual string Token { get; set; }
        public virtual DateTime LastValidation { get; set; }

       
    }
}