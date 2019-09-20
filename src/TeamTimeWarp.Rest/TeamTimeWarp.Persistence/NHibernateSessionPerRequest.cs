using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using TeamTimeWarp.Persistence.ClassMaps;

namespace TeamTimeWarp.Persistence
{
    public class NHibernateSessionPerRequest<T> : IDisposable where T : FluentNHibernate.IMappingProvider
    {
        const string ConnString = "FluentNHibernateConnection";

        private readonly ISessionFactory _sessionFactory;
        private readonly ISession _session;
        public NHibernateSessionPerRequest()
        {
            _sessionFactory = CreateSessionFactory();
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

        
        private static ISessionFactory CreateSessionFactory()
        {
            try
            {
                return Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008
                        .ConnectionString(c => c
                        .FromAppSetting(ConnString))
                        .ShowSql())
                    .ExposeConfiguration(cfg => cfg.Properties.Add("current_session_context_class", "web"))
                    .Mappings(m => m
                        .FluentMappings.AddFromAssemblyOf<T>()
                    //.Conventions.AddFromAssemblyOf<EnumConvention>()
                    )
                    .BuildSessionFactory();
            }
            catch (Exception ex)
            {
                //todo: log.
                throw;
            }
        }
    }
}