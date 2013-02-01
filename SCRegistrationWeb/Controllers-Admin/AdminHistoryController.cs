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
    }
}