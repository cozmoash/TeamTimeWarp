using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TimeManager.Client.Tray.Wpf;

namespace TeamTimeWarp.Client.Tray.Wpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly RoomNavigationViewModel _roomNavigationViewModel;
        private readonly IUiAuthenticationService _authenticationService;
        private readonly IUiRoomService _roomService;

        private readonly RoomStateViewModel _roomStateViewModel;
        private readonly StartStopViewModel _startStopViewModel;


        private Visibility _loginVisibility;
        private Visibility _roomNavigationVisiblity;


        public MainWindowViewModel(IUiAuthenticationService authenticationService,
                                   IUiRoomService roomService,
                                   IUiGlobalRoomsService globalRoomsService,
                                   IUserStateService userStateService,IUiUserMessageService uiUserMessageService)
        {
            _authenticationService = authenticationService;
            _roomService = roomService;
            _loginViewModel = new LoginViewModel(authenticationService);
            _roomNavigationViewModel = new RoomNavigationViewModel(roomService, globalRoomsService);
            _roomStateViewModel = new RoomStateViewModel(_roomService, authenticationService, uiUserMessageService);
            _startStopViewModel = new StartStopViewModel(userStateService);
            
            ShowLogin();

            _authenticationService.LoginCompleted += HandleLoginCompleted;
        }

        public Visibility LoginVisibility
        {
            get { return _loginVisibility; }
            set
            {
                if (value == _loginVisibility) return;
                _loginVisibility = value;
                OnPropertyChanged("LoginVisibility");
            }
        }

        public Visibility RoomNavigationVisiblity
        {
            get { return _roomNavigationVisiblity; }
            set
            {
                if (value == _roomNavigationVisiblity) return;
                _roomNavigationVisiblity = value;
                OnPropertyChanged("RoomNavigationVisiblity");
            }
        }

        public LoginViewModel LoginViewModel
        {
            get { return _loginViewModel; }
        }

        public RoomNavigationViewModel RoomNavigationViewModel
        {
            get { return _roomNavigationViewModel; }
        }

        public RoomStateViewModel RoomStateViewModel
        {
            get { return _roomStateViewModel; }
        }

        public StartStopViewModel StartStopViewModel
        {
            get { return _startStopViewModel; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _authenticationService.LoginCompleted -= HandleLoginCompleted;
                if (_roomStateViewModel != null)
                    _roomStateViewModel.Dispose();
            }
        }

        ~MainWindowViewModel()
        {
            Debug.Fail(string.Format("finalizer called on type ({0})", GetType()));
            Dispose(false);
        }
        
        private void ShowRoomNavigation()
        {
            LoginVisibility = Visibility.Collapsed;
            RoomNavigationVisiblity = Visibility.Visible;
        }

        private void HandleLoginCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.IsErrored())
            {
                ShowRoomNavigation();
            }
        }


        private void ShowLogin()
        {
            LoginVisibility = Visibility.Visible;
            RoomNavigationVisiblity = Visibility.Collapsed;
        }
    }
}
