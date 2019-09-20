using System.Net.Http;
using System.Web.Http;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Authentication;
using log4net;

namespace TeamTimeWarp.Rest.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IAccountCreator _accountCreator;

        private static readonly ILog Log = LogManager.GetLogger(typeof(AccountController));
        
        public AccountController(IAccountCreator accountCreator)
        {
            _accountCreator = accountCreator;
        }
        
        //quick logon.
        public AccountCreationResponse Post(HttpRequestMessage request, string name)
        {
            if (Log.IsDebugEnabled)
                Log.DebugFormat("new quick logon request for {0}", name);

            return _accountCreator.CreateAccount(new QuickCreationInfo
                {
                    DisplayName = name
                });
        }

        public AccountCreationResponse Post(HttpRequestMessage request, string name, string emailAddress)
        {
            if(Log.IsDebugEnabled)
                Log.DebugFormat("new account creation request for {0}", emailAddress);

            string password;
            request.TryGetPassword(out password);

            return
                _accountCreator.CreateAccount(new FullAccountCreationInfo
                    {
                        DisplayName = name, 
                        EmailAddress = emailAddress, 
                        Password = password
                    });
        }       
    }
}