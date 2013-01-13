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
    public class AdminStatusController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /AdminStatus/
        [ChildActionOnly]
        public ActionResult Index()
        {
            return PartialView(db.Statuses.ToList());
        }

        //
        // GET: /AdminStatus/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    Status status = db.Statuses.Find(id);
        //    if (status == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(status);
        //}

        //
        // GET: /AdminStatus/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /AdminStatus/Create

        //[HttpPost]
        //public ActionResult Create(Status status)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Statuses.Add(status);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(status);
        //}

        //
        // GET: /AdminStatus/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Status status = db.Statuses.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        //
        // POST: /AdminStatus/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Status status)
        {
            if (ModelState.IsValid)
            {
                status.StatusID = id;
                db.Entry(status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TableModification", "Admin");
            }
            return View(status);
        }

        //
        // GET: /AdminStatus/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Status status = db.Statuses.Find(id);
        //    if (status == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(status);
        //}

        //
        // POST: /AdminStatus/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Status status = db.Statuses.Find(id);
        //    db.Statuses.Remove(status);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}