using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Public.Converters;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Rest.Controllers
{
    public class GlobalRoomInfoController : ApiController
    {
        private readonly IRoomRepository _roomRepository;

        public GlobalRoomInfoController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public IEnumerable<RoomInfo> Get()
        {
            return _roomRepository.GetAll().Select(x => RoomConverter.ConvertToPublicV001(x));
        }


        public IEnumerable<RoomInfo> Get(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return Enumerable.Empty<RoomInfo>();

            var matches = _roomRepository.GetRooms(searchString);
            return matches.Select(x => x.ConvertToPublicV001()).ToArray();
        }

        public RoomInfo Get(int id)
        {
            var room = _roomRepository.GetRoom(id);

            if (room == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "No Room with that id" });

            return room.ConvertToPublicV001();
        }
    }
}