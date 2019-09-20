using System;
using System.Collections.Generic;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Persistence;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;

namespace TeamTimeWarp.Service
{

    public abstract class Store : IDisposable
    {



        public void Dispose()
        {
            
        }
    }

    public interface IAccountStore : IDisposable
    {



    }

    public class AccountsStore : IAccountStore
    {
        private TimeWarpSessionFactory<AccountClassMap> _sessionFactory;

        public AccountsStore()
        {
            _sessionFactory = new TimeWarpSessionFactory<AccountClassMap>();
        }



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }



    public static class AccountsCacheFactory
    {
        //public static ConcurrentCache<long,Account> Get()
        //{
        //    IList<Account> accounts;
        //    using (var SessionFactory = )
        //    {
        //        var accountRespository = new AccountRepository(SessionFactory.GetCurrentSession());
        //        accounts = accountRespository.GetAll();
        //    }

        //    return new AccountsConcurrentCache(accounts);
        //}
    }
}