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
    public class AdminHeadsetController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /AdminHeadset/

        public ActionResult Index()
        {
            var headsets = db.Headsets.Include(h => h.ParticipantEntries);
            return View(headsets.ToList());
        }

        //
        // GET: /Headset/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    Headset headset = db.Headsets.Find(id);
        //    if (headset == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(headset);
        //}

        //
        // GET: /Headset/Create

        //public ActionResult Create()
        //{
        //    ViewBag.ParticipantID = new SelectList(db.ParticipantEntries, "ParticipantID", "FirstName");
        //    return View();
        //}

        //
        // POST: /Headset/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Headset headset)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Headsets.Add(headset);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ParticipantID = new SelectList(db.ParticipantEntries, "ParticipantID", "FirstName", headset.ParticipantID);
        //    return View(headset);
        //}

        //
        // GET: /Headset/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    Headset headset = db.Headsets.Find(id);
        //    if (headset == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ParticipantID = new SelectList(db.ParticipantEntries, "ParticipantID", "FirstName", headset.ParticipantID);
        //    return View(headset);
        //}

        //
        // POST: /Headset/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Headset headset)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(headset).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ParticipantID = new SelectList(db.ParticipantEntries, "ParticipantID", "FirstName", headset.ParticipantID);
        //    return View(headset);
        //}

        //
        // GET: /AdminHeadset/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Headset headset = db.Headsets.Find(id);
            if (headset == null)
            {
                return HttpNotFound();
            }

            var participant = db.ParticipantEntries.Where(m => m.ParticipantID.Equals(id));
        
            ViewBag.FirstName = participant.FirstOrDefault().FirstName;
            ViewBag.LastName = participant.FirstOrDefault().LastName;

            return View(headset);
        }

        //
        // POST: /AdminHeadset/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Headset headset = db.Headsets.Find(id);
            db.Headsets.Remove(headset);
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