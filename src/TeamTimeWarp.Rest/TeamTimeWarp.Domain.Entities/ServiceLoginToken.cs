namespace TeamTimeWarp.Rest.Authentication
{
    public class ServiceLoginToken
    {
        public ServiceLoginToken(string token, long accountId)
        {
            AccountId = accountId;
            Token = token;
        }

        public string Token { get; private set; }
        public long AccountId { get; private set; }
    }
}