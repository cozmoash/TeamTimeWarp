using TeamTimeWarp.Persistence;
using TeamTimeWarp.Persistence.Accounts;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;
using TeamTimeWarp.Persistence.UserState;

namespace TeamTimeWarp.Service
{
    //public class NewAccountPersistence : IEntityPersistence<NewAccountInfo>
    //{
    //    public void Save(NewAccountInfo entity)
    //    {
    //        //this should probably be on a new thread...and exception handling. logging ..etc
    //        using (var session = new NHibernateTransaction<AccountClassMap>())
    //        {
    //            var accountRepository = new AccountRepository(session.GetCurrentSession());
    //            accountRepository.Add(entity.Account);

    //            var stateRepository = new TimeWarpUserStateRepository(session.GetCurrentSession());
    //            stateRepository.Add(entity.TimeWarpUserState);
    //        }
    //    }
    //}
}