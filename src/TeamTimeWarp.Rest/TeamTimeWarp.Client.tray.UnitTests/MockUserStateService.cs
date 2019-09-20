using System;
using System.IO;
using TeamTimeWarp.Client.Core.Services;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.tray.UnitTests
{
    public class MockUserStateService : IUserStateService
    {
        private UserStateInfoResponse _userStateInfoResponse;

        public void StartWorkAsync()
        {
            //throw new NotImplementedException();
        }

        public void StopWorkAsync()
        {
            //throw new NotImplementedException();
        }

        public event EventHandler<AsyncCompletedEventArgs<UserStateInfoResponse>> QueryUserStatusCompleted;

        protected virtual void OnQueryUserStatusCompleted(AsyncCompletedEventArgs<UserStateInfoResponse> e)
        {
            
        }

        public void SetUserState(UserStateInfoResponse userStateInfoResponse)
        {
            _userStateInfoResponse = userStateInfoResponse;
        }

        public void QueryGetUserStateAsync()
        {
            EventHandler<AsyncCompletedEventArgs<UserStateInfoResponse>> handler = QueryUserStatusCompleted;
            if (handler != null) handler(this, new AsyncCompletedEventArgs<UserStateInfoResponse>(null,null,_userStateInfoResponse));
        }

        public UserStateInfoResponse GetUserState()
        {
            return _userStateInfoResponse;
        }
    }

    public class MockInvalidUserStateService : IUserStateService
    {

        public void StartWorkAsync()
        {
            //throw new NotImplementedException();
        }

        public void StopWorkAsync()
        {
            //throw new NotImplementedException();
        }

        public event EventHandler<AsyncCompletedEventArgs<UserStateInfoResponse>> QueryUserStatusCompleted;

        public void QueryGetUserStateAsync()
        {
            EventHandler<AsyncCompletedEventArgs<UserStateInfoResponse>> handler = QueryUserStatusCompleted;
            if (handler != null) handler(this, new AsyncCompletedEventArgs<UserStateInfoResponse>(new InvalidDataException(), null, null));
        }

        public UserStateInfoResponse GetUserState()
        {
            throw new InvalidDataException();
        }
    }
}