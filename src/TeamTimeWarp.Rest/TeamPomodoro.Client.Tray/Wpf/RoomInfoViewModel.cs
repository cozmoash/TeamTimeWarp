using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray.Wpf
{
    public class RoomInfoViewModel : ViewModelBase
    {
        protected readonly RoomInfo RoomInfo;

        public RoomInfoViewModel(RoomInfo roomInfo)
        {
            RoomInfo = roomInfo;
            RoomName = roomInfo.Name;
            RoomId = roomInfo.Id;
            NumberOfUsers = roomInfo.NumberOfUsers;
        }

        public int RoomId { get; private set; }
        public string RoomName { get; private set; }
        public int NumberOfUsers { get; private set; }

        public override string ToString()
        {
            return RoomName;
        }

    }
}