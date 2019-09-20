using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray.Wpf
{
    public class RoomNavigationViewModel : ViewModelBase, IDisposable
    {
        private readonly IUiRoomService _roomService;
        private readonly IUiGlobalRoomsService _globalRoomsService;
        private string _searchTerm;
        private IEnumerable<RoomInfoViewModel> _searchResults;
        private RoomInfoViewModel _selected;
        private bool _joinButtonEnabled;
        private string _message;

        public RoomNavigationViewModel(IUiRoomService roomService, IUiGlobalRoomsService globalRoomsService)
        {
            _roomService = roomService;
            _globalRoomsService = globalRoomsService;
            _searchResults = new RoomInfoViewModel[0];

            JoinRoomCommand = new DelegateCommand(_ =>
                {
                    if (Selected == null) return;
                    Message = "joining room..";
                    _roomService.JoinRoomAsync(Selected.RoomId);
                });

            _roomService.JoinRoomCompleted += HandleJoinRoomCompleted;
            _globalRoomsService.SearchCompleted += HandleSearchCompleted;
        }

        private void HandleJoinRoomCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Message = "Room Joined!";
        }

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

        public ICommand JoinRoomCommand { get; private set; }

        public bool JoinButtonEnabled
        {
            get { return _joinButtonEnabled; }
            private set
            {
                if (value.Equals(_joinButtonEnabled)) return;
                _joinButtonEnabled = value;
                OnPropertyChanged("JoinButtonEnabled");
            }
        }

        public RoomInfoViewModel Selected
        {
            get { return _selected; }
            set
            {
                if (Equals(value, _selected))
                    return;

                if (value != null)
                {
                    _selected = value;
                    JoinButtonEnabled = true;
                }
                else
                {
                    JoinButtonEnabled = false;
                }
            }
        }
        
        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                if (value == _searchTerm) return;

                _searchTerm = value;
                OnPropertyChanged("SearchTerm");
                
                if (!string.IsNullOrEmpty(_searchTerm))
                {
                    _globalRoomsService.SearchAsync(_searchTerm);
                }
            }
        }

        public IEnumerable<RoomInfoViewModel> SearchResults
        {
            get { return _searchResults; }
            private set
            {
                if (Equals(value, _searchResults)) return;
                _searchResults = value;
                OnPropertyChanged("SearchResults");
            }
        }

        private void HandleSearchCompleted(object sender, AsyncCompletedEventArgs<ICollection<RoomInfo>> e)
        {
            IEnumerable<RoomInfoViewModel> result = e.Error != null
                                                        ? Enumerable.Empty<RoomInfoViewModel>()
                                                        : e.Result.Select(info => new RoomInfoViewModel(info));
            SearchResults = result.ToArray();
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
                _globalRoomsService.SearchCompleted -= HandleSearchCompleted;
            }
        }

    }
}
