using System.Collections.Generic;
using System.Linq;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Service
{
    public class UserStateConcurrentCache : ConcurrentCache<long,UserStateManager>
    {
        public UserStateConcurrentCache(IEnumerable<UserStateManager> users)
            : base(users.ToDictionary(x => x.Account.Id, x => x))
        {}
    }
}