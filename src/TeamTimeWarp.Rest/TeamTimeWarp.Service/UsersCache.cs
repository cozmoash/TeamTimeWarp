using System.Collections.Generic;
using System.Linq;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Service
{
    public class UsersCache : Cache<long,TimeWarpUser>
    {
        public UsersCache(IEnumerable<TimeWarpUser> users)
            : base(users.ToDictionary(x => x.Account.Id, x => x))
        {}
    }
}