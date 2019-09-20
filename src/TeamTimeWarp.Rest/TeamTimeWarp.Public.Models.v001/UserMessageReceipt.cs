using System;
using System.Runtime.Serialization;

namespace TeamTimeWarp.Public.Models.v001
{
    [DataContract]
    public sealed class UserMessageReceipt
    {
        public UserMessageReceipt(long fromId, string fromName, string message, DateTime sendTime)
        {
            FromId = fromId;
            FromName = fromName;
            Message = message;
            SendTime = sendTime;
        }

        [DataMember]
        public long FromId { get; private set; }

        [DataMember]
        public string FromName { get; private set; }

        [DataMember]
        public string Message { get; private set; }

        [DataMember]
        public DateTime SendTime { get; private set; }
    }
}