using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;

namespace TeamTimeWarp.Persistence.Accounts
{


    public class UserMessageRepository : PersistenceRepositoryBase<UserMessage, UserMessageClassMap>, IUserMessageRepository
    {
        public UserMessageRepository() : base(new TimeWarpSessionFactory<UserMessageClassMap>())
        {
        }

        public IList<UserMessage> GetAllPendingMessagesForAccount(long toAccountId)
        {
            using (var session = SessionFactory.Get())
            {
                UserMessage[] results =
                    (session.Query<UserMessage>()
                        .Where(message => message.ToAccount.Id == toAccountId)
                        .Where(message => !message.HasBeenReceived)
                        .ToArray());

                foreach(var result in results)
                {
                    NHibernateUtil.Initialize(result.FromAccount);
                }

                return results;
            }
        }
    }


    public class AccountRepository : PersistenceRepositoryBase<Account, AccountClassMap>, IAccountRepository
    {
        public AccountRepository()
            : base(new TimeWarpSessionFactory<AccountClassMap>())
        {
        }

        public IList<Account> GetByEmail(string email)
        {
            using (var session = SessionFactory.Get())
            {
                return (from account in session.Query<Account>()
                        where account.Email.Equals(email)
                        select account).ToList();
            }
        }

        public Account Get(long id)
        {
            using (var session = SessionFactory.Get())
            {
                return session.Query<Account>().SingleOrDefault(x => x.Id == id);
            }
        }

    }
}