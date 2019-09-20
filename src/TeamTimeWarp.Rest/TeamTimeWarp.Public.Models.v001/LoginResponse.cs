using System.Runtime.Serialization;

namespace TeamTimeWarp.Public.Models.v001
{
    [DataContract]
    public class LoginResponse
    {
        public LoginResponse(string token, long accountId)
        {
            AccountId = accountId;
            Token = token;
        }

        [DataMember]
        public string Token { get; private set; }

        [DataMember]
        public long AccountId { get; private set; }
    }
}