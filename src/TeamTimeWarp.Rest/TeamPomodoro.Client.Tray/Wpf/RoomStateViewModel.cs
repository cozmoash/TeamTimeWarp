using System;
using System.Collections.Generic;
using System.Threading;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Public.Models.v001;
using TimeManager.Client.Tray.Controls;

namespace TimeManager.Client.Tray.Wpf
{
    public class RoomStateViewModel : ViewModelBase, IDisposable
    {
        private readonly int _roomId;
        private readonly IUiRoomService _roomService;
        private readonly Timer _poller;
        private readonly SynchronizationContext _context;

        public RoomStateViewModel(int roomId, IUiRoomService roomService)
        {
            _roomId = roomId;
            _roomService = roomService;
            _poller = new Timer(PollUserStates);
            UserStates = new ObservableDictionary<long,UserStateInfoViewModel>();
            _context = SynchronizationContext.Current;
        }

        public ObservableDictionary<long, UserStateInfoViewModel> UserStates { get; private set; }
        
        public void Start()
        {
            _poller.Change(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(-1));
        }
        
        public void Dispose()
        {
            _poller.Dispose();
        }
        
        private void PollUserStates(object state)
        {
            ICollection<UserStateInfoResponse> roomStateResponseses = _roomService.RoomStatus(_roomId);

            _context.Post(_ => UpdateStates(roomStateResponseses), null);

            _poller.Change(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(-1));
        }

        private void UpdateStates(IEnumerable<UserStateInfoResponse> roomStateResponseses)
        {
            foreach (var stateInfoResponse in roomStateResponseses)
            {
                UserStateInfoViewModel vm;
                if (!UserStates.TryGetValue(stateInfoResponse.AccountId, out vm))
                {
                    vm = new UserStateInfoViewModel(stateInfoResponse.AccountId, stateInfoResponse.Username);
                }

                vm.Update(stateInfoResponse);
            }
        }
    }
}