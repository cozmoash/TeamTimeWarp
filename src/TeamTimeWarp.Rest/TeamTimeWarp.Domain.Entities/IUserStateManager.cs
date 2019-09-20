using System;

namespace TeamTimeWarp.Domain.Entities
{
    public interface IUserStateManager
    {
        TimeWarpUserState StartWork(Account account, DateTime time, int agentType);
        TimeWarpUserState StartRest(Account account, DateTime time, int agentType);
        bool TryGetCurrentState(long accountId, DateTime time, out TimeWarpUserState timeWarpUserState);
    }
}