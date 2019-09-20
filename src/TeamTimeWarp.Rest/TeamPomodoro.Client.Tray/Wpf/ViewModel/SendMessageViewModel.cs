using System.Windows;
using System.Windows.Input;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Client.Core.Services.Interfaces;

namespace TimeManager.Client.Tray.Wpf
{
    public class SendMessageViewModel : ViewModelBase
    {
        private readonly IUiUserMessageService _userMessageService;
        private readonly long _toAccount;
        private string _message;
        private bool _sendingInProgress;
        private Visibility _sendMessageVisible;

        public SendMessageViewModel(IUiUserMessageService userMessageService, long toAccount)
        {
            SendingInProgress = false;
            _sendMessageVisible = Visibility.Collapsed;
            _userMessageService = userMessageService;

            _toAccount = toAccount;

            Send = new DelegateCommand(OnSendMessage);
            Cancel = new DelegateCommand(OnCancelMessage);
        }

        public ICommand Send { get; private set; }
        public ICommand Cancel { get; private set; }

        public string Message
        {
            get { return _message; }
            set
            {
                if (value == _message) return;
                _message = value;
                OnPropertyChanged("Message");
            }
        }
        
        public bool SendingInProgress
        {
            get { return _sendingInProgress; }
            private set
            {
                if (value.Equals(_sendingInProgress)) return;
                _sendingInProgress = value;
                OnPropertyChanged("SendingInProgress");
            }
        }

        public Visibility SendMessageVisible
        {
            get { return _sendMessageVisible; }
            set
            {
                if (value == _sendMessageVisible) return;
                _sendMessageVisible = value;
                OnPropertyChanged("SendMessageVisible");
            }
        }

        private void HandleUserMessageServiceMessageSent()
        {
            SendingInProgress = false;
            CollapseSendMessageView();
        }

        private void OnCancelMessage(object state)
        {
            CollapseSendMessageView();
        }
        
        private void CollapseSendMessageView()
        {
            SendMessageVisible = Visibility.Collapsed;
        }
        
        private void OnSendMessage(object state)
        {
            if (string.IsNullOrEmpty(Message))
                return;

            SendingInProgress = true;
            _userMessageService.SendMessageAsync(_toAccount, Message, HandleUserMessageServiceMessageSent);
        }

    }
}