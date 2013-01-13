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
    public class AdminServiceController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /AdminService/
        [ChildActionOnly]
        public ActionResult Index()

        {
            return PartialView(db.Services.ToList());
        }

        //
        // GET: /AdminService/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    Service service = db.Services.Find(id);
        //    if (service == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(service);
        //}

        //
        // GET: /AdminService/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /AdminService/Create

        //[HttpPost]
        //public ActionResult Create(Service service)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Services.Add(service);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(service);
        //}

        //
        // GET: /AdminService/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        //
        // POST: /AdminService/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Service service)
        {
            if (ModelState.IsValid)
            {
                service.ServiceID = id;
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TableModification", "Admin");
            }
            return View(service);
        }

        //
        // GET: /AdminService/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Service service = db.Services.Find(id);
        //    if (service == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(service);
        //}

        //
        // POST: /AdminService/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Service service = db.Services.Find(id);
        //    db.Services.Remove(service);
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