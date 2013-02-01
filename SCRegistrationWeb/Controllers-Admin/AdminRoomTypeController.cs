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
    public class AdminRoomTypeController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /AdminRoomType/
        [ChildActionOnly]
        public ActionResult Index()
        {
            var roomtypes = db.RoomTypes.Include(r => r.RegTypes);
            return PartialView(roomtypes.ToList());
        }

        //
        // GET: /AdminRoomType/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    RoomType roomtype = db.RoomTypes.Find(id);
        //    if (roomtype == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(roomtype);
        //}

        //
        // GET: /AdminRoomType/Create

        public ActionResult Create()
        {
            int defaultRegTypeID = (int)1;
            ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", defaultRegTypeID);
            return View();
        }

        //
        // POST: /AdminRoomType/Create

        [HttpPost]
        public ActionResult Create(RoomType roomtype)
        {
            if (ModelState.IsValid)
            {
                if (roomtype.RegTypeID != 1)
                {
                    ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", roomtype.RegTypeID);
                    ViewBag.Message = "Can not add Room Type to Part Time rooms";
                    return View(roomtype);
                }
                else
                {
                    db.RoomTypes.Add(roomtype);
                    db.SaveChanges();
                    return RedirectToAction("TableModification", "Admin");
                }
            }

            ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", roomtype.RegTypeID);
            return View(roomtype);
        }

        //
        // GET: /AdminRoomType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RoomType roomtype = db.RoomTypes.Find(id);
            if (roomtype == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", roomtype.RegTypeID);
            return View(roomtype);
        }

        //
        // POST: /AdminRoomType/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, RoomType roomtype)
        {
            if (ModelState.IsValid)
            {
                roomtype.RoomTypeID = id;

                db.Entry(roomtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TableModification", "Admin");
            }
            ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", roomtype.RegTypeID);
            return View(roomtype);
        }

        //
        // GET: /AdminRoomType/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    RoomType roomtype = db.RoomTypes.Find(id);
        //    if (roomtype == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(roomtype);
        //}

        //
        // POST: /AdminRoomType/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    RoomType roomtype = db.RoomTypes.Find(id);
        //    db.RoomTypes.Remove(roomtype);
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