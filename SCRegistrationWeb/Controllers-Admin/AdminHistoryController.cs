using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCRegistrationWeb.Models;

namespace SCRegistrationWeb.Controllers_Admin
{
    [Authorize(Roles = "Administrator")]
    public class AdminHistoryController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /AdminHistory/

        public ActionResult Index()
        {
            return View(db.EventHistory.ToList());
        }

        //
        // GET: /AdminHistory/Reg/ID

        public ActionResult Reg(int id =0 )
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            
            var eventhistories = from m in db.EventHistory.Where(p => p.RegHistID.Equals(id))
                select m;

            if (eventhistories != null)
            {
                return View(eventhistories.ToList());
            }

            return RedirectToAction("Index");
            
        }

    }
}