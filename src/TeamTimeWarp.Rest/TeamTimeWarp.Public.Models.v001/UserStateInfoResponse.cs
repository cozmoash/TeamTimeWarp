using System;
using System.Runtime.Serialization;

namespace TeamTimeWarp.Public.Models.v001
{
    [DataContract]
    public sealed class UserStateInfoResponse
    {
        public UserStateInfoResponse(long accountId, string username,DateTime queryTime, TimeWarpState state, DateTime periodStartTime,
                                     TimeSpan timeLeft, double progress, bool isQuickLoginUser, TimeWarpAgent timeWarpAgent)
        {
            AccountId = accountId;
            QueryTime = queryTime;
            State = state;
            PeriodStartTime = periodStartTime;
            TimeLeft = timeLeft;
            Progress = progress;
            Username = username;
            IsQuickLoginUser = isQuickLoginUser;
            TimeWarpAgent = timeWarpAgent;
        }

      
        [DataMember]
        public long AccountId { get; private set; }

        [DataMember]
        public string Username { get; private set; }

        [DataMember]
        public DateTime QueryTime { get; private set; }

        [DataMember]
        public TimeWarpState State { get; private set; }

        [DataMember]
        public DateTime PeriodStartTime { get; private set; }

        [DataMember]
        public TimeSpan TimeLeft { get; private set; }

        [DataMember]
        public double Progress { get; private set; }

        [DataMember]
        public bool IsQuickLoginUser { get; private set; }

        [DataMember]
        public TimeWarpAgent TimeWarpAgent { get; private set; }
    }
}