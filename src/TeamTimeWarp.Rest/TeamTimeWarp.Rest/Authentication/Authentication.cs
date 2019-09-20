namespace TeamTimeWarp.Rest.Authentication
{
    internal class Authentication : IAuthentication
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public Authentication(int id, int clientAccountId, string username, string token)
        {
            Id = id;
            AccountId = clientAccountId;
            Username = username;
            Token = token;            
        }
    }
}