namespace TeamTimeWarp.Domain.Entities.Repositories
{
    public interface ITimeWarpUserStateRepository : IRepository<TimeWarpUserState>
    {
        TimeWarpUserState GetLatestStateByAccountId(long accountId);
    }
}