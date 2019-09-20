using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Service;

namespace TeamTimeWarp.Rest.Tests.Controllers
{
    public class FakeTimeWarpStatePersistence : IEntityPersistence<TimeWarpUserState>
    {
        public void Save(TimeWarpUserState timeWarpUserState)
        {
            //do nothing.
        }
    }
}