using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using TeamTimeWarp.Domain.Entities;
using TeamTimeWarp.Domain.Entities.Repositories;

namespace TeamTimeWarp.Persistence.Repositories
{
    public class RoomRepository : PersistenceRepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(ISession session)
            : base(session)
        {
        }

        public Room GetRoom(int id)
        {
            return Session.Query<Room>().SingleOrDefault(x=>x.Id == id);
        }

       

  
    }
}