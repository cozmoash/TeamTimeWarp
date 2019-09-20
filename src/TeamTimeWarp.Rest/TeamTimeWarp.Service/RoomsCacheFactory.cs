using System.Collections.Generic;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Persistence;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;
using TeamTimeWarp.Persistence.Rooms;

namespace TeamTimeWarp.Service
{
    //public static class RoomsCacheFactory
    //{
    //    public static ConcurrentCache<int,Room> Get()
    //    {
    //        IList<Room> result;
    //        using (var session = new NHibernateTransaction<RoomClassMap>())
    //        {
    //            var roomRepository = new RoomRepository(session.GetCurrentSession());
    //            result = roomRepository.GetAll();
    //        }
    //        return  new RoomsConcurrentCache(result);
    //    }
    //}
}