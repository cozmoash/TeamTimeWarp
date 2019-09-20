using System;
using System.Collections.Generic;
using System.ComponentModel;
using RestSharp;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services
{
    public class RoomService : UiServiceBase, IUiRoomService
    {
        public RoomService(TokenStore tokenStore, IRestServiceUriFactory restServiceUriFactory) : base(tokenStore, restServiceUriFactory)
        {
        }

        public event EventHandler<AsyncCompletedEventArgs<ICollection<RoomInfo>>> GetMyRoomsCompleted;

        public void GetMyRoomsAsync()
        {
            ExecuteRequestAsync("roominfo", Method.GET, restResponse => AsyncCompletedEventArgsExtensions.Raise(GetMyRoomsCompleted, restResponse));
        }

        public event EventHandler<AsyncCompletedEventArgs> JoinRoomCompleted;

        public void JoinRoomAsync(RoomInfo room)
        {
            ExecuteRequestAsync(
                string.Format("roominfo/?roomid={0}&userRoomCommand={1}", room.Id, (int) UserRoomCommand.Join),
                Method.POST, restResponse => AsyncCompletedEventArgsExtensions.Raise(JoinRoomCompleted, restResponse, room));
        }

        public event EventHandler<AsyncCompletedEventArgs> LeaveRoomCompleted;

        public void LeaveRoomAsync(RoomInfo room)
        {
            ExecuteRequestAsync(
                string.Format("roominfo/?roomid={0}&userRoomCommand={1}", room.Id, (int)UserRoomCommand.Leave),
                Method.POST, restResponse => AsyncCompletedEventArgsExtensions.Raise(LeaveRoomCompleted, restResponse, room));
        }

        public event EventHandler<AsyncCompletedEventArgs<ICollection<UserStateInfoResponse>>> QueryRoomStatusCompleted;

        public void QueryRoomStatus(int roomId)
        {
            ExecuteRequestAsync(string.Format("roomstate/?roomid={0}", roomId), Method.GET,
                                restRespose =>
                                AsyncCompletedEventArgsExtensions.Raise(QueryRoomStatusCompleted, restRespose));
        }

    }
}