using System.Threading;

namespace TeamTimeWarp.Client.Core
{
    public interface ITokenPersister
    {
        bool TokenExists();
        void RemoveToken();
        LoginToken GetToken();
        void PersistToken(LoginToken tokenStore);
    }

    public class NullTokenPersiter : ITokenPersister
    {
        public bool TokenExists()
        {
            return false;
        }

        public void RemoveToken()
        {
            
        }

        public LoginToken GetToken()
        {
            return null;
        }

        public void PersistToken(LoginToken tokenStore)
        {
        }
    }

    public class TokenStore
    {
        private LoginToken _token;
        public virtual LoginToken Token
        {
            get { return _token; }
            set
            {
                Interlocked.Exchange(ref _token, value);
            }
        }
    }
}