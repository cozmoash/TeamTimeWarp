using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TeamTimeWarp.Domain.Entities.Repositories;
using TeamTimeWarp.Public.Converters;
using TeamTimeWarp.Public.Models.v001;

namespace TeamTimeWarp.Rest.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}
