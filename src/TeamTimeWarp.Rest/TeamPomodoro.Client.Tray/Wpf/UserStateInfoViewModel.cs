using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray.Wpf
{
    public class UserStateInfoViewModel : ViewModelBase
    {
        private string _username;
        private double _progress;

        public UserStateInfoViewModel(long accountId, string username)
        {
            Username = username;
            AccountId = accountId;
        }

        public long AccountId { get; private set; }

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

        public double Progress
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

            Progress = userStateInfoResponse.Progress;
        }
    }
}