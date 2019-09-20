using System;

namespace TeamTimeWarp.Domain.Entities
{
    public class UserMessage
    {
        public virtual long Id { get; protected set; }
        public virtual Account ToAccount { get; protected set; }
        public virtual Account FromAccount { get; protected set; }
        public virtual string TextMessage { get; protected set; }
        public virtual DateTime SendTime { get; protected set; }
        public virtual bool HasBeenReceived { get;  set; }

        public UserMessage(Account toAccount, Account fromAccount, DateTime sendTime, string message)
        {
            ToAccount = toAccount;
            FromAccount = fromAccount;
            SendTime = sendTime;
            TextMessage = message;
            HasBeenReceived = false;
        }

        protected UserMessage()
        {}
    }
}