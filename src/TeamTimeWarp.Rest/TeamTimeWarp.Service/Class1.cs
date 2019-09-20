using System.Collections.Generic;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;

namespace TeamTimeWarp.Service
{
    public class Accounts
    {
        private readonly HashSet<Account> _accounts;

        public Accounts(IEnumerable<Account> accounts)
        {
            _accounts = new HashSet<Account>(accounts,
                                                     new FuncEqualityCompare<Account>((x, y) => x.Id == y.Id,
                                                                                              x => x.GetHashCode()));
        }


        public static Accounts Get(ITimeWarpAccountRepository accountRepository)
        {
            var accounts = accountRepository.GetAll();
            return new Accounts(accounts);
        }
    }
}
