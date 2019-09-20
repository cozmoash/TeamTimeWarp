using System.Collections.Generic;
using System.Linq;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Persistence;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;
using TeamTimeWarp.Persistence.UserState;

namespace TeamTimeWarp.Service
{
    //public static class UsersCacheFactory
    //{
    //    public static ConcurrentCache<long,UserStateManager> Get(IEnumerable<Account> accounts , ITimeWarpStateCalculator timeWarpStateCalculator)
    //    {
    //        IList<TimeWarpUserState> userStates;

    //        //todo: optimize this crap.
    //        var userStateRepository = new TimeWarpUserStateRepository();
                
    //        userStates = new List<TimeWarpUserState>();
    //        foreach(var account in accounts)
    //        {
    //            var latestState = userStateRepository.GetLatestStateByAccountId(account.Id);
    //            if (latestState != null)
    //            {
    //                //NHibernateUtil.Initialize(latestState.Account);
    //                userStates.Add(userStateRepository.GetLatestStateByAccountId(account.Id));
    //            }
    //        }
    
            

    //        var users = userStates.Select(x => new UserStateManager(x, timeWarpStateCalculator));
    //        return new UserStateConcurrentCache(users);
    //    }
    //}
}