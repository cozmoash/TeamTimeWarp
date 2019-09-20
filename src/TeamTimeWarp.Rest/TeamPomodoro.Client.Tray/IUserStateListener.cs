using System;
using TeamTimeWarp.Public.Models.v001;
using TimeManager.Client.Tray;

namespace TeamTimeWarp.Client.Tray
{
    public interface IUserStateListener : IDisposable
    {
        event EventHandler<UserMessageEventArgs> UserStateChanged;
        event EventHandler<TimeChangedEventArgs> TimeChanged;

        int MinutesRemaining { get; }
        TimeWarpState CurrentTimeWarpState { get; }
        void Start();
    }
}