using System;
using System.Configuration;
using DbUp;
using DbUp.ScriptProviders;
using ILog = log4net.ILog;

namespace TeamTimeWarp.Database
{
    public class DbScripts
    {
        
    }

    class Program
    {
        private static ILog _logger;

        public static void Main(string[] args)
        {
            _logger = log4net.LogManager.GetLogger(typeof (Program));

            var connectionString = ConfigurationManager.AppSettings["FluentNHibernateConnection"];

            var upgrader = new DatabaseUpgrader(
                //"server=(local);Initial Catalog=TeamTimeWarp;Integrated Security=True",
                connectionString,
                new EmbeddedScriptProvider(typeof(Program).Assembly)
            );

            var result = upgrader.PerformUpgrade();

            foreach (var script in result.Scripts)
                _logger.InfoFormat("executed script ({0})", script.Name);
                

            if (result.Successful)
            {
                _logger.Info("Success");
                Environment.Exit(0);
            }
            else
            {
                _logger.Error("Failed",result.Error);
                Environment.Exit(1);
            }
        }


    }
}
