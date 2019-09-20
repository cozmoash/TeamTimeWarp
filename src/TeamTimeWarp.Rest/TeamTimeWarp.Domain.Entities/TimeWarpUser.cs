using System;

namespace TeamTimeWarp.Domain.Entities
{
    public class TimeWarpUser
    {
        private readonly ITimeWarpStateCalculator _timeWarpStateCalculator;
        private TimeWarpUserState _timeWarpUserState;

        public TimeWarpUser(Account account, 
                            ITimeWarpStateCalculator timeWarpStateCalculator)
            : this(new TimeWarpUserState(account, TimeWarpState.None, default(DateTime), default(TimeSpan),
                default(double)), timeWarpStateCalculator)
        {}

        public TimeWarpUser(TimeWarpUserState state, ITimeWarpStateCalculator timeWarpStateCalculator)
        {
            Account = state.Account;
            _timeWarpStateCalculator = timeWarpStateCalculator;
            _timeWarpUserState = state;
        }

        public virtual Account Account { get; protected set; }

        public TimeWarpUserState StartWork(DateTime time)
        {
            _timeWarpUserState = new TimeWarpUserState(Account,TimeWarpState.Working, time,TimeSpan.Zero,0.0);
            return _timeWarpUserState;
        }

        public TimeWarpUserState StartRest(DateTime time)
        {
            _timeWarpUserState = new TimeWarpUserState(Account, TimeWarpState.Resting, time, TimeSpan.Zero, 0.0);
            return _timeWarpUserState;
        }

        public TimeWarpUserState CurrentState(DateTime time)
        {
            return _timeWarpStateCalculator.RecalculateTimeWarpState(_timeWarpUserState, time);
        }

    }
}