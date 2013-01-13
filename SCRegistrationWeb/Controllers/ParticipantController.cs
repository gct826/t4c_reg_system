using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Text;
using SCRegistrationWeb.Models;

namespace SCRegistrationWeb.Controllers
{
    public class ParticipantController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /Participant/Index?RegUID=xxx
        [ChildActionOnly]
        public ActionResult Index(string RegUID)
        {

            if (RegUID == null)
            {
                ViewBag.PartMessage = "Participant not found";
                return PartialView();
            }
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int RegID = FoundEntry.RegUIDtoID(RegUID);

                var PartEntry = from m in db.ParticipantEntries.Include(p => p.RegistrationEntries).
                                    Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
                                    Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p=> p.RoomTypes).
                                    Where(p => p.RegistrationID.Equals(RegID)).Where(p => !p.StatusID.Equals((int)4))
                                select m;
                
                ViewBag.RegUID = RegUID;
                ViewBag.TotalPrice = FoundEntry.RegTotalPrice(RegUID);
                ViewBag.RegIsConfirm = FoundEntry.RegIsConfirm(RegUID);
                ViewBag.RegIsComplete = FoundEntry.RegIsComplete(RegUID);

                return PartialView(PartEntry.ToList());
            }
        }

        //
        // GET: /Participant/Create?RegUID=xxx
        public ActionResult Create(string RegUID)
        {
            if (RegUID == null)
            {
                ViewBag.PartMessage = "Participant not found";
                return RedirectToAction("Index","Home");

            }
            else
            {

                RegistrationEntry FoundEntry = new RegistrationEntry();
                int RegID = FoundEntry.RegUIDtoID(RegUID);

                if (RegID == 0)
                {
                    ViewBag.PartMessage = "Participant not found";
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.RegUID = (string)RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
                ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name");
                ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name");
                ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name");
                
                return View();
            }
        }

        //
        // POST: /Participant/Create?RegUID=xxx
        [HttpPost]
        public ActionResult Create(string RegUID, ParticipantEntry participantentry)
        {
            int RegID = (int)0;

            if (RegUID == null)
            {
                ViewBag.PartMessage = "Participant not found";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                RegID = FoundEntry.RegUIDtoID(RegUID); 
            }

            if (ModelState.IsValid && RegID != 0)
            {
           
                participantentry.RegistrationID = RegID;
                participantentry.StatusID = (int)1;
                participantentry.FellowshipID = participantentry.ServiceID;
                participantentry.RoomTypeID = participantentry.RegTypeID;

                RegPrice FoundPrice = new RegPrice();
                participantentry.PartPrice = FoundPrice.PriceReturn(participantentry.AgeRangeID, participantentry.RegTypeID);

                db.ParticipantEntries.Add(participantentry);
                db.SaveChanges();
 
                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(RegID, "New Participant Created", participantentry.ParticipantID);
 

                return RedirectToAction("Page2", new { RegUID = RegUID, id = participantentry.ParticipantID });
            }

            ViewBag.RegUID = (string)RegUID;
            ViewBag.RegistrationID = RegID;
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", participantentry.ServiceID);
            ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", participantentry.AgeRangeID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", participantentry.GenderID);
            ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", participantentry.RegTypeID);
            return View(participantentry);
        }

        //
        // GET: /Participant/Edit/5?RegUID=xxx
        public ActionResult Edit(string RegUID, int id = 0)
        {
            if (RegUID == null || id == 0)
            {
                ViewBag.PartMessage = "Participant not found";
                return RedirectToAction("Index", "Home");

            }
            else
            {
                var participantentry = from m in db.ParticipantEntries.Include(p => p.RegistrationEntries).
                    Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
                    Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p=> p.RoomTypes).
                    Where(p => p.ParticipantID.Equals(id))
                                select m;

                if (participantentry == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int RegID = FoundEntry.RegUIDtoID(RegUID);

                if (RegID == 0)
                {
                    return RedirectToAction("Index", "Home");
                }

                ParticipantEntry FoundPartEntry = new ParticipantEntry();
                FoundPartEntry = participantentry.FirstOrDefault();

                if (FoundPartEntry == null)
                {
                    return HttpNotFound();
                }

                if (FoundPartEntry.RegistrationID != RegID)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.RegUID = RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ParticipantID = FoundPartEntry.ParticipantID;
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", FoundPartEntry.ServiceID);
                ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", FoundPartEntry.AgeRangeID);
                ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", FoundPartEntry.GenderID);
                ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", FoundPartEntry.RegTypeID);

                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(RegID, "Participant Opened", FoundPartEntry.ParticipantID);

                return View(FoundPartEntry);
            }
        }

        //
        // POST: /Participant/Edit/5
        [HttpPost]
        public ActionResult Edit(string RegUID, int id, ParticipantEntry participantentry)
        {

            int RegID = (int)0;

            if (RegUID == null)
            {
                ViewBag.PartMessage = "Participant not found";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                RegID = FoundEntry.RegUIDtoID(RegUID);
            }

            if (ModelState.IsValid && RegID != 0)
            {
                participantentry.RegistrationID = RegID;
                participantentry.ParticipantID = id;
                participantentry.StatusID = (int)1;

                var partFellowshipList = from m in db.Fellowships.Where(p => p.FellowshipID.Equals(participantentry.FellowshipID))
                                     select m;
                Fellowship partFellowship = partFellowshipList.First();

                try
                {
                    if (partFellowship.ServiceID != participantentry.ServiceID)
                    {
                        participantentry.FellowshipID = participantentry.ServiceID;
                    }
                }
                catch
                {
                }

                var partRoomTypeList = from m in db.RoomTypes.Where(p => p.RoomTypeID.Equals(participantentry.RoomTypeID))
                                         select m;
                RoomType partRoomType = partRoomTypeList.FirstOrDefault();

                if (partRoomType.RegTypeID != participantentry.RegTypeID)
                {
                    participantentry.RoomTypeID = participantentry.RegTypeID;
                }

                RegPrice FoundPrice = new RegPrice();
                participantentry.PartPrice = FoundPrice.PriceReturn(participantentry.AgeRangeID, participantentry.RegTypeID);

                db.Entry(participantentry).State = EntityState.Modified;
                db.SaveChanges();


                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(RegID, "Participant Edited", participantentry.ParticipantID);

                return RedirectToAction("Page2", new { RegUID = RegUID, id = participantentry.ParticipantID });
            }

            ViewBag.RegUID = RegUID;
            ViewBag.RegistrationID = RegID;
            ViewBag.ParticipantID = participantentry.ParticipantID;
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", participantentry.ServiceID);
            ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", participantentry.AgeRangeID);
            ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", participantentry.GenderID);
            ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", participantentry.RegTypeID);

            return View(participantentry);
        }

        //
        // GET: /Participant/Page2/5?RegUID=xxx
        public ActionResult Page2(string RegUID, int id = 0)
        {
            if (RegUID == null || id == 0)
            {
                ViewBag.PartMessage = "Participant not found";
                return RedirectToAction("Index", "Home");

            }
            else
            {
                var participantentry = from m in db.ParticipantEntries.Include(p => p.RegistrationEntries).
                    Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
                    Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p => p.RoomTypes).
                    Where(p => p.ParticipantID.Equals(id))
                                       select m;

                if (participantentry == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int RegID = FoundEntry.RegUIDtoID(RegUID);

                if (RegID == 0)
                {
                    return RedirectToAction("Index", "Home");
                }

                ParticipantEntry FoundPartEntry = new ParticipantEntry();
                FoundPartEntry = participantentry.FirstOrDefault();

                if (FoundPartEntry == null)
                {
                    return HttpNotFound();
                }

                if (FoundPartEntry.RegistrationID != RegID)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.RegUID = RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ParticipantID = FoundPartEntry.ParticipantID;
                ViewBag.ServiceID = new SelectList(db.Services.Where(p => p.ServiceID.Equals(FoundPartEntry.ServiceID)), "ServiceID", "Name", FoundPartEntry.ServiceID);
                ViewBag.AgeRangeID = new SelectList(db.AgeRanges.Where(p => p.AgeRangeID.Equals(FoundPartEntry.AgeRangeID)), "AgeRangeID", "Name", FoundPartEntry.AgeRangeID);
                ViewBag.GenderID = new SelectList(db.Genders.Where(p => p.GenderID.Equals(FoundPartEntry.GenderID)), "GenderID", "Name", FoundPartEntry.GenderID);
                ViewBag.RegTypeID = new SelectList(db.RegTypes.Where(p => p.RegTypeID.Equals(FoundPartEntry.RegTypeID)), "RegTypeID", "Name", FoundPartEntry.RegTypeID);
                ViewBag.FellowshipID = new SelectList(db.Fellowships.Where(p => p.ServiceID.Equals(FoundPartEntry.ServiceID)), "FellowshipID", "Name", FoundPartEntry.FellowshipID);
                ViewBag.RoomTypeID = new SelectList(db.RoomTypes.Where(p => p.RegTypeID.Equals(FoundPartEntry.RegTypeID)), "RoomTypeID", "Name", FoundPartEntry.RoomTypeID);
                ViewBag.PartPrice = FoundPartEntry.PartPrice;

                return View(FoundPartEntry);
            }
        }

        //
        // POST: /Participant/Page2/5?RegUID=xxx
        [HttpPost]
        public ActionResult Page2(string RegUID, int id, ParticipantEntry participantentry)
        {
            int RegID = (int)0;

            if (RegUID == null)
            {
                ViewBag.PartMessage = "Participant not found";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                RegID = FoundEntry.RegUIDtoID(RegUID);
            }

            if (ModelState.IsValid && RegID != 0)
            {
                participantentry.RegistrationID = RegID;
                participantentry.ParticipantID = id;
                participantentry.StatusID = (int)2;

                RegPrice FoundPrice = new RegPrice();
                participantentry.PartPrice = FoundPrice.PriceReturn(participantentry.AgeRangeID, participantentry.RegTypeID);


                db.Entry(participantentry).State = EntityState.Modified;
                db.SaveChanges();


                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(RegID, "Participant Confirmed", participantentry.ParticipantID);

                return RedirectToAction("Modify", "Register", new { RegUID = RegUID });
            }

            ViewBag.RegUID = RegUID;
            ViewBag.RegistrationID = RegID;
            ViewBag.ParticipantID = participantentry.ParticipantID;
            ViewBag.ServiceID = new SelectList(db.Services.Where(p => p.ServiceID.Equals(participantentry.ServiceID)), "ServiceID", "Name", participantentry.ServiceID);
            ViewBag.AgeRangeID = new SelectList(db.AgeRanges.Where(p => p.AgeRangeID.Equals(participantentry.AgeRangeID)), "AgeRangeID", "Name", participantentry.AgeRangeID);
            ViewBag.GenderID = new SelectList(db.Genders.Where(p => p.GenderID.Equals(participantentry.GenderID)), "GenderID", "Name", participantentry.GenderID);
            ViewBag.RegTypeID = new SelectList(db.RegTypes.Where(p => p.RegTypeID.Equals(participantentry.RegTypeID)), "RegTypeID", "Name", participantentry.RegTypeID);
            ViewBag.FellowshipID = new SelectList(db.Fellowships.Where(p => p.ServiceID.Equals(participantentry.ServiceID)), "FellowshipID", "Name", participantentry.FellowshipID);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes.Where(p => p.RegTypeID.Equals(participantentry.RegTypeID)), "RoomTypeID", "Name", participantentry.RoomTypeID);
            ViewBag.PartPrice = participantentry.PartPrice;
            
            return View(participantentry);
        }

        //
        // GET: /Participant/Remove/5?RegUID=xxx
        public ActionResult Remove(string RegUID, int id = 0)
        {
            if (RegUID == null || id == 0)
            {
                ViewBag.PartMessage = "Participant not found";
                return RedirectToAction("Index", "Home");

            }
            else
            {
                var participantentry = from m in db.ParticipantEntries.Include(p => p.RegistrationEntries).
                    Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
                    Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p => p.RoomTypes).
                    Where(p => p.ParticipantID.Equals(id))
                                       select m;

                if (participantentry == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int RegID = FoundEntry.RegUIDtoID(RegUID);

                if (RegID == 0)
                {
                    return RedirectToAction("Index", "Home");
                }

                ParticipantEntry FoundPartEntry = new ParticipantEntry();
                FoundPartEntry = participantentry.FirstOrDefault();

                if (FoundPartEntry == null)
                {
                    return HttpNotFound();
                }

                if (FoundPartEntry.RegistrationID != RegID)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.RegUID = RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ParticipantID = FoundPartEntry.ParticipantID;
                ViewBag.ServiceID = new SelectList(db.Services.Where(p => p.ServiceID.Equals(FoundPartEntry.ServiceID)), "ServiceID", "Name", FoundPartEntry.ServiceID);
                ViewBag.AgeRangeID = new SelectList(db.AgeRanges.Where(p => p.AgeRangeID.Equals(FoundPartEntry.AgeRangeID)), "AgeRangeID", "Name", FoundPartEntry.AgeRangeID);
                ViewBag.GenderID = new SelectList(db.Genders.Where(p => p.GenderID.Equals(FoundPartEntry.GenderID)), "GenderID", "Name", FoundPartEntry.GenderID);
                ViewBag.RegTypeID = new SelectList(db.RegTypes.Where(p => p.RegTypeID.Equals(FoundPartEntry.RegTypeID)), "RegTypeID", "Name", FoundPartEntry.RegTypeID);
                ViewBag.FellowshipID = new SelectList(db.Fellowships.Where(p => p.ServiceID.Equals(FoundPartEntry.ServiceID)), "FellowshipID", "Name", FoundPartEntry.FellowshipID);
                ViewBag.RoomTypeID = new SelectList(db.RoomTypes.Where(p => p.RegTypeID.Equals(FoundPartEntry.RegTypeID)), "RoomTypeID", "Name", FoundPartEntry.RoomTypeID);
                ViewBag.PartPrice = FoundPartEntry.PartPrice;

                return View(FoundPartEntry);
            }
        }

        //
        // POST: /Participant/Remove/5?RegUID=xxx
        [HttpPost]
        public ActionResult Remove(string RegUID, int id, ParticipantEntry participantentry)
        {
            int RegID = (int)0;

            if (RegUID == null)
            {
                ViewBag.PartMessage = "Participant not found";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                RegID = FoundEntry.RegUIDtoID(RegUID);
            }

            if (ModelState.IsValid && RegID != 0)
            {
                participantentry.RegistrationID = RegID;
                participantentry.ParticipantID = id;
                participantentry.StatusID = (int)4;

                participantentry.PartPrice = (decimal)0;


                db.Entry(participantentry).State = EntityState.Modified;
                db.SaveChanges();


                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(RegID, "Participant Deleted", participantentry.ParticipantID);

                return RedirectToAction("Modify", "Register", new { RegUID = RegUID });
            }

            ViewBag.RegUID = RegUID;
            ViewBag.RegistrationID = RegID;
            ViewBag.ParticipantID = participantentry.ParticipantID;
            ViewBag.ServiceID = new SelectList(db.Services.Where(p => p.ServiceID.Equals(participantentry.ServiceID)), "ServiceID", "Name", participantentry.ServiceID);
            ViewBag.AgeRangeID = new SelectList(db.AgeRanges.Where(p => p.AgeRangeID.Equals(participantentry.AgeRangeID)), "AgeRangeID", "Name", participantentry.AgeRangeID);
            ViewBag.GenderID = new SelectList(db.Genders.Where(p => p.GenderID.Equals(participantentry.GenderID)), "GenderID", "Name", participantentry.GenderID);
            ViewBag.RegTypeID = new SelectList(db.RegTypes.Where(p => p.RegTypeID.Equals(participantentry.RegTypeID)), "RegTypeID", "Name", participantentry.RegTypeID);
            ViewBag.FellowshipID = new SelectList(db.Fellowships.Where(p => p.ServiceID.Equals(participantentry.ServiceID)), "FellowshipID", "Name", participantentry.FellowshipID);
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes.Where(p => p.RegTypeID.Equals(participantentry.RegTypeID)), "RoomTypeID", "Name", participantentry.RoomTypeID);
            ViewBag.PartPrice = participantentry.PartPrice;

            return View(participantentry);
        }
    
    }
}