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
    public class AdminSmallGroupController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /AdminSmallGroup/

        public ActionResult Index()
        {
            var smallgroups = db.SmallGroups.Include(s => s.Services);
            return View(smallgroups.ToList());
        }

        //
        // GET: /AdminSmallGroup/Details/5
        [ChildActionOnly]
        public ActionResult Details(int id = 0)
        {
            ParticipantEntry participant = db.ParticipantEntries.Find(id);
            if (participant == null)
            {
                ViewBag.isFound = false;
                ViewBag.PartID = 0;
                return PartialView();
            }

            var SGCheck = from m in db.SmallGroups.Where(p => p.PartID.Equals(id))
                          select m;

            SmallGroup smallgroup = SGCheck.FirstOrDefault();

            if (smallgroup != null && smallgroup.PartID == id)
            {
                ViewBag.isFound = true;
                return PartialView(smallgroup);
            }

            ViewBag.isFound = false;
            ViewBag.PartID = participant.ParticipantID;
            return PartialView();
        }

        //
        // GET: /AdminSmallGroup/Create/Id

        public ActionResult Create(int id)
        {
            if (id == 0)
            {
                ViewBag.isFound = false;
                return View();
            }

            ParticipantEntry participant = db.ParticipantEntries.Find(id);
            if (participant == null)
            {
                return HttpNotFound();
            }

            var SGCheck = from m in db.SmallGroups.Where(p => p.PartID.Equals(id))
                          select m;

            SmallGroup smallgroup = SGCheck.FirstOrDefault();

            if (smallgroup != null && smallgroup.PartID == id)
            {
                return RedirectToAction("Edit", new { id = smallgroup.SmallGroupID });
            }

            if (smallgroup == null)
            {
                smallgroup = new SmallGroup();

                var SGLIst = new[] { "Small Group 1", "Small Group 2", "Small Group 3", "Small Group 4", "Small Group 5", "Small Group 6", "Small Group 7", "Small Group 8", "Small Group 9" };
                smallgroup.PartID = participant.ParticipantID;
                smallgroup.ServiceID = participant.ServiceID;

                ViewBag.PartID = participant.ParticipantID;
                ViewBag.SmallGroupName = new SelectList(SGLIst, smallgroup.SmallGroupName);
                ViewBag.isFound = true;

                return View(smallgroup);
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /AdminSmallGroup/Create/Id
        [HttpPost]
        public ActionResult Create(int id, SmallGroup smallgroup)
        {
            if (id == 0)
            {
                ViewBag.isFound = false;
                return View();
            }
            
            ParticipantEntry participant = db.ParticipantEntries.Find(id);
            if (participant == null)
            {
                return HttpNotFound();
            }

            var SGCheck = from m in db.SmallGroups.Where(p => p.PartID.Equals(id))
                          select m;

            SmallGroup sgcheck = SGCheck.FirstOrDefault();

            if (sgcheck != null && sgcheck.PartID == id)
            {
                return RedirectToAction("Edit", new { id =sgcheck.SmallGroupID });
            }


            if (participant.ParticipantID == smallgroup.PartID)
            {
                smallgroup.PartID = id;
                smallgroup.ServiceID = participant.ServiceID;

                db.SmallGroups.Add(smallgroup);
                db.SaveChanges();

                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(participant.RegistrationID, "Small Group Assigned", smallgroup.SmallGroupID);

                return RedirectToAction("Index", "AdminSmallGroupController", new { SerID = smallgroup.ServiceID });
            }
            
            var SGLIst = new[] { "Small Group 1", "Small Group 2", "Small Group 3", "Small Group 4", "Small Group 5", "Small Group 6", "Small Group 7", "Small Group 8", "Small Group 9" };

            ViewBag.SmallGroupName = new SelectList(SGLIst, smallgroup.SmallGroupName);

            ViewBag.isFound = true;
            ViewBag.PartID = participant.ParticipantID;
            
            return View(smallgroup);
        }

        //
        // GET: /AdminSmallGroup/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SmallGroup smallgroup = db.SmallGroups.Find(id);
            if (smallgroup == null)
            {
                return HttpNotFound();
            }
                        
            var SGLIst = new[] { "Small Group 1", "Small Group 2", "Small Group 3", "Small Group 4", "Small Group 5", "Small Group 6", "Small Group 7", "Small Group 8", "Small Group 9"};
            
            ViewBag.PartID = smallgroup.PartID;
            ViewBag.SmallGroupName = new SelectList(SGLIst,smallgroup.SmallGroupName);
            ViewBag.isFound = true;

            return View(smallgroup);
        }

        //
        // POST: /AdminSmallGroup/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, SmallGroup smallgroup)
        {
            if (ModelState.IsValid)
            {

                SmallGroup SGCheck = db.SmallGroups.Find(id);
                if (smallgroup == null)
                {
                    return HttpNotFound();
                }

                ParticipantEntry participant = db.ParticipantEntries.Find(smallgroup.PartID);
                if (participant == null)
                {
                    return HttpNotFound();
                }

                if (participant.ParticipantID != SGCheck.PartID)
                {
                    return HttpNotFound();
                }

                smallgroup.SmallGroupID = id;

                db.Entry(smallgroup).State = EntityState.Modified;
                db.SaveChanges();

                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(participant.RegistrationID, "Small Group Modified", smallgroup.SmallGroupID);

                return RedirectToAction("Index", "AdminSmallGroupController", new { SerID = smallgroup.ServiceID });
            }
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", smallgroup.ServiceID);
            return View(smallgroup);
        }

        //
        // GET: /AdminSmallGroup/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    SmallGroup smallgroup = db.SmallGroups.Find(id);
        //    if (smallgroup == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(smallgroup);
        //}

        //
        // POST: /AdminSmallGroup/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    SmallGroup smallgroup = db.SmallGroups.Find(id);
        //    db.SmallGroups.Remove(smallgroup);
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