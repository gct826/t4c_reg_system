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
    //[Authorize(Roles = "Administrator")]
    public class SearchRegistrationController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /SearchRegistration/

        public ActionResult Index(string searchPhone, string searcheMail, string searchName, int StatusID=0)
        {
            var participantentries = db.ParticipantEntries.Include(p => p.RegistrationEntries).Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships);


            if (!string.IsNullOrEmpty(searchPhone))
            {
                participantentries = participantentries.Where(s => s.RegistrationEntries.Phone.Contains(searchPhone));
            }

            if (!string.IsNullOrEmpty(searcheMail))
            {
                participantentries = participantentries.Where(s => s.RegistrationEntries.Email.Contains(searcheMail));
            }

            if (!string.IsNullOrEmpty(searchName))
            {
                participantentries = participantentries.Where(s => s.FirstName.Contains(searchName) || s.LastName.Contains(searchName));
            }

            ViewBag.StatusID = new SelectList(db.Statuses, "StatusID", "Name");
            if (StatusID != 0)
            {
                return View(participantentries.Where(s => s.StatusID == StatusID).ToList());
            }
            else
            {
                return View(participantentries.Where(s => s.StatusID != 4).ToList());
            }
        }

        //
        // GET: /SearchRegistration/Detail/RegUID

        public ActionResult Detail(int Id = 0)
        {
            if (Id == 0)
            {
                ViewBag.Found = false;
                ViewBag.Message = "No Registration number";
                return View();
            }
            else
            {
                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(Id, "Admin Registration Opened", 0);

                ViewBag.Found = true;
                ViewBag.RegID = Id;
                return View();
            }
        }

        //
        // GET: /SearchRegistration/EditRegistration/RegUID

        public ActionResult EditRegistration(string RegUID)
        {
            if (RegUID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int FoundRegID = FoundEntry.RegUIDtoID(RegUID);

                if (FoundRegID != 0)
                {

                    var registrationentry = from m in db.RegEntries.Where(p => p.RegistrationID.Equals(FoundRegID))
                                            select m;
                    ViewBag.Message = "Edit Registration Detail";

                    return View(registrationentry.FirstOrDefault());
                }
                else
                {
                    ViewBag.Message = "Invalid Registration Key";
                    return View();
                }
            }
        }

        //
        // POST: /SearchRegistration/EditRegistration/RegUID

        [HttpPost]
        public ActionResult EditRegistration(string RegUID, RegistrationEntry registrationentry)
        {
            if (RegUID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int FoundRegID = FoundEntry.RegUIDtoID(RegUID);

                if (FoundRegID != 0 && ModelState.IsValid)
                {

                    registrationentry.RegistrationID = FoundRegID;

                    db.Entry(registrationentry).State = EntityState.Modified;
                    db.SaveChanges();

                    EventHistory NewEvent = new EventHistory();
                    NewEvent.AddHistory(registrationentry.RegistrationID, "Admin Registration Edited", 0);

                    return RedirectToAction("Detail", "SearchRegistration", new { RegUID = registrationentry.RegistrationUID });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        //
        // GET: /SearchRegistration/EditParticipant/5?RegUID

        public ActionResult EditParticipant(string RegUID, int id = 0)
        {
            if (RegUID == null || id == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int RegID = FoundEntry.RegUIDtoID(RegUID);

                var participantentry = from m in db.ParticipantEntries.Include(p => p.RegistrationEntries).
                    Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
                    Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p => p.RoomTypes).
                    Where(p => p.ParticipantID.Equals(id))
                                       select m;

                if (participantentry == null || RegID == 0 || participantentry.FirstOrDefault().RegistrationID != RegID)
                {
                    return RedirectToAction("Index", "Home");
                }
                
                ViewBag.RegUID = RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ParticipantID = participantentry.FirstOrDefault().ParticipantID;
                ViewBag.ServiceID = new SelectList(db.Services.Where(p => p.ServiceID.Equals(participantentry.FirstOrDefault().ServiceID)), "ServiceID", "Name", participantentry.FirstOrDefault().ServiceID);
                ViewBag.AgeRangeID = new SelectList(db.AgeRanges.Where(p => p.AgeRangeID.Equals(participantentry.FirstOrDefault().AgeRangeID)), "AgeRangeID", "Name", participantentry.FirstOrDefault().AgeRangeID);
                ViewBag.GenderID = new SelectList(db.Genders.Where(p => p.GenderID.Equals(participantentry.FirstOrDefault().GenderID)), "GenderID", "Name", participantentry.FirstOrDefault().GenderID);
                ViewBag.RegTypeID = new SelectList(db.RegTypes.Where(p => p.RegTypeID.Equals(participantentry.FirstOrDefault().RegTypeID)), "RegTypeID", "Name", participantentry.FirstOrDefault().RegTypeID);
                ViewBag.FellowshipID = new SelectList(db.Fellowships.Where(p => p.ServiceID.Equals(participantentry.FirstOrDefault().ServiceID)), "FellowshipID", "Name", participantentry.FirstOrDefault().FellowshipID);
                ViewBag.RoomTypeID = new SelectList(db.RoomTypes.Where(p => p.RegTypeID.Equals(participantentry.FirstOrDefault().RegTypeID)), "RoomTypeID", "Name", participantentry.FirstOrDefault().RoomTypeID);
                ViewBag.PartPrice = participantentry.FirstOrDefault().PartPrice;

                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(RegID, "Admin Participant Opened", participantentry.FirstOrDefault().ParticipantID);

                return View(participantentry.FirstOrDefault());
            }
        }

        //
        // POST: /SearchRegistration/EditParticipant/RegUID

        [HttpPost]
        public ActionResult EditParticipant(string RegUID, RegistrationEntry registrationentry)
        {
            if (RegUID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int FoundRegID = FoundEntry.RegUIDtoID(RegUID);

                if (FoundRegID != 0 && ModelState.IsValid)
                {

                    registrationentry.RegistrationID = FoundRegID;

                    db.Entry(registrationentry).State = EntityState.Modified;
                    db.SaveChanges();

                    EventHistory NewEvent = new EventHistory();
                    NewEvent.AddHistory(registrationentry.RegistrationID, "Admin Registration Edited", 0);

                    return RedirectToAction("Detail", "SearchRegistration", new { RegUID = registrationentry.RegistrationUID });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}
