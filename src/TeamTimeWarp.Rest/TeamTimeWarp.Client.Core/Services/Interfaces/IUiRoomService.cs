using System;
using System.Collections.Generic;
using System.ComponentModel;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services.Interfaces
{
    public interface IUiRoomService
    {
        //get my rooms
        event EventHandler<AsyncCompletedEventArgs<ICollection<RoomInfo>>> GetMyRoomsCompleted;
        void GetMyRoomsAsync();
        //ICollection<RoomInfo> GetMyRooms();

        //join room
        event EventHandler<AsyncCompletedEventArgs> JoinRoomCompleted;
        void JoinRoomAsync(RoomInfo room);
        //void JoinRoom(int roomId);

        //leave room
        event EventHandler<AsyncCompletedEventArgs> LeaveRoomCompleted;
        void LeaveRoomAsync(RoomInfo room);


        //RoomInfo CreateRoom(string roomName);
        event EventHandler<AsyncCompletedEventArgs<ICollection<UserStateInfoResponse>>> QueryRoomStatusCompleted;
        void QueryRoomStatus(int roomId);
       
    }    
}