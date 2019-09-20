using System.Linq;
using NHibernate.Linq;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;

namespace TeamTimeWarp.Persistence.Accounts
{
    public class AccountPasswordRepository : PersistenceRepositoryBase<AccountPassword, AccountPasswordClassMap>,  IAccountPasswordRepository
    {
        public AccountPasswordRepository() : base(new TimeWarpSessionFactory<AccountPasswordClassMap>())
        {
        }

        public AccountPassword GetByEmail(string email)
        {
            using (var session = SessionFactory.Get())
            {
                return (from accountPassword in session.Query<AccountPassword>()
                        where accountPassword.Account.Email.Equals(email)
                        select accountPassword).SingleOrDefault();
            }
        }
    }
}