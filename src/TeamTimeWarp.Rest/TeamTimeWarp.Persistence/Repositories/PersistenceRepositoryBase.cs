using System.Collections.Generic;
using System.Linq;
using FluentNHibernate;
using NHibernate.Linq;
using TeamTimeWarp.Domain.Entities.Repositories;

namespace TeamTimeWarp.Persistence.Repositories
{
    public abstract class PersistenceRepositoryBase<T, TClassMap> : IRepository<T> where TClassMap : IMappingProvider where T : class
    {
        protected readonly TimeWarpSessionFactory<TClassMap> SessionFactory;

        protected PersistenceRepositoryBase(TimeWarpSessionFactory<TClassMap> sessionFactory)
        {
            SessionFactory = sessionFactory;
        }
        public IList<T> GetAll()
        {
            using (var session = SessionFactory.Get())
            {
                return (from account in session.Query<T>()
                        select account).ToList();
            }
        }

        public void Merge(T item) 
        {
            using (var session = SessionFactory.Get())
            {
                session.Merge(item);
                session.Flush();
            }
        }

        public void Add(T item)
        {
            using (var session = SessionFactory.Get())
            {
                session.SaveOrUpdate(item);
                session.Flush();
            }
        }
    }
}