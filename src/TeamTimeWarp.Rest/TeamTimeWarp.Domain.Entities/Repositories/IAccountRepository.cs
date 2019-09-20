using System.Collections.Generic;

namespace TeamTimeWarp.Domain.Entities.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        IList<Account> GetByEmail(string email); //should this be non collection?
        Account Get(long id);
    }
}