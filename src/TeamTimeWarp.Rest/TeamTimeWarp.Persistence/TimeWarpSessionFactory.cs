using System;
using NHibernate;

namespace TeamTimeWarp.Persistence
{
    public class TimeWarpSessionFactory<T> : IDisposable where T :  FluentNHibernate.IMappingProvider
    {
        private readonly ISessionFactory _sessionFactory;

        public TimeWarpSessionFactory()
        {
            _sessionFactory = NHibernateHelper.CreateSessionFactory<T>();
        }

        public ISession Get()
        {
            return _sessionFactory.OpenSession();
        }

        public void Dispose()
        {
            _sessionFactory.Dispose();
        }
    }
}