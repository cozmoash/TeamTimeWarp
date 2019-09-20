using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Public.Converters;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Rest.Authentication;
using log4net;
using TimeWarpState = TeamTimeWarp.Public.Models.v001.TimeWarpState;

namespace TeamTimeWarp.Rest.Controllers
{
    public class UserStateController : ApiController
    {
        private readonly IUserStateManager _userStateManager;
        private readonly IAccountRepository _accountRepository;
        private readonly ITimeWarpAuthenticationManager _authenticationManager;
        private readonly INowProvider _nowProvider;

        private static readonly ILog Log = LogManager.GetLogger(typeof(UserStateController));

        public UserStateController(IUserStateManager userStateManager, INowProvider nowProvider, IAccountRepository accountRepository, ITimeWarpAuthenticationManager authenticationManager)
        {
            _userStateManager = userStateManager;
            _nowProvider = nowProvider;
            _accountRepository = accountRepository;
            _authenticationManager = authenticationManager;
        }
        
        public UserStateInfoResponse Get(HttpRequestMessage request)
        {
            long id;
            if (!_authenticationManager.TryAuthenticateForReadOperation(request.GetToken(), out id))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }

            var queryTime = _nowProvider.Now;

            TimeWarpUserState state;
            if (!_userStateManager.TryGetCurrentState(id, queryTime, out state))
            {
                if (Log.IsDebugEnabled)
                    Log.DebugFormat("no userState for account ({0}), returning default", id);

                return GetDefaultResponse(id, queryTime);
            }

            return state.ConvertToPublicV001(queryTime);
        }

        // PUT api/values/5
        public void Post(HttpRequestMessage request, TimeWarpCommand command, TimeWarpAgent agent = TimeWarpAgent.Unknown)
        {
            long id;
            if (!_authenticationManager.TryAuthenticateForWriteOperation(request.GetToken(), out id))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }

            if (Log.IsDebugEnabled)
                Log.DebugFormat("updating userState for account ({0}) with command ({1})", id, command);

            var requestTime = _nowProvider.Now;

            var account = _accountRepository.Get(id);
            if (account == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent));
            }

            switch (command)
            {
                case (TimeWarpCommand.Work):
                    _userStateManager.StartWork(account, requestTime,(int)agent);
                    break;
                case (TimeWarpCommand.Rest):
                    _userStateManager.StartRest(account, requestTime,(int)agent);
                    break;
                default:
                    throw new InvalidEnumArgumentException("command", (int)command, typeof(TimeWarpCommand));
            }
        }

        // PUT api/values/5
        //public void Post(HttpRequestMessage request, TimeWarpCommand command)
        //{
        //    Post(request,command,TimeWarpAgent.Unknown);
        //}

        private static UserStateInfoResponse GetDefaultResponse(long id, DateTime queryTime)
        {
            //this should never be run.
            return new UserStateInfoResponse(id, "test",queryTime, TimeWarpState.None, default(DateTime), default(TimeSpan),
                                             0,false,TimeWarpAgent.Unknown);
        }
    }
}