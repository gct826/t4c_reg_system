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
    public class PaymentController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /Payment/

        public ActionResult Index()
        {
            return View(db.PaymentEntries.ToList());
        }

        //
        // GET: /Payment/Details/5

        public ActionResult Details(int id = 0)
        {
            PaymentEntry paymententry = db.PaymentEntries.Find(id);
            if (paymententry == null)
            {
                return HttpNotFound();
            }
            return View(paymententry);
        }

        //
        // GET: /Payment/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Payment/Create

        [HttpPost]
        public ActionResult Create(PaymentEntry paymententry)
        {
            if (ModelState.IsValid)
            {
                db.PaymentEntries.Add(paymententry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paymententry);
        }

        //
        // GET: /Payment/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PaymentEntry paymententry = db.PaymentEntries.Find(id);
            if (paymententry == null)
            {
                return HttpNotFound();
            }
            return View(paymententry);
        }

        //
        // POST: /Payment/Edit/5

        [HttpPost]
        public ActionResult Edit(PaymentEntry paymententry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymententry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymententry);
        }

        //
        // GET: /Payment/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PaymentEntry paymententry = db.PaymentEntries.Find(id);
            if (paymententry == null)
            {
                return HttpNotFound();
            }
            return View(paymententry);
        }

        //
        // POST: /Payment/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentEntry paymententry = db.PaymentEntries.Find(id);
            db.PaymentEntries.Remove(paymententry);
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