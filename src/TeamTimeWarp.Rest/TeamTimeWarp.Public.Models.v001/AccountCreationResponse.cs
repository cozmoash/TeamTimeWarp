using System.Runtime.Serialization;

namespace TeamTimeWarp.Public.Models.v001
{
    [DataContract]
    public class AccountCreationResponse
    {
        public AccountCreationResponse(long accountId, string token)
        {
            AccountId = accountId;
            Token = token;
        }

        public AccountCreationResponse()
        {
        }

        [DataMember]
        public long AccountId { get; private set; }

        [DataMember]
        public string Token { get; private set; }
    }
}