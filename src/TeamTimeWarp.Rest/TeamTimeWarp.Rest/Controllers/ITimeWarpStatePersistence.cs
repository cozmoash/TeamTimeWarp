using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Rest.Controllers
{
    public interface ITimeWarpStatePersistence
    {
        void SaveState(TimeWarpUserState timeWarpUserState);
    }
}