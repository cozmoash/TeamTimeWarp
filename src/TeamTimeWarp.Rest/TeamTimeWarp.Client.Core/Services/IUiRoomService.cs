using System.Collections.Generic;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services
{
    public interface IUiRoomService
    {
        ICollection<RoomInfo> GetAllRoomsForLoggedInUser();
        RoomInfo CreateRoom(string roomName);
        ICollection<UserStateInfoResponse> RoomStatus(int roomId);
    }
}