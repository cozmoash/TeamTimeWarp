using System.Linq;
using NHibernate;
using NHibernate.Linq;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;

namespace TeamTimeWarp.Persistence.Repositories
{
    public class TimeWarpUserStateRepository : PersistenceRepositoryBase<TimeWarpUserState>, ITimeWarpUserStateRepository 
    {
        public TimeWarpUserStateRepository(ISession session) : base(session)
        {
        }

        public TimeWarpUserState GetLatestStateByAccountId(long accountId)
        {
            return Session.Query<TimeWarpUserState>().OrderByDescending(x=>x.PeriodStartTime).FirstOrDefault(x => x.Account.Id == accountId);
        }
    }
}