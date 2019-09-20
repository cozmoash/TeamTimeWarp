using System.ComponentModel;
using System.Windows.Input;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TimeManager.Client.Tray.Annotations;

namespace TeamTimeWarp.Client.Tray.Wpf.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged//, IDataErrorInfo 
    {
        private readonly IUiAuthenticationService _uiAuthenticationService;
        private readonly ICommand _signInCommand;
        private string _loadingMessage;
        private string _emailAddress;
        private string _password;
        private bool _inputEnabled;
        private string _quickLoginUsername;

        public LoginViewModel(IUiAuthenticationService uiAuthenticationService)
        {
            _inputEnabled = true;
            _loadingMessage = string.Empty;
            _uiAuthenticationService = uiAuthenticationService;
            _signInCommand = new DelegateCommand(OnSignIn);
            //_quickLoginUsername = "Anonymous time warper";
            //_emailAddress = "dev";
            //_password = "dev";
        }


        public string DefaultQuickLoginText
        {
            get { return "Trial username"; }
        }

        public string DefaultEmailAddressText
        {
            get { return "Email Address"; }
        }

        public string DefaultPasswordText
        {
            get { return "Password"; }
        }

        public string QuickLoginUsername
        {
            get { return _quickLoginUsername; }
            set
            {
                if (value == _quickLoginUsername) return;
                _quickLoginUsername = value;
                OnPropertyChanged("QuickLoginUsername");
            }
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

        private void HandleLoginCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.IsErrored())
            {
                LoadingMessage = "success!";
                //_userStateListener.Start();
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

            //InputEnabled = false;
            //LoadingMessage = "loading";
            //_uiAuthenticationService.LoginCompleted += HandleLoginCompleted;
            //_uiAuthenticationService.LoginAsync("dev","dev");

            if (!string.IsNullOrWhiteSpace(EmailAddress))
            {
                InputEnabled = false;
                LoadingMessage = "Logging in...";
                _uiAuthenticationService.LoginCompleted += HandleLoginCompleted;
                _uiAuthenticationService.LoginAsync(EmailAddress, Password);
            }else
            if (!string.IsNullOrWhiteSpace(QuickLoginUsername))
            {
                InputEnabled = false;
                LoadingMessage = "Logging in...";
                _uiAuthenticationService.LoginCompleted += HandleLoginCompleted;
                _uiAuthenticationService.QuickLoginAsync(QuickLoginUsername);
            }
            
        }

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        switch (columnName)
        //        {
        //            case ("EmailAddress"):
        //                {
        //                    if (string.IsNullOrEmpty(EmailAddress))
        //                        return "Please enter an email address";
        //                    break;
        //                }
        //            case ("Password"):
        //                {
        //                    if (string.IsNullOrEmpty(Password))
        //                        return "Please enter a password";
        //                    break;
        //                }
        //        }
        //        return null;
        //    }
        //}

        //public string Error { get; private set; }
    }
}