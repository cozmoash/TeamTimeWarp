using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Public.Models.v001;
using TimeManager.Client.Tray.Wpf;

namespace TeamTimeWarp.Client.Tray.Wpf.ViewModel
{
    public class RoomStateViewModel : ViewModelBase, IDisposable
    {
        private readonly IUiRoomService _roomService;
        private readonly IUiAuthenticationService _authenticationService;
        private readonly SynchronizationContext _context;

        private RoomInfo _roomInfo;
        private readonly object _locker = new object();
       
        private Timer _poller;
        private string _roomName;
        private string _errorMessage;

        public RoomStateViewModel(IUiRoomService roomService, IUiAuthenticationService authenticationService)
        {
            _roomService = roomService;
            _authenticationService = authenticationService;
            _poller = new Timer(PollUserStates);
            UserStates = new ObservableCollection<UserStateInfoViewModel>();
            _context = SynchronizationContext.Current;

            roomService.JoinRoomCompleted += HandleJoinRoomCompleted;
        }

        void HandleJoinRoomCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var newRoom = (RoomInfo) e.UserState;

            lock (_locker)
            {
                _roomInfo = newRoom;
                RoomName = newRoom.Name.ToUpper();
                UserStates.Clear();
                Start();
            }
        }

        public string RoomName
        {
            get { return _roomName; }
            private set
            {
                if (value == _roomName) return;
                _roomName = value;
                OnPropertyChanged("RoomName");
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            private set
            {
                if (value == _errorMessage) return;
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        public ObservableCollection<UserStateInfoViewModel> UserStates { get; private set; }

        private void Start()
        {
            lock (_locker)
                _poller.Change(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(-1));
        }
        
        public void Dispose()
        {
            lock (_locker)
            {
                if (_poller != null)
                {
                    _poller.Dispose();
                    _poller = null;
                }
            }
        }

        private void PollUserStates(object state)
        {
            lock (_locker)
            {
                if (_roomInfo == null)//no room selected.
                    return;
                ICollection<UserStateInfoResponse> roomStateResponseses = null;
                
                try
                {
                    roomStateResponseses = _roomService.RoomStatus(_roomInfo.Id);
                }
                catch (Exception e)
                {
                    _context.Post(_ => { ErrorMessage = e.Message; },null);
                }

                if (roomStateResponseses != null)
                {
                    _context.Post(_ => { ErrorMessage = null; }, null);
                    _context.Post(_ => UpdateStates(roomStateResponseses), null);
                }

                if(_poller != null)
                    _poller.Change(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(-1));
            }
        }

        private void UpdateStates(IEnumerable<UserStateInfoResponse> roomStateResponseses)
        {
            if (roomStateResponseses == null)
                return;

            foreach (var stateInfoResponse in roomStateResponseses)
            {
                UserStateInfoViewModel vm;
                if (!TryGetAccountState(stateInfoResponse.AccountId, out vm))
                {
                    var isMe = _authenticationService.AccountId == stateInfoResponse.AccountId;
                    vm = new UserStateInfoViewModel(stateInfoResponse.AccountId, stateInfoResponse.Username,isMe);
                    UserStates.Add(vm);
                }

                vm.Update(stateInfoResponse);
            }
            OnPropertyChanged("UserStates");
        }

        private bool TryGetAccountState(long accountId, out UserStateInfoViewModel vm)
        {
            vm = UserStates.SingleOrDefault(x=>x.AccountId == accountId);
            return vm != null;
        }

    }
}