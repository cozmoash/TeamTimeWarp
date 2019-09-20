using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Persistence;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;

namespace TeamTimeWarp.Rest.Controllers
{
    public class TimeWarpStatePersistence : ITimeWarpStatePersistence
    {
        public void SaveState(TimeWarpUserState newState)
        {
            //this should probably be on a new thread...and exception handling. logging ..etc
            using (var session = new NHibernateSessionPerRequest<TimeWarpUserStateClassMap>())
            {
                var repository = new TimeWarpUserStateRepository(session.GetCurrentSession());
                repository.Add(newState);
            }
        }
    }
}