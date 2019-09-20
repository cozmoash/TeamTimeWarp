using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Mvc;
using TeamTimeWarp.Rest.Controllers;
using log4net;
using log4net.Appender;

namespace TeamTimeWarp.Rest
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        

        public void SetupDependencyInjection()
        {
            // Create Ninject DI kernel
            IKernel kernel = new StandardKernel();
         
          
            // Tell ASP.NET MVC 3 to use our Ninject DI Container
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure(
                new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            ILog log = LogManager.GetLogger(typeof(WebApiApplication));

            log.Info("Starting..");
            log.InfoFormat("Version = {0}", GetFileVersion());
            log.InfoFormat("Username = {0}", Environment.UserName);
            log.InfoFormat("Currect Dir = {0}", Environment.CurrentDirectory);
            log.InfoFormat("OS = {0}", Environment.OSVersion);
            log.InfoFormat("Processor Count = {0}", Environment.ProcessorCount);
            log.InfoFormat("Machine name = {0}", Environment.MachineName);
            //SetupDependencyInjection();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        private string GetFileVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }




        

    }
}