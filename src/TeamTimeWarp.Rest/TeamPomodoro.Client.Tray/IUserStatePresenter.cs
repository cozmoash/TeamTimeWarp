using System;
using TeamTimeWarp.Public.Models.v001;

namespace TimeManager.Client.Tray
{
    public interface IUserStatePresenter
    {
        event EventHandler<UserMessageEventArgs> UserStateChanged;
        event EventHandler<TimeChangedEventArgs> TimeChanged;

        int MinutesRemaining { get; }
        TimeWarpState CurrentTimeWarpState { get; }
    }
}