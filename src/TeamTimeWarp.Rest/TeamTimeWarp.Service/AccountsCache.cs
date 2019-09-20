using System.Collections.Generic;
using System.Linq;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Service
{
    public class AccountsCache : Cache<long, Account>
    {
        public AccountsCache(IEnumerable<Account> accounts)
            : base(accounts.ToDictionary(x => x.Id, x => x))
        {}


    }
}