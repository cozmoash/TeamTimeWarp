using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Persistence.Repositories
{
    public class TimeWarpAccountRepository : PersistenceRepositoryBase<Account>
    {
        public TimeWarpAccountRepository(ISession session) : base(session)
        {
        }

        public IList<Account> GetByEmail(string email)
        {
            return (from account in Session.Query<Account>()
                   where account.Email.Equals(email)
                   select account).ToList();
        }

        public Account Get(long id)
        {
            return Session.Query<Account>().SingleOrDefault(x => x.Id == id);
        }

    }
}