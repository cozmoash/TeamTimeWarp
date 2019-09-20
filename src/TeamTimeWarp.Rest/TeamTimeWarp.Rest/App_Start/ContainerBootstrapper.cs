using System.ComponentModel;
using System.Web.Http;
using Microsoft.FSharp.Core;
using TeamPomodoro.Domain;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Persistence.Accounts;
using TeamTimeWarp.Persistence.Rooms;
using TeamTimeWarp.Persistence.UserState;
using TeamTimeWarp.Rest.Authentication;
using TeamTimeWarp.Rest.Controllers;
using TeamTimeWarp.Rest.RandomDataGenerator;

[assembly: WebActivator.PreApplicationStartMethod(typeof(TeamTimeWarp.Rest.App_Start.ContainerBootstrapper), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(TeamTimeWarp.Rest.App_Start.ContainerBootstrapper), "Stop")]

namespace TeamTimeWarp.Rest.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;


    public static class ContainerBootstrapper 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
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
            var accountPasswordRepository = new AccountPasswordRepository();
            var fakeAccountUserStateGenerator = new FakeAccountUserStateGenerator();
            fakeAccountUserStateGenerator.Start();

            kernel.Bind<IRoomRemovalPolicy>().To<RoomRemovalPolicy>();
            kernel.Bind<ITimeWarpStateCalculator>().ToConstant(timeWarpStateCalculator);
            kernel.Bind<IUserStateManager>().To<UserStateManager>();
            kernel.Bind<INowProvider>().To<NowProvider>();
            kernel.Bind<ITimeWarpUserStateRepository>().ToConstant(new TimeWarpUserStateRepository());
            kernel.Bind<IRoomRepository>().ToConstant(new RoomRepository());
            kernel.Bind<IAccountRepository>().ToConstant(new AccountRepository());
            kernel.Bind<IAccountPasswordRepository>().ToConstant(accountPasswordRepository);
            kernel.Bind<IAuthenticationSessionRepository>().ToConstant(new AuthenticationSessionRepository());
            kernel.Bind<ITimeWarpAuthenticationManager>().To<TimeWarpAuthenticationManager>();
            kernel.Bind<IAccountCreator>().To<AccountCreator>();
            kernel.Bind<IAccountCreationInformationValidator>().To<AccountCreationInformationValidator>();
            kernel.Bind<IUserMessageRepository>().ToConstant(new UserMessageRepository());
            kernel.Bind<FakeAccountUserStateGenerator>().ToConstant(fakeAccountUserStateGenerator);
        }


        private static TimeWarpStateCalculator GetTimeWarpStateCalculator()
        {
            Func<TimeWarpState, TimeSpan> csFunc = i =>
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
                new Converter<TimeWarpState, TimeSpan>(csFunc));
            var timeWarpStateCalculator = new TimeWarpStateCalculator(fsharpFunc);
            return timeWarpStateCalculator;
        }
    }
}
