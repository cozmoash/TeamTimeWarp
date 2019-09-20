using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Service
{
    public class RoomsConcurrentCache : ConcurrentCache<int,Room>
    {
        public RoomsConcurrentCache(IEnumerable<Room> rooms)
            : base(rooms.ToDictionary(x => x.Id, x => x))
        {}   
    }
}