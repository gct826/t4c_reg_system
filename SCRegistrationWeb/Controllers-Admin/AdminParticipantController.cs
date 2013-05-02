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
    public class AdminParticipantController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /AdminParticipant/

        public ActionResult Index()
        {
            var participantentries = db.ParticipantEntries.Include(p => p.RegistrationEntries).Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p => p.RoomTypes);
            return View(participantentries.ToList());
        }

        //
        // GET: /AdminParticipant/Details/5

        public ActionResult Details(int id = 0)
        {
            ParticipantEntry participantentry = db.ParticipantEntries.Find(id);
            if (participantentry == null)
            {
                return HttpNotFound();
            }
            return View(participantentry);
        }

        //
        // GET: /AdminParticipant/Create

        public ActionResult Create()
        {
            ViewBag.RegistrationID = new SelectList(db.RegEntries, "RegistrationID", "RegistrationUID");
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "Name");
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
            ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name");
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name");
            ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name");
            ViewBag.FellowshipID = new SelectList(db.Fellowships, "FellowshipID", "Name");
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "Name");
            return View();
        }

        //
        // POST: /AdminParticipant/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ParticipantEntry participantentry)
        {
            if (ModelState.IsValid)
            {
                db.ParticipantEntries.Add(participantentry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RegistrationID = new SelectList(db.RegEntries, "RegistrationID", "RegistrationUID", participantentry.RegistrationID);
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "Name", participantentry.StatusID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", participantentry.ServiceID);
            ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", participantentry.AgeRangeID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", participantentry.GenderID);
            ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", participantentry.RegTypeID);
            ViewBag.FellowshipID = new SelectList(db.Fellowships, "FellowshipID", "Name", participantentry.FellowshipID);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "Name", participantentry.RoomTypeID);
            return View(participantentry);
        }

        //
        // GET: /AdminParticipant/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ParticipantEntry participantentry = db.ParticipantEntries.Find(id);
            if (participantentry == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegistrationID = new SelectList(db.RegEntries, "RegistrationID", "RegistrationUID", participantentry.RegistrationID);
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "Name", participantentry.StatusID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", participantentry.ServiceID);
            ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", participantentry.AgeRangeID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", participantentry.GenderID);
            ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", participantentry.RegTypeID);
            ViewBag.FellowshipID = new SelectList(db.Fellowships, "FellowshipID", "Name", participantentry.FellowshipID);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "Name", participantentry.RoomTypeID);
            return View(participantentry);
        }

        //
        // POST: /AdminParticipant/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ParticipantEntry participantentry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(participantentry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegistrationID = new SelectList(db.RegEntries, "RegistrationID", "RegistrationUID", participantentry.RegistrationID);
            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "Name", participantentry.StatusID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", participantentry.ServiceID);
            ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", participantentry.AgeRangeID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", participantentry.GenderID);
            ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", participantentry.RegTypeID);
            ViewBag.FellowshipID = new SelectList(db.Fellowships, "FellowshipID", "Name", participantentry.FellowshipID);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "Name", participantentry.RoomTypeID);
            return View(participantentry);
        }

        //
        // GET: /AdminParticipant/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ParticipantEntry participantentry = db.ParticipantEntries.Find(id);
            if (participantentry == null)
            {
                return HttpNotFound();
            }
            return View(participantentry);
        }

        //
        // POST: /AdminParticipant/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParticipantEntry participantentry = db.ParticipantEntries.Find(id);
            db.ParticipantEntries.Remove(participantentry);
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