using System.Collections.Generic;

namespace TeamTimeWarp.Domain.Entities.Repositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        Room GetRoom(int id);

        Room GetRoom(string roomName);

        IEnumerable<Room> GetRooms(long accountId);

        IEnumerable<Room> GetRooms(string searchString);

        IEnumerable<Room> GetPopularRooms();

    }
}