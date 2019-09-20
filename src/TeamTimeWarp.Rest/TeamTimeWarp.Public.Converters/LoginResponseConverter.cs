using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Authentication;

namespace TeamTimeWarp.Public.Converters
{
    public static class LoginResponseConverter
    {
        public static LoginResponse ConvertToPublicV001(this ServiceLoginToken loginToken)
        {
            return new LoginResponse(loginToken.Token, loginToken.AccountId);
        }
    }
}