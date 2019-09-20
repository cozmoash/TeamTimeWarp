using System;
using System.Windows;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Core.Services.Interfaces;

namespace TimeManager.Client.Tray.Wpf
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly RoomNavigationViewModel _roomNavigationViewModel;
        private Visibility _loginVisibility;
        private Visibility _roomNavigationVisiblity;
        private readonly IUiAuthenticationService _authenticationService;

        public MainWindowViewModel(IUiAuthenticationService authenticationService,
                                   IUserStateListener userStateListener,
                                   IUiRoomService roomService,
                                   IUiGlobalRoomsService globalRoomsService)
        {
            _authenticationService = authenticationService;
            _loginViewModel = new LoginViewModel(authenticationService, userStateListener);
            _roomNavigationViewModel = new RoomNavigationViewModel(roomService,globalRoomsService);

            LoginVisibility = Visibility.Visible;
            RoomNavigationVisiblity = Visibility.Collapsed;

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

        private void HandleLoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                LoginVisibility = Visibility.Collapsed;
                RoomNavigationVisiblity = Visibility.Visible;
            }
        }

        public void Dispose()
        {
            _authenticationService.LoginCompleted -= HandleLoginCompleted;
        }
    }
}
