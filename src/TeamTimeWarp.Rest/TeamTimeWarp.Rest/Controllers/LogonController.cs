using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Public.Converters;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Authentication;
using log4net;

namespace TeamTimeWarp.Rest.Controllers
{

    public class LogoutController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LogonController));
        private readonly ITimeWarpAuthenticationManager _authenticationManager;
        private readonly IAccountRepository _accountRepository;
        private readonly IRoomRepository _roomRepository;

        public LogoutController(ITimeWarpAuthenticationManager authenticationManager, IAccountRepository accountRepository, IRoomRepository roomRepository)
        {
            _authenticationManager = authenticationManager;
            _accountRepository = accountRepository;
            _roomRepository = roomRepository;
        }


        public void Post(HttpRequestMessage request)
        {
            var token = request.GetToken();

            if (string.IsNullOrWhiteSpace(token))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "no token" });

            Log.DebugFormat("logout for token ({0})",token);

            long accountId;
            if (_authenticationManager.TryAuthenticateForWriteOperation(token, out accountId))
            {
                _authenticationManager.Invalidate(token);

                var account = _accountRepository.Get(accountId);
                if (account.AccountType == AccountType.Quick)
                {
                    var rooms = _roomRepository.GetRooms(accountId);
                    foreach (var room in rooms)
                    {
                        room.Remove(account);
                        _roomRepository.Add(room);
                    }
                }
            }
        }
    }

    public class LogonController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LogonController));
        private readonly ITimeWarpAuthenticationManager _authenticationManager;

        public LogonController(ITimeWarpAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        public LoginResponse Post(HttpRequestMessage authenticationRequest)
        {
            var emailAddress = authenticationRequest.GetEmailAddress();
            string password;
            bool hasPassword = authenticationRequest.TryGetPassword(out password);
            
            if(string.IsNullOrWhiteSpace(emailAddress))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) {ReasonPhrase = "no email"});

            if (hasPassword && string.IsNullOrWhiteSpace(password))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "no password" });
            
            if(Log.IsDebugEnabled)
                Log.DebugFormat("logon for ({0})", emailAddress);

            ServiceLoginToken token;
            if (!_authenticationManager.TryAuthenticate(emailAddress, password, out token))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));

            return token.ConvertToPublicV001();
        }



    }
}