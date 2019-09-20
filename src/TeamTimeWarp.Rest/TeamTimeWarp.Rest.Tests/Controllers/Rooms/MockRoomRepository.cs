using System.Collections.Generic;
using System.Linq;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Rest.Tests.Controllers.Accounts;

namespace TeamTimeWarp.Rest.Tests.Controllers.Rooms
{
    public class MockRoomRepository : MockRepository<Room>, IRoomRepository
    {
        public override void Add(Room item)
        {
            Items[item.Id] = item;
        }

        public Room GetRoom(int id)
        {
            Room room;
            if(Items.TryGetValue(id,out room))
            {
                return room;
            }
            return null;
        }

        public Room GetRoom(string roomName)
        {
            var lowerRoomName = roomName.ToLower();
            return Items.Values.SingleOrDefault(x => x.Name.ToLower().Equals(lowerRoomName));
        }

        public IEnumerable<Room> GetRooms(long accountId)
        {
            return Items.Values.Where(x => x.Users.Any(user => user.Id.Equals(accountId))).ToList();
        }

        public IEnumerable<Room> GetRooms(string searchString)
        {
            return Items.Select(x => x.Value).Where(x => x.Name.Contains(searchString));
        }

        public IEnumerable<Room> GetPopularRooms()
        {
            throw new System.NotImplementedException();
        }
    }
}