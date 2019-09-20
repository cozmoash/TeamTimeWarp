using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using RestSharp;
using TeamTimeWarp.Client.Core.Services.Interfaces;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services
{
    public class UserMessageService : UiServiceBase, IUiUserMessageService
    {
        public UserMessageService(TokenStore tokenStore, IRestServiceUriFactory restServiceUriFactory,
                                  SynchronizationContext synchronizationContext = null)
            : base(tokenStore, restServiceUriFactory, synchronizationContext)
        {
        }

        public event EventHandler<AsyncCompletedEventArgs<ICollection<UserMessageReceipt>>> ReceivedMessages;

        public void SendMessageAsync(long toAccount, string textMessage, Action onComplete)
        {
            var request = CreateRequest(string.Format("UserMessage/?toAccount={0}", toAccount), Method.POST);
            request.AddHeader("textMessage", textMessage);

            ExecuteRequestAsync(request,
                                response => SynchronizationContext.Post(_ => onComplete(),null));
        }


        public void GetMessagesAsync()
        {
            ExecuteRequestAsync("UserMessage", Method.GET,
                                response =>
                                AsyncCompletedEventArgsExtensions.Raise<ICollection<UserMessageReceipt>>(
                                    ReceivedMessages, response));
        }
    }
}