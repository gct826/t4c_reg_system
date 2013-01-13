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

        public ActionResult Detail(string RegUID)
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
                    ViewBag.Message = "Registration Detail";

                    EventHistory NewEvent = new EventHistory();
                    NewEvent.AddHistory(FoundRegID, "Admin Registration Opened", 0);

                    return View(registrationentry.ToList());
                }
                else
                {
                    ViewBag.Message = "Invalid Registration Key";
                    return View();
                }
            }

        }


    }
}
