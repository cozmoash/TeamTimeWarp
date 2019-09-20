using System;
using System.Collections.Generic;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Client.Core.Services.Interfaces
{
    public interface IUiUserMessageService
    {
        void SendMessageAsync(long toAccount, string textMessage,Action onSendComplete);

        event EventHandler<AsyncCompletedEventArgs<ICollection<UserMessageReceipt>>> ReceivedMessages;
        void GetMessagesAsync();
    }
}