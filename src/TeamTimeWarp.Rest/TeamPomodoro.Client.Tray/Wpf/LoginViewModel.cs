using System.ComponentModel;
using System.Windows.Input;
using TeamTimeWarp.Client.Core.Services;
using TimeManager.Client.Tray.Annotations;

namespace TimeManager.Client.Tray.Wpf
{
    public class LoginViewModel : INotifyPropertyChanged, IDataErrorInfo 
    {
        private readonly IUiAuthenticationService _uiAuthenticationService;
        private readonly IUserStateListener _userStateListener;
        private readonly ICommand _signInCommand;
        private string _loadingMessage;
        private string _emailAddress;
        private string _password;

        public LoginViewModel(IUiAuthenticationService uiAuthenticationService, IUserStateListener userStateListener)
        {
            _loadingMessage = string.Empty;
            _uiAuthenticationService = uiAuthenticationService;
            _userStateListener = userStateListener;
            _signInCommand = new DelegateCommand(OnSignIn);
            _emailAddress = "dev";
            _password = "dev";
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                if (value == _emailAddress) return;
                _emailAddress = value;
                OnPropertyChanged("EmailAddress");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string LoadingMessage
        {
            get { return _loadingMessage; }
            set
            {
                _loadingMessage = value;
                OnPropertyChanged("LoadingMessage");
            }
        }
        
        public ICommand SignInCommand
        {
            get { return _signInCommand; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void HandleLoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            if (e.Error != null)
                Error = LoadingMessage = e.Error.Message;
            else
            {
                LoadingMessage = "success!";
                _userStateListener.Start();
            }
        }

        private void OnSignIn(object obj)
        {
            LoadingMessage = "loading";
            _uiAuthenticationService.LoginCompleted += HandleLoginCompleted;
            _uiAuthenticationService.LoginAsync(EmailAddress, Password);
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case ("EmailAddress"):
                        {
                            if (string.IsNullOrEmpty(EmailAddress))
                                return "Please enter an email address";
                            break;
                        }
                    case ("Password"):
                        {
                            if (string.IsNullOrEmpty(Password))
                                return "Please enter a password";
                            break;
                        }
                }
                return null;
            }
        }

        public string Error { get; private set; }
    }
}