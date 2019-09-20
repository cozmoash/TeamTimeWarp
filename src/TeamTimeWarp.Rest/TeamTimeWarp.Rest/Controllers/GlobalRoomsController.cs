using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Public.Converters;

namespace TeamTimeWarp.Rest.Controllers
{
    public class GlobalRoomsController : Controller
    {
         private readonly IRoomRepository _roomRepository;

        public GlobalRoomsController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        //
        // GET: /GlobalRooms/

        public ActionResult Index()
        {
            var rooms = _roomRepository.GetAll().Select(x => RoomConverter.ConvertToPublicV001(x));
            return View(rooms);
        }
    }
}
