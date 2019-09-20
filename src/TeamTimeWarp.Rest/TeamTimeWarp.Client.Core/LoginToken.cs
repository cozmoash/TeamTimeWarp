namespace TeamTimeWarp.Client.Core
{
    public class LoginToken
    {
        private readonly string _tokenStr;

        public LoginToken(string tokenStr)
        {
            _tokenStr = tokenStr;
        }

        public string TokenStr
        {
            get { return _tokenStr; }
        }
    }
}