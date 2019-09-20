using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using RestSharp;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services
{
    public class GlobalRoomsService : UiServiceBase, IUiGlobalRoomsService
    {
        public GlobalRoomsService(TokenStore tokenStore, IRestServiceUriFactory restServiceUriFactory, SynchronizationContext synchronizationContext) : base(tokenStore, restServiceUriFactory,synchronizationContext)
        {
        }

        public event EventHandler<AsyncCompletedEventArgs<ICollection<RoomInfo>>> SearchCompleted;

        public void SearchAsync(string searchString)
        {
            ExecuteRequestAsync(string.Format("globalroominfo/?searchstring={0}", searchString),
                                        Method.GET, restResponse => AsyncCompletedEventArgsExtensions.Raise(SearchCompleted,restResponse));
        }

    }
}