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
    public class AdminAgeRangeController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /AdminAgeRange/
        [ChildActionOnly]
        public ActionResult Index()
        {
            return PartialView(db.AgeRanges.ToList());
        }

        //
        // GET: /AdminAgeRange/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    AgeRange agerange = db.AgeRanges.Find(id);
        //    if (agerange == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(agerange);
        //}

        //
        // GET: /AdminAgeRange/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /AdminAgeRange/Create

        //[HttpPost]
        //public ActionResult Create(AgeRange agerange)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.AgeRanges.Add(agerange);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(agerange);
        //}

        //
        // GET: /AdminAgeRange/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AgeRange agerange = db.AgeRanges.Find(id);
            if (agerange == null)
            {
                return HttpNotFound();
            }
            return View(agerange);
        }

        //
        // POST: /AdminAgeRange/Edit/5

        [HttpPost]
        public ActionResult Edit(int id,AgeRange agerange)
        {
            if (ModelState.IsValid)
            {
                agerange.AgeRangeID = id;

                db.Entry(agerange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TableModification", "Admin");
            }
            return View(agerange);
        }

        //
        // GET: /AdminAgeRange/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    AgeRange agerange = db.AgeRanges.Find(id);
        //    if (agerange == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(agerange);
        //}

        //
        // POST: /AdminAgeRange/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    AgeRange agerange = db.AgeRanges.Find(id);
        //    db.AgeRanges.Remove(agerange);
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