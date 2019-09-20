using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TeamTimeWarp.Domain.Entities;

namespace TeamTimeWarp.Service
{
    public class RoomsCache : Cache<int,Room>
    {
        public RoomsCache(IEnumerable<Room> rooms)
            : base(rooms.ToDictionary(x => x.Id, x => x))
        {}   
    }
}