using System;
using NHibernate;

namespace TeamTimeWarp.Persistence
{
    public class NHibernateTransaction<T> : IDisposable where T : FluentNHibernate.IMappingProvider
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
        public NHibernateTransaction()
        {
            _sessionFactory = NHibernateHelper.CreateSessionFactory<T>();
            _session = _sessionFactory.OpenSession();
            _session.BeginTransaction();
        }

        public ISession GetCurrentSession()
        {
            return _session;
        }

        public void Dispose()
        {
            if (_session == null) return;
            try
            {
                _session.Transaction.Commit();
            }
            catch (Exception)
            {
                _session.Transaction.Rollback();
            }
            finally
            {
                _session.Close();
                _session.Dispose();
            }
        }

        
        
    }
}