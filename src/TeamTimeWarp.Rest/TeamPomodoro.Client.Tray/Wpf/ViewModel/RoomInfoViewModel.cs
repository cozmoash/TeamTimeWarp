using TeamTimeWarp.Public.Models.v001;
using TimeManager.Client.Tray.Wpf;

namespace TeamTimeWarp.Client.Tray.Wpf.ViewModel
{
    public class RoomInfoViewModel : ViewModelBase
    {

        public RoomInfoViewModel(RoomInfo roomInfo)
        {
            RoomName = roomInfo.Name;
            RoomInfo = roomInfo;
            NumberOfUsers = roomInfo.NumberOfUsers;
        }

        public RoomInfo RoomInfo { get; private set; }
        public string RoomName { get; private set; }
        public int NumberOfUsers { get; private set; }

        public override string ToString()
        {
            return RoomName;
        }

    }
}