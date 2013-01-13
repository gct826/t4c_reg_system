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
    public class AdminPricingController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /AdminPricing/
        [ChildActionOnly]
        public ActionResult Index()
        {
            var regprices = db.RegPrices.Include(r => r.AgeRange);
            return PartialView(regprices.ToList());
        }

        //
        // GET: /AdminPricing/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    RegPrice regprice = db.RegPrices.Find(id);
        //    if (regprice == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(regprice);
        //}

        //
        // GET: /AdminPricing/Create

        //public ActionResult Create()
        //{
        //    ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name");
        //    return View();
        //}

        //
        // POST: /AdminPricing/Create

        //[HttpPost]
        //public ActionResult Create(RegPrice regprice)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.RegPrices.Add(regprice);
        //        db.SaveChanges();
        //        return RedirectToAction("TableModification", "Admin");
        //    }

        //    ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", regprice.AgeRangeID);
        //    return View(regprice);
        //}

        //
        // GET: /AdminPricing/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RegPrice regprice = db.RegPrices.Find(id);
            if (regprice == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", regprice.AgeRangeID);
            return View(regprice);
        }

        //
        // POST: /AdminPricing/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, RegPrice regprice)
        {
            if (ModelState.IsValid)
            {
                regprice.RegTypeID = id;

                db.Entry(regprice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TableModification", "Admin");
            }
            ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", regprice.AgeRangeID);
            return View(regprice);
        }

        //
        // GET: /AdminPricing/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    RegPrice regprice = db.RegPrices.Find(id);
        //    if (regprice == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(regprice);
        //}

        //
        // POST: /AdminPricing/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    RegPrice regprice = db.RegPrices.Find(id);
        //    db.RegPrices.Remove(regprice);
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