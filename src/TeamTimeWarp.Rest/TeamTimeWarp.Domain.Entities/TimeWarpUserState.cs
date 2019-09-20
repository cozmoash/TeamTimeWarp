using System;

namespace TeamTimeWarp.Domain.Entities
{
    /// <summary>
    /// This class is responsible for storing the state of a user at a specific point in time.
    /// </summary>
    public class TimeWarpUserState
    {
        public TimeWarpUserState(Account account, TimeWarpState state, DateTime periodStartTime, TimeSpan timeLeft, double progress, int agentType)
        {
            Account = account;
            State = state;
            PeriodStartTime = periodStartTime;
            TimeLeft = timeLeft;
            Progress = progress;
            AgentType = agentType;
        }

        public TimeWarpUserState(long id ,Account account, TimeWarpState state, DateTime periodStartTime, TimeSpan timeLeft, double progress, int agentType)
        {
            Id = id;
            Account = account;
            State = state;
            PeriodStartTime = periodStartTime;
            TimeLeft = timeLeft;
            Progress = progress;
            AgentType = agentType;
        }

        protected TimeWarpUserState()
        {
            
        }

        public virtual long Id { get; protected set; }

        public virtual Account Account { get; protected set; }
        public virtual TimeWarpState State { get; protected set; }
        public virtual DateTime PeriodStartTime { get; protected set; }
        public virtual TimeSpan TimeLeft { get; protected set; }
        public virtual double Progress { get; protected set; }
        public virtual int AgentType { get; protected set; }

        public static TimeWarpUserState DefaultState(Account account)
        {
            return new TimeWarpUserState(account, TimeWarpState.None, new DateTime(2000,1,1), default(TimeSpan),
                                         default(double), 1);
        }
    }
}