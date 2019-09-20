using System.Security;

namespace TimeManager.Client.Tray
{
    public class LoginToken
    {
        private readonly SecureString _token;

        public LoginToken(SecureString token)
        {
            _token = token;
        }

        public SecureString Token
        {
            get { return _token; }
        }
    }
}