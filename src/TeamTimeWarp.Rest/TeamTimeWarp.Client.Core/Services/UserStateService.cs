using System;
using RestSharp;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services
{
    public class UserStateService : UiServiceBase, IUserStateService
    {
        private readonly IAgentTypeProvider _agentTypeProvider;

        public UserStateService(TokenStore tokenStore, IRestServiceUriFactory restServiceUriFactory, IAgentTypeProvider agentTypeProvider)
            : base(tokenStore, restServiceUriFactory)
        {
            _agentTypeProvider = agentTypeProvider;
        }

        public void StartWorkAsync()
        {
            ExecuteRequestAsync(string.Format("userstate/?command={0}&agent={1}", (int)TimeWarpCommand.Work,(int)_agentTypeProvider.Agent),
                           Method.POST,_ =>
                               {
                                   Console.WriteLine("hello");
                               });
        }

        public void StopWorkAsync()
        {
            ExecuteRequestAsync(string.Format("userstate/?command={0}&agent={1}", (int)TimeWarpCommand.Rest, (int)_agentTypeProvider.Agent),
                           Method.POST, _ => {});

        }

        public event EventHandler<AsyncCompletedEventArgs<UserStateInfoResponse>> QueryUserStatusCompleted;

        public void QueryGetUserStateAsync()
        {
            ExecuteRequestAsync("userstate", Method.GET, restResponse => AsyncCompletedEventArgsExtensions.Raise(QueryUserStatusCompleted,restResponse));
        } 
    }
}