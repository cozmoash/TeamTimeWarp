using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;

namespace TeamTimeWarp.Persistence.Rooms
{
    public class RoomRepository : PersistenceRepositoryBase<Room, RoomClassMap>, IRoomRepository
    {
        public RoomRepository()
            : base(new TimeWarpSessionFactory<RoomClassMap>())
        {
        }




        public IEnumerable<Room> GetRooms(string searchString)
        {
            using (var session = SessionFactory.Get())
            {
                return (from room in session.Query<Room>()
                        where room.Name.Contains(searchString)
                       select room).Take(10).ToArray();
            }
        }

        public IEnumerable<Room> GetPopularRooms()
        {
            using (var session = SessionFactory.Get())
            {
                return (from room in session.Query<Room>()
                        orderby room.NumberOfUsers descending 
                        select room
                        ).Take(10).ToArray();
            }
        }

        public Room GetRoom(int id)
        {
            using (var session = SessionFactory.Get())
            {
                return session.Query<Room>().SingleOrDefault(x => x.Id == id);
            }
        }

        public Room GetRoom(string roomName)
        {
            var lowerRoomName = roomName.ToLower();

            using (var session = SessionFactory.Get())
            {
                return session.Query<Room>().SingleOrDefault(x => x.Name.ToLower() == lowerRoomName);
            }
        }

        public IEnumerable<Room> GetRooms(long accountId)
        {
         //todo: fix performance with proper lookup.
            return GetAll().Where(x => x.Users.Any(user => user.Id.Equals(accountId)));

            //using (var session = SessionFactory.Get())
            //{
            //    return session.Query<Room>().Where(x => x.Users.Any(user=>user.Id.Equals(accountId)));
            //}
        }
    }
}