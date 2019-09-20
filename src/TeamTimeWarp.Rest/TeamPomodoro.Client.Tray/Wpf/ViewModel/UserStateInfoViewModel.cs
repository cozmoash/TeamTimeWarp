using System.Globalization;
using System.Windows;
using System.Windows.Input;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Client.Tray.Wpf.ViewModel;
using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray.Wpf
{
    public class UserStateInfoViewModel : ViewModelBase
    {
        private string _username;
        private int _progress;
        private TimeWarpState _currentState;
        private string _remainingTime;
        private string _sortString;
        private Visibility _notificationsEnabled;

        public UserStateInfoViewModel(long accountId, string username, bool isCurrentUser, IUiUserMessageService userMessageService)
        {
            Username = SanitizeUsername(username);
            SortString = isCurrentUser ? "!" : username;
            AccountId = accountId;

            SendMessageViewModel = new SendMessageViewModel(userMessageService,AccountId);

            SendMessageToUserCommand = new DelegateCommand(_ =>
                {
                    SendMessageViewModel.SendMessageVisible = Visibility.Visible;
                });
        }


        public ICommand SendMessageToUserCommand { get; private set; }

        public long AccountId { get; private set; }
        public SendMessageViewModel SendMessageViewModel { get; private set; }
        
        public Visibility NotificationsEnabled
        {
            get { return _notificationsEnabled; }
            set

            {
                if (Equals(value, _notificationsEnabled)) return;
                _notificationsEnabled = value;
                OnPropertyChanged("NotificationsEnabled");
            }
        }


        public TimeWarpState CurrentState
        {
            get { return _currentState; }
            private set
            {
                if (value == _currentState) return;
                _currentState = value;
                OnPropertyChanged("CurrentState");
            }
        }

        public string RemainingTime
        {
            get { return _remainingTime; }
            private set
            {
                if (value == _remainingTime) return;
                _remainingTime = value;
                OnPropertyChanged("RemainingTime");
            }
        }

        public string SortString
        {
            get { return _sortString; }
            private set
            {
                if (value == _sortString) return;
                _sortString = value;
                OnPropertyChanged("SortString");
            }
        }

        public string Username
        {
            get { return _username; }
            private set
            {
                if (value == _username) return;
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public void Update(UserStateInfoResponse userStateInfoResponse)
        {
            if (userStateInfoResponse.AccountId != AccountId)
                return;

            Progress = (int)((1-userStateInfoResponse.Progress) * 100);
            //Username = userStateInfoResponse.Username.Length;
            //if(!_isCurrentUser)
            //    SortString = Username;

            RemainingTime = userStateInfoResponse.TimeLeft.Minutes.ToString(CultureInfo.InvariantCulture);
            CurrentState = userStateInfoResponse.State;
            NotificationsEnabled = userStateInfoResponse.State == TimeWarpState.Working ? Visibility.Visible : Visibility.Collapsed;
        }

        public string SanitizeUsername(string userName)
        {
            if (userName.Length > 15)
                return userName.Substring(0, 14) + "..";
            return userName;
        }
    }
}