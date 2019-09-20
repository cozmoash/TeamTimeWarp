using System;
using TeamTimeWarp.Domain.Entities.Repositories;

namespace TeamTimeWarp.Domain.Entities
{
    public class UserStateManager : IUserStateManager
    {
        private readonly ITimeWarpUserStateRepository _userStateRepository;

        private readonly ITimeWarpStateCalculator _timeWarpStateCalculator;

        public UserStateManager(ITimeWarpStateCalculator timeWarpStateCalculator,
                                ITimeWarpUserStateRepository userStateRepository)
        {
            _timeWarpStateCalculator = timeWarpStateCalculator;
            _userStateRepository = userStateRepository;
        }
        
        public TimeWarpUserState StartWork(Account account, DateTime time, int agentType)
        {
            return SaveNewState(account, time, TimeWarpState.Working, agentType);
        }

        public TimeWarpUserState StartRest(Account account, DateTime time, int agentType)
        {
            return SaveNewState(account, time, TimeWarpState.Resting, agentType);
        }

        public bool TryGetCurrentState(long accountId, DateTime time, out TimeWarpUserState timeWarpUserState)
        {
            var currentState = _userStateRepository.GetLatestStateByAccountId(accountId);
            if (currentState == null)
            {
                timeWarpUserState = null;
                return false;
            }
            timeWarpUserState =  _timeWarpStateCalculator.RecalculateTimeWarpState(currentState, time);
            return true;
        }

        private TimeWarpUserState SaveNewState(Account account, DateTime time, TimeWarpState timeWarpState, int agentType)
        {
            var updatedState = new TimeWarpUserState(account, timeWarpState, time, TimeSpan.Zero, 0.0, agentType);
            _userStateRepository.Add(updatedState);
            return updatedState;
        }
    }
}