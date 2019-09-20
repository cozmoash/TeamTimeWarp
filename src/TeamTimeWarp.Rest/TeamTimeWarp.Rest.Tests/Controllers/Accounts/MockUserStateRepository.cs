using System.Linq;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;

namespace TeamTimeWarp.Rest.Tests.Controllers.Accounts
{
    public class MockUserStateRepository :MockRepository<TimeWarpUserState>, ITimeWarpUserStateRepository
    {
        
        public override void Add(TimeWarpUserState item)
        {
            Items.Add(item.Id,item);
        }

        public TimeWarpUserState GetLatestStateByAccountId(long accountId)
        {
            return Items.Values.Where(x => x.Account.Id == accountId).OrderBy(x => x.PeriodStartTime).FirstOrDefault();
        }
    }
}