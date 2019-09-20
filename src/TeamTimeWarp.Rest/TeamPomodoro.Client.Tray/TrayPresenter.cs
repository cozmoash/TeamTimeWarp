using System;
using System.ComponentModel;
using System.Diagnostics;
using TeamTimeWarp.Client.Core;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Tray.Wpf.ViewModel;
using TeamTimeWarp.Public.Models.v001;
using TimeManager.Client.Tray;

namespace TeamTimeWarp.Client.Tray
{
    public class TrayPresenter : ITrayPresenter
    {
        private readonly IServiceContainer _serviceContainer;
        private readonly IUserStateListener _userStateListener;
        private readonly IIconFactory _iconFactory;
        private readonly ITooltipTextFactory _toolTipFactory;


        public event EventHandler<EventArgs> ShowPostLoginTrayInformation;
        public event EventHandler<ShowBalloonEventArgs> ShowBallon;
        public event EventHandler<TooltipTextChangedEventArgs> TooltipTextChanged;

        public TrayPresenter(ServiceContainer serviceContainer,UserStateListener userStateListener, IIconFactory trayIconFactory, ITooltipTextFactory tooltipTextFactory)
        {
            _iconFactory = trayIconFactory;
            _toolTipFactory = tooltipTextFactory;
            _serviceContainer = serviceContainer;
            _userStateListener = userStateListener;

            TeamViewModel = new MainWindowViewModel(_serviceContainer.AuthenticationService,
                                                    _serviceContainer.RoomService,
                                                    _serviceContainer.GlobalRoomService,
                                                    _serviceContainer.UserStateService,
                                                    _serviceContainer.UserMessageService);

            _serviceContainer.AuthenticationService.LoginCompleted += HandleLoginCompleted;

            _userStateListener.TimeChanged += UserStateListenerTimeChanged;
            _userStateListener.UserStateChanged += UserStateListenerUserStateChanged;
        }

        public void StartWork()
        {
            _serviceContainer.UserStateService.StartWorkAsync();
        }

        public void StopWork()
        {
            _serviceContainer.UserStateService.StopWorkAsync();
        }

        public void TrayIconClick()
        {
            if (_serviceContainer.AuthenticationService.IsLoggedIn &&
                _userStateListener.CurrentTimeWarpState == TimeWarpState.None)
                StartWork();
        }

        public void TrayIconBallonTipClicked()
        {
            if (_serviceContainer.AuthenticationService.IsLoggedIn &&
                _userStateListener.CurrentTimeWarpState == TimeWarpState.None)
                StartWork();
        }

        public MainWindowViewModel TeamViewModel { get; private set; }


        protected virtual void OnTooltipTextChanged(TooltipTextChangedEventArgs e)
        {
            EventHandler<TooltipTextChangedEventArgs> handler = TooltipTextChanged;
            if (handler != null) handler(this, e);
        }


        private void UserStateListenerUserStateChanged(object sender, UserMessageEventArgs e)
        {
            ShowBalloonEventArgs eventArgs = null;
            var icon = _iconFactory.Get(_userStateListener.CurrentTimeWarpState);

            switch (e.UpdatedState)
            {
                case (TimeWarpStateUserMessage.TakeABreak):
                    eventArgs = new ShowBalloonEventArgs(1000, "Time Warp", "Take a break!", icon, "pack://application:,,,/Wpf/Resources/moon_stroke_32x32.png");
                    break;
                case (TimeWarpStateUserMessage.TimeToWork):
                    eventArgs = new ShowBalloonEventArgs(1000000, "Time Warp", "time to start working", icon, "pack://application:,,,/Wpf/Resources/bolt_32x32.png");
                    break;
                case (TimeWarpStateUserMessage.Working):
                    {
                        string message;
                        if (e.TimeWarpAgent == TimeWarpAgent.Unknown || e.TimeWarpAgent == TimeWarpAgent.WindowsTrayClient)
                        {   
                            message = "Timer started..";
                        }
                        else if(e.TimeWarpAgent == TimeWarpAgent.VisualStudio)
                        {

                            message = "Triggered by Visual Studio";
                        }
                        else
                        {
                            message = "Triggered by " + e.TimeWarpAgent;
                        }

                        eventArgs = new ShowBalloonEventArgs(1000, "Time Warp - Work started", message, icon, "pack://application:,,,/Wpf/Resources/bolt_32x32.png");
                        break;
                    }
            }

            EventHandler<ShowBalloonEventArgs> handler = ShowBallon;
            if (handler != null) handler(this, eventArgs);

        }

        private void UserStateListenerTimeChanged(object sender, TimeChangedEventArgs e)
        {
            var text = _toolTipFactory.Get(_userStateListener.CurrentTimeWarpState,
                                           _userStateListener.MinutesRemaining);

            EventHandler<TooltipTextChangedEventArgs> handler = TooltipTextChanged;
            if (handler != null) handler(this, new TooltipTextChangedEventArgs(text));
        }

        private void HandleLoginCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.IsErrored())
            {
                _userStateListener.Start();
                EventHandler<EventArgs> handler = ShowPostLoginTrayInformation;
                if (handler != null) handler(this, EventArgs.Empty);
            }
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
                _serviceContainer.AuthenticationService.LoginCompleted -= HandleLoginCompleted;
                _userStateListener.TimeChanged -= UserStateListenerTimeChanged;
                _userStateListener.UserStateChanged -= UserStateListenerUserStateChanged;
                _userStateListener.Dispose();
                _serviceContainer.Dispose();
                TeamViewModel.Dispose();
            }
        }

        ~TrayPresenter()
        {
            Debug.Fail(string.Format("finalizer called on type ({0})", GetType()));
            Dispose(false);
        }

    }
}