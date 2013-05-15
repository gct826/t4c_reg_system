using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCRegistrationWeb.Models;

namespace SCRegistrationWeb.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BuildingController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /Building/

        public ActionResult Index()
        {
            var buildings = db.Buildings.Include(b => b.RoomTypes);
            return View(buildings.ToList());
        }

        //
        // GET: /Building/Details/5

        public ActionResult Details(int id = 0)
        {
            Building building = db.Buildings.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        //
        // GET: /Building/TotalBeds/5

        public ActionResult TotalBeds(int id = 0)
        {
            Building building = new Building();

            if (id != 0)
            {
                ViewBag.TotalBeds = building.TotalBeds(id);
                return PartialView();
            }

            return PartialView();
        }

        

    }
}