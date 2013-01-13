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
    public class AdminFellowshipController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /AdminFellowship/
        [ChildActionOnly]
        public ActionResult Index()
        {
            var fellowships = db.Fellowships.Include(f => f.Service);
            return PartialView(fellowships.ToList());
        }

        //
        // GET: /AdminFellowship/Details/5
        //
        //public ActionResult Details(int id = 0)
        //{
        //    Fellowship fellowship = db.Fellowships.Find(id);
        //    if (fellowship == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fellowship);
        //}

        //
        // GET: /AdminFellowship/Create

        public ActionResult Create()
        {
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
            return View();
        }

        //
        // POST: /AdminFellowship/Create

        [HttpPost]
        public ActionResult Create(Fellowship fellowship)
        {
            if (ModelState.IsValid)
            {
                db.Fellowships.Add(fellowship);
                db.SaveChanges();
                return RedirectToAction("TableModification", "Admin");
            }

            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", fellowship.ServiceID);
            return View(fellowship);
        }

        //
        // GET: /AdminFellowship/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Fellowship fellowship = db.Fellowships.Find(id);
            if (fellowship == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", fellowship.ServiceID);
            return View(fellowship);
        }

        //
        // POST: /AdminFellowship/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Fellowship fellowship)
        {
            if (ModelState.IsValid)
            {
                fellowship.FellowshipID = id;
                db.Entry(fellowship).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TableModification","Admin");
            }
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", fellowship.ServiceID);
            return View(fellowship);
        }

        //
        // GET: /AdminFellowship/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Fellowship fellowship = db.Fellowships.Find(id);
        //    if (fellowship == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(fellowship);
        //}

        //
        // POST: /AdminFellowship/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Fellowship fellowship = db.Fellowships.Find(id);
        //    db.Fellowships.Remove(fellowship);
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