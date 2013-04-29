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
    public class AdminRegistrationController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /AdminRegistration/

        public ActionResult Index()
        {
            return View(db.RegEntries.ToList());
        }

        //
        // GET: /AdminRegistration/Details/5

        public ActionResult Details(int id = 0)
        {
            RegistrationEntry registrationentry = db.RegEntries.Find(id);
            if (registrationentry == null)
            {
                return HttpNotFound();
            }
            return View(registrationentry);
        }

        //
        // GET: /AdminRegistration/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AdminRegistration/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegistrationEntry registrationentry)
        {
            if (ModelState.IsValid)
            {
                db.RegEntries.Add(registrationentry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(registrationentry);
        }

        //
        // GET: /AdminRegistration/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RegistrationEntry registrationentry = db.RegEntries.Find(id);
            if (registrationentry == null)
            {
                return HttpNotFound();
            }
            return View(registrationentry);
        }

        //
        // POST: /AdminRegistration/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegistrationEntry registrationentry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registrationentry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registrationentry);
        }

        //
        // GET: /AdminRegistration/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RegistrationEntry registrationentry = db.RegEntries.Find(id);
            if (registrationentry == null)
            {
                return HttpNotFound();
            }
            return View(registrationentry);
        }

        //
        // POST: /AdminRegistration/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RegistrationEntry registrationentry = db.RegEntries.Find(id);
            db.RegEntries.Remove(registrationentry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}