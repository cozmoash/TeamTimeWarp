using System;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Threading;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.TeamTimeWarp_VsPackage
{



    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidTeamTimeWarp_VsPackagePkgString)]
    [ProvideAutoLoad("D2567162-F94F-4091-8798-A096E61B8B50")]
    public sealed class TeamTimeWarp_VsPackagePackage : Package
    {
        private DTE _dte;
        private readonly IUiAuthenticationService _authenticationService;
        private readonly TokenStore _tokenStore;
        private readonly IRestServiceUriFactory _restServiceUriFactory;
        private readonly UserStateService _userStateService;
        private readonly TokenPersister _tokenPersister;
        private VsWorkStarter _workStarter;

        public TeamTimeWarp_VsPackagePackage()
        {
            _restServiceUriFactory = new RestServiceUriFactory();

            _tokenStore = new TokenStore();
            _tokenPersister = new TokenPersister();
            _authenticationService = new AuthenticationService(_tokenStore,_restServiceUriFactory,_tokenPersister,new SynchronizationContext());
            _userStateService = new UserStateService(_tokenStore, _restServiceUriFactory,
                                                     new AgentTypeProvider(TimeWarpAgent.VisualStudio));
        }
        protected override void Initialize()
        {
            base.Initialize();

            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                AddButtonHandler((int)PkgCmdIDList.cmTeamTimeWarpPackageCommand, StartWorkCallback,StartWorkEnabledCallback, mcs,false);
                AddButtonHandler((int)PkgCmdIDList.cmTeamTimeWarpEndWorkCommand, EndWorkCallback, EndWorkEnabledCallback,mcs,false);
                AddButtonHandler((int)PkgCmdIDList.cmTeamTimeWarpEnableAutoTriggeringCommand, AutoTriggerToggleCallback,IsAutoTriggerEnabledCallback, mcs,false);
                AddButtonHandler((int)PkgCmdIDList.cmTeamTimeWarpSetupLoginDetailsCommand, LoginCallback, IsLoggedInCallback, mcs,true);
                AddButtonHandler((int)PkgCmdIDList.cmTeamTimeWarpSetupLogoutDetailsCommand, LogoutCallback, IsLogoutEnabledCallback, mcs, true);
            }

            _dte = (DTE)GetService(typeof(DTE));
            _workStarter = new VsWorkStarter(_userStateService, _authenticationService, new DteTrigger(_dte));
            

            LoginToken token;
            if (TryPopulatedPersistedToken(out token))
            {
                _tokenStore.Token = token;
            }

        }

        private bool TryPopulatedPersistedToken(out LoginToken loginToken)
        {
            try
            {
                if (_tokenPersister.TokenExists())
                {
                    loginToken = _tokenPersister.GetToken();
                    return true;
                }

            }
            catch (Exception)
            {
            }
            loginToken = null;
            return false;
        }

        private void EndWorkEnabledCallback(object sender, EventArgs e)
        {
            EnableCommandIfLoggedIn(sender);
        }

        private void StartWorkEnabledCallback(object sender, EventArgs e)
        {
            EnableCommandIfLoggedIn(sender);
        }

        private void IsAutoTriggerEnabledCallback(object sender, EventArgs e)
        {
            var isLoggedIn = _authenticationService.IsLoggedIn;
            var command = (OleMenuCommand)sender;
            command.Enabled = isLoggedIn;
            command.Text = _workStarter.IsEnabled ? "Disable Auto Start On Key Press" : "Enable Auto Start On Key Press";
        }

        private void IsLogoutEnabledCallback(object sender, EventArgs e)
        {
            EnableCommandIfLoggedIn(sender);
        }

        private void IsLoggedInCallback(object sender, EventArgs e)
        {
            var isLoggedIn = _authenticationService.IsLoggedIn;
            var command = (OleMenuCommand)sender;
            command.Enabled = !isLoggedIn;
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _workStarter.Dispose();
            }
        }

        private static void AddButtonHandler(int command, EventHandler eventHandler, EventHandler beforeQueryStatus, OleMenuCommandService mcs, bool isEnabled)
        {
            CommandID startWorkCommandId = new CommandID(GuidList.guidTeamTimeWarp_VsPackageCmdSet, command);
            OleMenuCommand startWorkMenuCommand = new OleMenuCommand(eventHandler, startWorkCommandId);
            startWorkMenuCommand.Enabled = isEnabled;
            if (beforeQueryStatus != null)
                startWorkMenuCommand.BeforeQueryStatus += beforeQueryStatus;
            mcs.AddCommand(startWorkMenuCommand);
            
        }
        
        private void LoginCallback(object sender, EventArgs e)
        {
            VsLoginViewModel vsLoginViewModel = new VsLoginViewModel(_authenticationService);
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.DataContext = vsLoginViewModel;
            loginWindow.Show();
        }

        private void LogoutCallback(object sender, EventArgs e)
        {
            try
            {
                _tokenPersister.RemoveToken();
            }
            catch (Exception)
            {
            }
            _tokenStore.Token = null;
        }

        private void AutoTriggerToggleCallback(object sender, EventArgs e)
        {
            _workStarter.IsEnabled = !_workStarter.IsEnabled;
        }

        private void EndWorkCallback(object sender, EventArgs e)
        {
            _userStateService.StopWorkAsync();
        }

        private void StartWorkCallback(object sender, EventArgs e)
        {
            _userStateService.StartWorkAsync();
        }

        private void EnableCommandIfLoggedIn(object sender)
        {
            var isLoggedIn = _authenticationService.IsLoggedIn;
            var command = (OleMenuCommand)sender;
            command.Enabled = isLoggedIn;
        }

        

    }
}
