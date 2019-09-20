using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Tray.Wpf.ViewModel;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.SL.RoomStateViewer
{
    public partial class App : Application
    {
        private readonly TokenStore _tokenStore;
        private readonly RestServiceUriFactory _restServiceUriFactory;
        private readonly AuthenticationService _authenticationService;
        private readonly RoomService _roomService;
        private UserMessageService _userMessageService;

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            _tokenStore = new TokenStore();
            _restServiceUriFactory = new RestServiceUriFactory();
            _authenticationService = new AuthenticationService(_tokenStore, _restServiceUriFactory,new NullTokenPersiter(),  SynchronizationContext.Current);
            _roomService = new RoomService(_tokenStore, _restServiceUriFactory);
            _userMessageService = new UserMessageService(_tokenStore, _restServiceUriFactory,
                                                         SynchronizationContext.Current);
            _authenticationService.LoginCompleted += _authenticationService_LoginCompleted;


            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainPage = new MainPage();

            
            _authenticationService.LoginAsync("dev","dev");

            RoomStateViewModel roomStateViewModel = new RoomStateViewModel(_roomService, _authenticationService, _userMessageService);

            mainPage.DataContext = roomStateViewModel;

            this.RootVisual = mainPage;
        }

        void _authenticationService_LoginCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            _roomService.JoinRoomAsync(new RoomInfo(102102, "SFS", default(DateTime), 1));
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
