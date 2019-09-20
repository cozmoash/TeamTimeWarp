using System.Threading;

namespace TeamTimeWarp.Rest.Authentication
{
    internal class AccountIdsProvider : IAccountIdsProvider
    {    
        public int GetAccountId()
        {
            var principal = (AuthenticationPrincipal)Thread.CurrentPrincipal;
            var authentication = principal.Authentication;
            return authentication.AccountId;
        }
    }
}