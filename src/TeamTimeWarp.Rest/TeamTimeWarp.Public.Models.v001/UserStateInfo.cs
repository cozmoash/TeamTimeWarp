using System;
using System.Runtime.Serialization;

namespace TeamTimeWarp.Public.Models.v001
{
    [DataContract]
    public sealed class UserStateInfo
    {
        public UserStateInfo(long accountId, TimeWarpState state, DateTime periodStartTime,
                        TimeSpan timeLeft, double progress)
        {
            AccountId = accountId;
            State = state;
            PeriodStartTime = periodStartTime;
            TimeLeft = timeLeft;
            Progress = progress;
        }

      
        [DataMember]
        public long AccountId { get; private set; }

        [DataMember]
        public TimeWarpState State { get; private set; }

        [DataMember]
        public DateTime PeriodStartTime { get; private set; }

        [DataMember]
        public TimeSpan TimeLeft { get; private set; }

        [DataMember]
        public double Progress { get; private set; }
    }
}