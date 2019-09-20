
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Public.Converters
{
    public static class RoomConverter
    {
        public static RoomInfo ConvertToPublicV001(this Room room)
        {
            return new RoomInfo(room.Id,room.Name,room.CreationTime,room.NumberOfUsers);
        }
    }
}
