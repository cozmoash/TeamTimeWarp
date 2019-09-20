using System;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Core.Services.Interfaces;

namespace TeamTimeWarp.Client.Tray
{
    public interface IServiceContainer : IDisposable
    {
        IUiAuthenticationService AuthenticationService { get; }
        IUiRoomService RoomService { get; }
        IUserStateService UserStateService { get; }
        IUiGlobalRoomsService GlobalRoomService { get; }
        IUiUserMessageService UserMessageService { get; }
    }
}