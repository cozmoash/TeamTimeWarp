using System.ComponentModel;
using System.Web.Http;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Public.Converters;
using TeamTimeWarp.Public.Models.v001;
using TeamTimeWarp.Service;

namespace TeamTimeWarp.Rest.Controllers
{
    public class TimeWarpUserController : ApiController
    {
        private readonly IUsers _users;
        private readonly INowProvider _nowProvider;
        private readonly ITimeWarpStatePersistence _persistence;

        public TimeWarpUserController(IUsers users, INowProvider nowProvider, ITimeWarpStatePersistence persistence)
        {
            _users = users;
            _nowProvider = nowProvider;
            _persistence = persistence;
        }

        public UserStateInfoResponse Get(long id)
        {
            var queryTime = _nowProvider.Now;
            var state = _users.Get(id).CurrentState(queryTime);

            return state.ConvertToPublicV001(queryTime);
        }
        
        // PUT api/values/5
        public void Post(long id, TimeWarpCommand command)
        {
            var requestTime = _nowProvider.Now;

            var user = _users.Get(id);

            TimeWarpUserState newState;
            switch (command)
            {
                case(TimeWarpCommand.Work):
                    newState = user.StartWork(requestTime );
                    break;
                case(TimeWarpCommand.Rest):
                    newState = user.StartRest(_nowProvider.Now);
                    break;
                default:
                    throw new InvalidEnumArgumentException("command", (int)command, typeof(TimeWarpCommand));
            }

            _persistence.SaveState(newState);
        }
    }
}