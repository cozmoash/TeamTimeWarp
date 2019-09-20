using System;
using System.Collections.Generic;
using System.Linq;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Public.Models.v001;
using TimeManager.Client.Tray.Wpf;

namespace TeamTimeWarp.Client.Tray.Wpf.ViewModel
{
    public class RoomNavigationViewModel : ViewModelBase, IDisposable
    {
        private readonly IUiRoomService _roomService;
        private readonly IUiGlobalRoomsService _globalRoomsService;
        private string _searchTerm;
        private IEnumerable<RoomInfoViewModel> _searchResults;
        private RoomInfoViewModel _selected;
        
        public RoomNavigationViewModel(IUiRoomService roomService, IUiGlobalRoomsService globalRoomsService)
        {
            _roomService = roomService;
            _globalRoomsService = globalRoomsService;
            _searchResults = new RoomInfoViewModel[0];   

            _globalRoomsService.SearchCompleted += HandleSearchCompleted;
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
                    _roomService.JoinRoomAsync(Selected.RoomInfo);
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
