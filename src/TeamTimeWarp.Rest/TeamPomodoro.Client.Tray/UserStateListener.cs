using System;
using System.Diagnostics;
using System.Threading;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Public.Models.v001;
using TimeManager.Client.Tray;

namespace TeamTimeWarp.Client.Tray
{
    //todo: move to core
    public class UserStateListener : IUserStateListener
    {
        private Timer _timer;
        private readonly object _disposeTimer = new object();

        private readonly IUserStateService _userStateService;
        private readonly SynchronizationContext _context;

        public event EventHandler<UserMessageEventArgs> UserStateChanged;
        public event EventHandler<TimeChangedEventArgs> TimeChanged;

        private static readonly TimeSpan RefreshPeriod = TimeSpan.FromSeconds(1);

        public UserStateListener(IUserStateService userStateService, SynchronizationContext context)
        {
            _userStateService = userStateService;
            _context = context;
            CurrentTimeWarpState = TimeWarpState.None;
            _timer = new Timer(OnTimerCallback);
        }

        public int MinutesRemaining { get; private set; }
        public TimeWarpState CurrentTimeWarpState { get; private set; }

        public void Start()
        {
            lock(_disposeTimer)
                _timer.Change(RefreshPeriod, TimeSpan.FromMilliseconds(-1));
        }

        private void OnTimerCallback(object state)
        {
            var info = _userStateService.GetUserState();//in try catch?
            
            if (info != null)
            {
                var previousState = CurrentTimeWarpState;

                CurrentTimeWarpState = info.State;
                MinutesRemaining = info.TimeLeft.Minutes;

                if (info.State == TimeWarpState.Resting && previousState == TimeWarpState.Working)
                    OnUserStateChanged(UserMessage.TakeABreak,info.TimeWarpAgent);

                if (info.State == TimeWarpState.None && previousState == TimeWarpState.Resting)
                    OnUserStateChanged(UserMessage.TimeToWork, info.TimeWarpAgent);

                if (info.State == TimeWarpState.Working && previousState != TimeWarpState.Working)
                    OnUserStateChanged(UserMessage.Working, info.TimeWarpAgent);

                OnTimeChanged(MinutesRemaining);
            }

            lock (_disposeTimer)
            {
                if(_timer != null)
                    _timer.Change(RefreshPeriod, TimeSpan.Zero); //todo fix dispose
            }
        }

        private void OnTimeChanged(int minutesRemaining)
        {
            SendOrPostCallback callback = delegate
                {
                    EventHandler<TimeChangedEventArgs> handler = TimeChanged;
                    if (handler != null) handler(this, new TimeChangedEventArgs(minutesRemaining));
                };

            RunCallback(callback);
        }
        
        private void OnUserStateChanged(UserMessage updatedState, TimeWarpAgent timeWarpAgent)
        {
            SendOrPostCallback callback = delegate
                {
                    EventHandler<UserMessageEventArgs> handler = UserStateChanged;
                    if (handler != null) handler(this, new UserMessageEventArgs(updatedState, timeWarpAgent));
                };

            RunCallback(callback);
        }

        private void RunCallback(SendOrPostCallback callback)
        {
            if (_context != null)
                _context.Post(callback, null);
            else
                callback(null);
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
                lock (_disposeTimer)
                {
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }

        ~UserStateListener()
        {
            Debug.Fail(string.Format("finalizer called on type ({0})",GetType()));
            Dispose(false);
        }


    }
}