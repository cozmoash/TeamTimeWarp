using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TeamTimeWarp.Persistence;
using TeamTimeWarp.Persistence.ClassMaps;
using TeamTimeWarp.Persistence.Repositories;
using TeamTimeWarp.Public.Converters;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Rest.Controllers
{
    public class RoomsController : ApiController
    {

        // GET api/values
        public IEnumerable<RoomInfo> Get()
        {
            IList<Domain.Entities.Room> rooms;
            using (var session = new NHibernateSessionPerRequest<RoomClassMap>())
            {
                RoomRepository roomRepository = new RoomRepository(session.GetCurrentSession());

                rooms = roomRepository.GetAll();
            }

            return rooms.Select(r => r.ConvertToPublicV001());
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post(string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}