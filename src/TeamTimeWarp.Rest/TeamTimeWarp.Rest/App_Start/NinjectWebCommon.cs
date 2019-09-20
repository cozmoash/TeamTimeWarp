using System.ComponentModel;
using System.Web.Http;
using Microsoft.FSharp.Core;
using TeamPomodoro.Domain;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Service;

[assembly: WebActivator.PreApplicationStartMethod(typeof(TeamTimeWarp.Rest.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(TeamTimeWarp.Rest.App_Start.NinjectWebCommon), "Stop")]

namespace TeamTimeWarp.Rest.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;


    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            


            RegisterServices(kernel);

            // Install our Ninject-based IDependencyResolver into the Web API config
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);


            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var timeWarpStateCalculator = GetTimeWarpStateCalculator();

            // Register services with Ninject DI Container
            kernel.Bind<ITimeWarpStateCalculator>().ToConstant(timeWarpStateCalculator);
            kernel.Bind<INowProvider>().To<NowProvider>();
            kernel.Bind<IUsers>().To<Users>();


        }

        private static TimeWarpStateCalculator GetTimeWarpStateCalculator()
        {
            Func<TimeWarpState, TimeSpan> cs_func = (i) =>
            {
                switch (i)
                {
                    case (TimeWarpState.None):
                        return TimeSpan.Zero;
                    case (TimeWarpState.Resting):
                        return TimeSpan.FromMinutes(5);
                    case (TimeWarpState.Working):
                        return TimeSpan.FromMinutes(25);
                    default:
                        throw new InvalidEnumArgumentException("i", (int)i, typeof(TimeWarpState));
                }
            };


            var fsharpFunc = FSharpFunc<TimeWarpState, TimeSpan>.FromConverter(
                new Converter<TimeWarpState, TimeSpan>(cs_func));
            var timeWarpStateCalculator = new TimeWarpStateCalculator(fsharpFunc);
            return timeWarpStateCalculator;
        }
    }
}
