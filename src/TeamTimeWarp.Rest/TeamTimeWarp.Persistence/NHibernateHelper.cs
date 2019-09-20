using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using log4net;

namespace TeamTimeWarp.Persistence
{
    public static class NHibernateHelper
    {
        const string ConnString = "FluentNHibernateConnection";
        private static readonly ILog Log = LogManager.GetLogger(typeof(NHibernateHelper));

        public static ISessionFactory CreateSessionFactory<T>()
        {
            try
            {
                return Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2005.ConnectionString(c => c.FromAppSetting(ConnString))
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
                Log.Error("unable to connect to DB",ex);
                throw;
            }
        }

    }
}