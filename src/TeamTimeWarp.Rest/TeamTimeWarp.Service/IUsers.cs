using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Service
{
    public interface IUsers
    {
        void Add(TimeWarpUser user);
        TimeWarpUser Get(long accountId);
    }
}