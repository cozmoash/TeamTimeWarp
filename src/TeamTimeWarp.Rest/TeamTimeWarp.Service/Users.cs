using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Persistence;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;

namespace TeamTimeWarp.Service
{
    public class Users : IUsers
    {
        private readonly IDictionary<long,TimeWarpUser> _users;

        public Users(IEnumerable<TimeWarpUser> users)
        {
            _users = new ConcurrentDictionary<long, TimeWarpUser>(users.ToDictionary(x => x.Account.Id, x => x));
        }

        public Users(ITimeWarpStateCalculator timeWarpStateCalculator)
            : this(GetUsersFromRepository(timeWarpStateCalculator))
        {

        }

        public void Add(TimeWarpUser user)
        {
            _users.Add(user.Account.Id,user);
        }

        public TimeWarpUser Get(long accountId)
        {
            return _users[accountId];
        }



        private static IEnumerable<TimeWarpUser> GetUsersFromRepository(ITimeWarpStateCalculator timeWarpStateCalculator)
        {

            IList<TimeWarpUserState> userStates;

            //todo: optimize this crap.
            using (var session = new NHibernateSessionPerRequest<TimeWarpUserStateClassMap>())
            {
                var accountRespository = new TimeWarpAccountRepository(session.GetCurrentSession());
                var accounts = accountRespository.GetAll();

                var userStateRepository = new TimeWarpUserStateRepository(session.GetCurrentSession());
                
                userStates = new List<TimeWarpUserState>();
                foreach(var account in accounts)
                {
                    var latestState = userStateRepository.GetLatestStateByAccountId(account.Id);
                    if (latestState != null)
                    {
                        NHibernateUtil.Initialize(latestState.Account);
                        userStates.Add(userStateRepository.GetLatestStateByAccountId(account.Id));
                    }
                }
    
            }

            return userStates.Select(x => new TimeWarpUser(x, timeWarpStateCalculator));
        }
    }
}