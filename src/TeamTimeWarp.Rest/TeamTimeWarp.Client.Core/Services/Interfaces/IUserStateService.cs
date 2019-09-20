using System;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services
{
    public interface IUserStateService
    {
        void StartWorkAsync();
        void StopWorkAsync();

        event EventHandler<AsyncCompletedEventArgs<UserStateInfoResponse>> QueryUserStatusCompleted;
        void QueryGetUserStateAsync();

        

    }
}