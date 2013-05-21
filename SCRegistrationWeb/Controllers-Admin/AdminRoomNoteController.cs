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
    public class AdminRoomNoteController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /RoomNote/

        public ActionResult Index()
        {
            return View(db.RoomNotes.ToList());
        }

        //
        // GET: /RoomNote/Details/5

        public ActionResult Details(int id = 0)
        {
            RoomNote roomnote = db.RoomNotes.Find(id);
            if (roomnote == null)
            {
                ViewBag.RoomNoteFound = false;
                return PartialView();
            }
            ViewBag.RoomNoteFound = true;
            return PartialView(roomnote);
        }

        //
        // GET: /RoomNote/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /RoomNote/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomNote roomnote)
        {
            if (ModelState.IsValid)
            {
                db.RoomNotes.Add(roomnote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomnote);
        }

        //
        // GET: /RoomNote/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RoomNote roomnote = db.RoomNotes.Find(id);
            if (roomnote == null)
            {
                return HttpNotFound();
            }
            return View(roomnote);
        }

        //
        // POST: /RoomNote/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomNote roomnote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomnote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomnote);
        }

        //
        // GET: /RoomNote/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RoomNote roomnote = db.RoomNotes.Find(id);
            if (roomnote == null)
            {
                return HttpNotFound();
            }
            return View(roomnote);
        }

        //
        // POST: /RoomNote/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomNote roomnote = db.RoomNotes.Find(id);
            db.RoomNotes.Remove(roomnote);
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