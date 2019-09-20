using System.Collections.Generic;
using System.Linq;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;

namespace TeamTimeWarp.Rest.Tests.Controllers.Accounts
{
    public class MockAccountsRepository : IAccountRepository
    {
        readonly IDictionary<long, Account> _accounts = new Dictionary<long, Account>(); 

        public IList<Account> GetAll()
        {
            return _accounts.Values.ToArray();
        }

        public void Add(Account item)
        {
            _accounts.Add(item.Id,item);
        }

        public IList<Account> GetByEmail(string email)
        {
            return _accounts.Values.Where(x => x.Email == email).ToList();
        }

        public Account Get(long id)
        {
            Account account;
            if (_accounts.TryGetValue(id, out account))
                return account;
            return null;
        }
    }
}