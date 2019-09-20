using System;
using System.ComponentModel;
using System.Windows.Input;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Core.Services.Interfaces;

namespace TeamTimeWarp.TeamTimeWarp_VsPackage
{
    public class VsLoginViewModel : INotifyPropertyChanged, IDataErrorInfo 
    {
        private readonly IUiAuthenticationService _uiAuthenticationService;
        private readonly ICommand _signInCommand;
        private string _loadingMessage;
        private string _emailAddress;
        private string _password;
        private bool _inputEnabled;
        private bool _rememberLogin;

        public VsLoginViewModel(IUiAuthenticationService uiAuthenticationService)
        {
            if(uiAuthenticationService == null)
                throw new ArgumentNullException("uiAuthenticationService");

            _inputEnabled = true;
            _loadingMessage = string.Empty;
            _uiAuthenticationService = uiAuthenticationService;
            _signInCommand = new DelegateCommand(OnSignIn);
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

        //public bool RememberLogin
        //{
        //    get { return _rememberLogin; }
        //    set { _rememberLogin = value; OnPropertyChanged("RememberLogin"); }
        //}

        public ICommand SignInCommand
        {
            get { return _signInCommand; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void HandleLoginCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.IsErrored())
            {
                LoadingMessage = "success!";
                _uiAuthenticationService.LoginCompleted -= HandleLoginCompleted;
            }
            else
            {
                LoadingMessage = e.Error.Message;
                InputEnabled = true;
            }
        }

        public bool InputEnabled
        {
            get { return _inputEnabled; }
            set
            {
                if (value.Equals(_inputEnabled)) return;
                _inputEnabled = value;
                OnPropertyChanged("InputEnabled");
            }
        }

        private void OnSignIn(object obj)
        {
            if (!string.IsNullOrWhiteSpace(EmailAddress))
            {
                InputEnabled = false;
                LoadingMessage = "Logging in...";
                _uiAuthenticationService.LoginCompleted += HandleLoginCompleted;
                _uiAuthenticationService.LoginAsync(EmailAddress, Password, true);
            }
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
                }
                return null;
            }
        }

        public string Error { get; private set; }
    }
}