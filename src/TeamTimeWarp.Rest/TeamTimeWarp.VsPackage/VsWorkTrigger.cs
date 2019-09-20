using System;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.TeamTimeWarp_VsPackage
{
    //todo:unit test.
    public interface IVsWorkStarter : IDisposable
    {
        void StartWorkAsync();
    }

    public class VsWorkStarter : IVsWorkStarter
    {
        private readonly IUiAuthenticationService _authenticationService;
        private readonly IDteTrigger _dtetrigger;
        private readonly IUserStateService _userStateService;

        private DateTime _lastWorkStartTime;
        private bool _disposeCalled;

        private volatile /*is this actually called on another thread?*/ bool _isEnabled = true;

        public VsWorkStarter(IUserStateService userStateService, IUiAuthenticationService authenticationService, IDteTrigger dtetrigger)
        {
            _authenticationService = authenticationService;
            _dtetrigger = dtetrigger;
            _userStateService = userStateService;

            _dtetrigger = dtetrigger;
            _dtetrigger.OnDteTrigger += OnDteTrigger;
            _userStateService.QueryUserStatusCompleted += HandleQueryUserStatusCompleted;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }


        public void StartWorkAsync()
        {
            if (!IsEnabled)
                return;
            
            if (_disposeCalled)
                return;

            if (!_authenticationService.IsLoggedIn)
                return;

            if (OverLastServiceCallTime())
            {
                _lastWorkStartTime = DateTime.Now;
                StartWorkCore();
            }
        }

        private bool OverLastServiceCallTime()
        {
            return DateTime.Now.Subtract(_lastWorkStartTime) > TimeSpan.FromSeconds(10);
        }

        private void StartWorkCore()
        {
            if (_authenticationService.IsLoggedIn)
                _userStateService.QueryGetUserStateAsync();
        }

        private void OnDteTrigger(object sender, EventArgs e)
        {
            StartWorkAsync();
        }

        public void Dispose()
        {
            _dtetrigger.OnDteTrigger -= OnDteTrigger;
            _dtetrigger.Dispose(); // todo: dispose this in containing class.
            _userStateService.QueryUserStatusCompleted -= HandleQueryUserStatusCompleted;
            _disposeCalled = true;
        }


        private void HandleQueryUserStatusCompleted(object sender, AsyncCompletedEventArgs<UserStateInfoResponse> e)
        {
            if (e.IsErrored())
                return;

            if (e.Result.State == TimeWarpState.None)
            {
                _userStateService.StartWorkAsync();
            }
        }

    }
}