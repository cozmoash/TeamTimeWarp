using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;

namespace TeamTimeWarp.Persistence.UserState
{
    public class TimeWarpUserStateRepository : PersistenceRepositoryBase<TimeWarpUserState, TimeWarpUserStateClassMap>, ITimeWarpUserStateRepository 
    {
        public TimeWarpUserStateRepository() : base(new TimeWarpSessionFactory<TimeWarpUserStateClassMap>())
        {
        }

        public TimeWarpUserState GetLatestStateByAccountId(long accountId)
        {
            using (var session = SessionFactory.Get())
            {
                var result = 
                    session.Query<TimeWarpUserState>().OrderByDescending(x => x.PeriodStartTime).FirstOrDefault(
                        x => x.Account.Id == accountId);

                if(result != null)
                    NHibernateUtil.Initialize(result.Account);

                return result;
            }
        }
    }
}