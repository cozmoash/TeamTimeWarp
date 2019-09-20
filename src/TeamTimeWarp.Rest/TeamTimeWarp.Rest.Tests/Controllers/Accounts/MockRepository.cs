using System.Collections.Generic;
using System.Linq;
using TeamTimeWarp.Domain.Entities.Repositories;

namespace TeamTimeWarp.Rest.Tests.Controllers.Accounts
{
    public abstract class MockRepository<T> : IRepository<T>
    {
        protected IDictionary<long, T> Items = new Dictionary<long, T>();

        public IList<T> GetAll()
        {
            return Items.Values.ToArray();
        }

        public abstract void Add(T item);
    }

}