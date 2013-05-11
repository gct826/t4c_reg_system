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
        // GET: /Participant/Index?RegID&isAdmin
        [ChildActionOnly]
        public ActionResult Index(int Id = 0 , bool isAdmin=false)
        {

            if (Id == 0)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Participant not found";
                return PartialView();
            }
            else
            {
                var PartEntry = from m in db.ParticipantEntries.Include(p => p.RegistrationEntries).
                                    Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
                                    Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p=> p.RoomTypes).
                                    Where(p => p.RegistrationID.Equals(Id))
                                select m;


                RegistrationEntry Registration = new RegistrationEntry();

                ViewBag.Found = true;   
                ViewBag.isAdmin = isAdmin;
                ViewBag.RegID = Id;
                ViewBag.RegUID = Registration.RegIDtoUID(Id);
                ViewBag.RegIsConfirm = Registration.RegIsConfirm(Id);
                ViewBag.RegIsComplete = Registration.RegIsComplete(Id);

                if (isAdmin)
                {
                    return PartialView(PartEntry.ToList());                    
                }
                else
                {
                    return PartialView(PartEntry.Where(p => !p.StatusID.Equals((int)4)).ToList());
                }
            }
        }

        //
        // GET: /Participant/ParticipantPartial
        [ChildActionOnly]
        public ActionResult ParticipantPartial(int id = 0)
        {
            if (id != 0)
            {
                var participantentry = from m in db.ParticipantEntries.Where(p => p.ParticipantID.Equals(id))
                        select m;

                if (participantentry.FirstOrDefault() != null)
                {
                    ViewBag.Found = true;
                    return PartialView(participantentry.FirstOrDefault());
                }
                else
                {
                    ViewBag.Found = false;
                    return PartialView();
                }
            }
            else
            {
                {
                    ViewBag.Found = false;
                    return PartialView();
                }
            }

        }

        //
        // GET: /Participant/Modify/RegUID
        public ActionResult Modify(string RegUID, bool isPage2 = false, bool? isAdmin = false, int Id = 0)
        {
            if (isAdmin == null)
            {
                isAdmin = false;
            }

            if (RegUID == null)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Invalid Registration Key";
                return View();
            }

            RegistrationEntry FoundEntry = new RegistrationEntry();
            int RegID = FoundEntry.RegUIDtoID(RegUID);

            if (RegID == 0)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Invalid Registration Key";
                return View();
            }

            if (RegID != 0 && Id == 0)
            {
                ViewBag.Found = true;
                ViewBag.isNew = true;
                ViewBag.isPage2 = isPage2;
                ViewBag.RegUID = (string)RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
                ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name");
                ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name");
                ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name");

                return View();
            }

            if (RegID != 0 && Id != 0)
            {
                 var participantentry = from m in db.ParticipantEntries.Include(p => p.RegistrationEntries).
                    Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
                    Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p => p.RoomTypes).
                    Where(p => p.ParticipantID.Equals(Id))
                                       select m;
                
                if (participantentry == null || RegID == 0 || participantentry.FirstOrDefault().RegistrationID != RegID)
                {
                    ViewBag.Found = false;
                    ViewBag.Message = "Participant not Found!";
                    return View();
                }

                ViewBag.Found = true;
                ViewBag.isNew = false;
                ViewBag.isPage2 = isPage2;
                ViewBag.isAdmin = isAdmin;
                ViewBag.RegUID = RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ParticipantID = participantentry.FirstOrDefault().ParticipantID;

                if (isPage2)
                {
                    ViewBag.ServiceID = new SelectList(db.Services.Where(p => p.ServiceID.Equals(participantentry.FirstOrDefault().ServiceID)), "ServiceID", "Name", participantentry.FirstOrDefault().ServiceID);
                    ViewBag.AgeRangeID = new SelectList(db.AgeRanges.Where(p => p.AgeRangeID.Equals(participantentry.FirstOrDefault().AgeRangeID)), "AgeRangeID", "Name", participantentry.FirstOrDefault().AgeRangeID);
                    ViewBag.GenderID = new SelectList(db.Genders.Where(p => p.GenderID.Equals(participantentry.FirstOrDefault().GenderID)), "GenderID", "Name", participantentry.FirstOrDefault().GenderID);
                    ViewBag.RegTypeID = new SelectList(db.RegTypes.Where(p => p.RegTypeID.Equals(participantentry.FirstOrDefault().RegTypeID)), "RegTypeID", "Name", participantentry.FirstOrDefault().RegTypeID);
                    ViewBag.FellowshipID = new SelectList(db.Fellowships.Where(p => p.ServiceID.Equals(participantentry.FirstOrDefault().ServiceID)), "FellowshipID", "Name", participantentry.FirstOrDefault().FellowshipID);
                    ViewBag.RoomTypeID = new SelectList(db.RoomTypes.Where(p => p.RegTypeID.Equals(participantentry.FirstOrDefault().RegTypeID)), "RoomTypeID", "Name", participantentry.FirstOrDefault().RoomTypeID);
                    ViewBag.PartPrice = participantentry.FirstOrDefault().PartPrice;
                }
                else
                {
                    ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", participantentry.FirstOrDefault().ServiceID);
                    ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", participantentry.FirstOrDefault().AgeRangeID);
                    ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", participantentry.FirstOrDefault().GenderID);
                    ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", participantentry.FirstOrDefault().RegTypeID);
                }
                
                return View(participantentry.FirstOrDefault());
            }
            
            ViewBag.Found = false;
            ViewBag.Message = "Catchall Error";
            return View();
        }

        //
        // POST: /Participant/Modify/RegUID
        [HttpPost]
        [MultiButton(MatchFormKey = "roomnote", MatchFormValue1 = "Next", MatchFormValue2 = "下页")]
        public ActionResult Modify(string RegUID, bool isPage2, bool? isAdmin, int Id, ParticipantEntry participantentry)
        {
            if (isAdmin == null)
            {
                isAdmin = false;
            }

            if (RegUID == null)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Invalid Registration Key";
                return View();
            }
            
            RegistrationEntry FoundEntry = new RegistrationEntry();
            int RegID = FoundEntry.RegUIDtoID(RegUID);

            if (RegID == 0)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Invalid Registration Key";
                return View();
            }

            if (RegID != 0 && Id == 0)
            {
                if (ModelState.IsValid)
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

                    return RedirectToAction("Modify", new { RegUID = RegUID, isPage2 = true, id = participantentry.ParticipantID });
                }

                ViewBag.Found = true;
                ViewBag.isNew = true;
                ViewBag.isPage2 = isPage2;
                ViewBag.RegUID = (string)RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
                ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name");
                ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name");
                ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name");

                return View();
            }

            if (RegID != 0 && Id != 0)
            {


                if (isPage2)
                {
                    if (ModelState.IsValid && RegID != 0)
                    {
                        participantentry.RegistrationID = RegID;
                        participantentry.ParticipantID = Id;
                        participantentry.StatusID = (int)2;

                        RegPrice FoundPrice = new RegPrice();
                        participantentry.PartPrice = FoundPrice.PriceReturn(participantentry.AgeRangeID, participantentry.RegTypeID);


                        db.Entry(participantentry).State = EntityState.Modified;
                        db.SaveChanges();


                        EventHistory NewEvent = new EventHistory();
                        NewEvent.AddHistory(RegID, "Participant Confirmed", participantentry.ParticipantID);

                        //return RedirectToAction("Modify", "Register", new { RegUID = RegUID });

                        if (participantentry.ServiceID == (int) 2 || participantentry.ServiceID == (int) 4)
                        {
                            return RedirectToAction("HeadsetRequest", "Participant",
                                             new {RegUID = RegUID, id = participantentry.ParticipantID});
                        }
                        else
                        {
                            return RedirectToAction("Modify", "Register", new { RegUID = RegUID });
                        }
                    }

                    ViewBag.ServiceID = new SelectList(db.Services.Where(p => p.ServiceID.Equals(participantentry.ServiceID)), "ServiceID", "Name", participantentry.ServiceID);
                    ViewBag.AgeRangeID = new SelectList(db.AgeRanges.Where(p => p.AgeRangeID.Equals(participantentry.AgeRangeID)), "AgeRangeID", "Name", participantentry.AgeRangeID);
                    ViewBag.GenderID = new SelectList(db.Genders.Where(p => p.GenderID.Equals(participantentry.GenderID)), "GenderID", "Name", participantentry.GenderID);
                    ViewBag.RegTypeID = new SelectList(db.RegTypes.Where(p => p.RegTypeID.Equals(participantentry.RegTypeID)), "RegTypeID", "Name", participantentry.RegTypeID);
                    ViewBag.FellowshipID = new SelectList(db.Fellowships.Where(p => p.ServiceID.Equals(participantentry.ServiceID)), "FellowshipID", "Name", participantentry.FellowshipID);
                    ViewBag.RoomTypeID = new SelectList(db.RoomTypes.Where(p => p.RegTypeID.Equals(participantentry.RegTypeID)), "RoomTypeID", "Name", participantentry.RoomTypeID);
                    ViewBag.PartPrice = participantentry.PartPrice;
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        participantentry.RegistrationID = RegID;
                        participantentry.ParticipantID = Id;
                        participantentry.StatusID = (int)1;

                        var partFellowshipList = from m in db.Fellowships.Where(p => p.FellowshipID.Equals(participantentry.FellowshipID))
                                                 select m;
                        Fellowship partFellowship = partFellowshipList.FirstOrDefault();

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

                        try
                        {
                            if (partRoomType.RegTypeID != participantentry.RegTypeID)
                            {
                                participantentry.RoomTypeID = participantentry.RegTypeID;
                            }
                        }
                        catch
                        {
                        }

                        RegPrice FoundPrice = new RegPrice();
                        participantentry.PartPrice = FoundPrice.PriceReturn(participantentry.AgeRangeID, participantentry.RegTypeID);

                        db.Entry(participantentry).State = EntityState.Modified;
                        db.SaveChanges();


                        EventHistory NewEvent = new EventHistory();
                        NewEvent.AddHistory(RegID, "Participant Edited", participantentry.ParticipantID);

                        return RedirectToAction("Modify", new { RegUID = RegUID, isPage2 = true, id = participantentry.ParticipantID });
                    }

                    ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", participantentry.ServiceID);
                    ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", participantentry.AgeRangeID);
                    ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", participantentry.GenderID);
                    ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", participantentry.RegTypeID);
                }

                ViewBag.Found = true;
                ViewBag.isNew = false;
                ViewBag.isPage2 = isPage2;
                ViewBag.isAdmin = isAdmin;
                ViewBag.RegUID = RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ParticipantID = participantentry.ParticipantID;

                return View(participantentry);
            }

            ViewBag.Found = false;
            ViewBag.Message = "Catchall Error";
            return View();
        }
        
        //
        // GET: /Participant/Remove/5?RegUID=xxx
        public ActionResult Remove(string RegUID, int id = 0)
        {
            if (RegUID == null || id == 0)
            {
                ViewBag.Found = false;
                ViewBag.PartMessage = "Missing Necessary Parameters";
                return View();

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
                    ViewBag.Found = false;
                    ViewBag.PartMessage = "Participant not found";
                    return View();
                }
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int RegID = FoundEntry.RegUIDtoID(RegUID);

                if (RegID == 0)
                {
                    ViewBag.Found = false;
                    ViewBag.PartMessage = "Registration not found";
                    return View();
                }

                ParticipantEntry FoundPartEntry = new ParticipantEntry();
                FoundPartEntry = participantentry.FirstOrDefault();

                if (FoundPartEntry == null)
                {
                    return HttpNotFound();
                }

                if (FoundPartEntry.RegistrationID != RegID)
                {
                    ViewBag.Found = false;
                    ViewBag.PartMessage = "Participant not found";
                    return View();
                }

                ViewBag.Found = true;
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

        //
        // GET: /Participant/Details/5
        [ChildActionOnly]
        public ActionResult Details(int id = 0)
        {
            var participant = from m in db.ParticipantEntries.Include(p => p.Services).Include(p => p.AgeRanges).
                    Include(p => p.Genders).Where(p => p.ParticipantID.Equals(id))
                                           select m;
            if (participant == null)
            {
                ViewBag.isFound = false;
                return PartialView();
            }

            ParticipantEntry foundParticipant = participant.FirstOrDefault();

            ViewBag.isFound = true;
            return PartialView(foundParticipant);
        }

        //
        // GET: /Participant/PartCount/5
        [ChildActionOnly]
        public ActionResult PartCount(int id = 0)
        {
            var partCount = db.ParticipantEntries.Where(a => a.StatusID != (int) 4).Count(a => a.RegistrationID.Equals(id));

            //db.Borrow.Where(x => x.UserID == 1).Select(x => x.BookId).Distinct().Count();

            ViewBag.partCount = partCount;

            return PartialView();


        }

        //
        // GET: /Participant/HeadsetRequest/5?RegUID=xxx
        public ActionResult HeadsetRequest(string RegUID, int id = 0)
        {
            if (RegUID == null || id == 0)
            {
                ViewBag.Found = false;
                ViewBag.PartMessage = "Missing Necessary Parameters";
                return View();

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
                    ViewBag.Found = false;
                    ViewBag.PartMessage = "Participant not found";
                    return View();
                }
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int RegID = FoundEntry.RegUIDtoID(RegUID);

                if (RegID == 0)
                {
                    ViewBag.Found = false;
                    ViewBag.PartMessage = "Registration not found";
                    return View();
                }

                ParticipantEntry FoundPartEntry = new ParticipantEntry();
                FoundPartEntry = participantentry.FirstOrDefault();

                if (FoundPartEntry == null)
                {
                    return HttpNotFound();
                }

                if (FoundPartEntry.RegistrationID != RegID)
                {
                    ViewBag.Found = false;
                    ViewBag.PartMessage = "Participant not found";
                    return View();
                }

                ViewBag.Found = true;
                ViewBag.RegUID = RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ParticipantID = FoundPartEntry.ParticipantID;

                return View(FoundPartEntry);
            }
        }
    
        //
        // POST: /Participant/HeadsetRequest/5?RegUID=xxx
        [HttpPost]
        [MultiButton(MatchFormKey = "headsetrequest", MatchFormValue1 = "Yes", MatchFormValue2 = "需要")]
        public ActionResult HeadsetRequest(ParticipantEntry participantEntry)
        {
            var foundEntry = from m in db.ParticipantEntries
                                 .Where(p => p.RegistrationID.Equals(participantEntry.RegistrationID))
                                 .Where(p => p.FirstName.Equals(participantEntry.FirstName))
                                   select m;

            var regUID = new RegistrationEntry().RegIDtoUID(foundEntry.FirstOrDefault().RegistrationID);
                
            var headset = new Headset();

            headset.ParticipantID = foundEntry.FirstOrDefault().ParticipantID;

            var foundHeadset = from m in db.Headsets
                                 .Where(p => p.ParticipantID.Equals(headset.ParticipantID))
                             select m;

            try
            {
                if (foundHeadset.FirstOrDefault().ParticipantID == headset.ParticipantID)
                {
                    return RedirectToAction("Modify", "Register", new { RegUID = regUID });
                }

            }
            catch (Exception)
            {
                if (ModelState.IsValid)
                {
                    db.Headsets.Add(headset);
                    db.SaveChanges();

                    EventHistory NewEvent = new EventHistory();
                    NewEvent.AddHistory(foundEntry.FirstOrDefault().RegistrationID, "Headset Requested", foundEntry.FirstOrDefault().ParticipantID);

                    return RedirectToAction("Modify", "Register", new { RegUID = regUID });
                }
            }

            return View(participantEntry); 
   
        }
    
        //
        // GET: /Participant/RoomNote/5
        [ChildActionOnly]
        public ActionResult RoomNote(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.Found = false;
                ViewBag.PartMessage = "Missing Necessary Parameters";
                return PartialView();

            }
            else
            {                
                var roomNote = from m in db.RoomNotes.Where(p => p.PartID.Equals(id))
                               select m;

                RoomNote foundRoomNote = new RoomNote();
                foundRoomNote = roomNote.FirstOrDefault();

                if (foundRoomNote == null)
                {
                    ViewBag.Found = false;
                    ViewBag.RoomNote = null;

                    RoomNote returnRoomNote = new RoomNote();
                    returnRoomNote.Note = null;
                    returnRoomNote.PartID = id;

                    return PartialView(returnRoomNote);
                }

                ViewBag.Found = true;
                ViewBag.RoomNote = foundRoomNote.Note;

                return PartialView(foundRoomNote);
            } 
        }

        //
        // POST: /Participant/RoomNote/
        [HttpPost]
        [MultiButton(MatchFormKey = "roomnote", MatchFormValue1 = "Add/Edit", MatchFormValue2 = "加/改")]
        public ActionResult RoomNote(string RegUID, bool isPage2, bool? isAdmin, int Id, ParticipantEntry participantentry)
        {
            if (isAdmin == null)
            {
                isAdmin = false;
            }

            if (RegUID == null)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Invalid Registration Key";
                return View();
            }

            RegistrationEntry FoundEntry = new RegistrationEntry();
            int RegID = FoundEntry.RegUIDtoID(RegUID);

            if (RegID == 0)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Invalid Registration Key";
                return View();
            }

            if (RegID != 0 && Id == 0)
            {

                ViewBag.Found = false;
                ViewBag.Message = "Invalid Participant ID";
                return View();
            }

            if (RegID != 0 && Id != 0)
            {
                if (isPage2)
                {
                    if (ModelState.IsValid && RegID != 0)
                    {
                        participantentry.RegistrationID = RegID;
                        participantentry.ParticipantID = Id;
                        participantentry.StatusID = (int)1;

                        RegPrice FoundPrice = new RegPrice();
                        participantentry.PartPrice = FoundPrice.PriceReturn(participantentry.AgeRangeID, participantentry.RegTypeID);

                        db.Entry(participantentry).State = EntityState.Modified;
                        db.SaveChanges();

                        EventHistory NewEvent = new EventHistory();
                        NewEvent.AddHistory(RegID, "RoomNote Add Page", participantentry.ParticipantID);

                        return RedirectToAction("RoomNoteAdd", new { RegUID = RegUID, isPage2 = true, id = participantentry.ParticipantID });
                    }

                    ViewBag.ServiceID = new SelectList(db.Services.Where(p => p.ServiceID.Equals(participantentry.ServiceID)), "ServiceID", "Name", participantentry.ServiceID);
                    ViewBag.AgeRangeID = new SelectList(db.AgeRanges.Where(p => p.AgeRangeID.Equals(participantentry.AgeRangeID)), "AgeRangeID", "Name", participantentry.AgeRangeID);
                    ViewBag.GenderID = new SelectList(db.Genders.Where(p => p.GenderID.Equals(participantentry.GenderID)), "GenderID", "Name", participantentry.GenderID);
                    ViewBag.RegTypeID = new SelectList(db.RegTypes.Where(p => p.RegTypeID.Equals(participantentry.RegTypeID)), "RegTypeID", "Name", participantentry.RegTypeID);
                    ViewBag.FellowshipID = new SelectList(db.Fellowships.Where(p => p.ServiceID.Equals(participantentry.ServiceID)), "FellowshipID", "Name", participantentry.FellowshipID);
                    ViewBag.RoomTypeID = new SelectList(db.RoomTypes.Where(p => p.RegTypeID.Equals(participantentry.RegTypeID)), "RoomTypeID", "Name", participantentry.RoomTypeID);
                    ViewBag.PartPrice = participantentry.PartPrice;
                }
                else
                {
                    ViewBag.Found = false;
                    ViewBag.Message = "Invalid Participant ID";
                    return View();
                }

                

                ViewBag.Found = false;
                ViewBag.Message = "Invalid Participant ID";
                return View();

            }

            ViewBag.Found = false;
            ViewBag.Message = "Catchall Error";
            return View();
        }

        //
        // GET: /Participant/RoomNoteAdd/UID
        public ActionResult RoomNoteAdd(string RegUID, bool isPage2 = false, bool? isAdmin = false, int Id = 0)
        {
            if (isAdmin == null)
            {
                isAdmin = false;
            }

            if (RegUID == null)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Invalid Registration Key";
                return View();
            }

            RegistrationEntry FoundEntry = new RegistrationEntry();
            int RegID = FoundEntry.RegUIDtoID(RegUID);

            if (RegID == 0)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Invalid Registration Key";
                return View();
            }

            if (RegID != 0 && Id == 0)
            {
                ViewBag.Found = true;
                ViewBag.isNew = true;
                ViewBag.isPage2 = isPage2;
                ViewBag.RegUID = (string)RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
                ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name");
                ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name");
                ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name");

                return View();
            }

            if (RegID != 0 && Id != 0)
            {
                var participantentry = from m in db.ParticipantEntries.Include(p => p.RegistrationEntries).
                   Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
                   Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p => p.RoomTypes).
                   Where(p => p.ParticipantID.Equals(Id))
                                       select m;

                var roomNote = from m in db.RoomNotes.Where(p => p.PartID.Equals(Id))
                               select m;


                if (roomNote == null || RegID == 0)
                {
                    ViewBag.Found = false;
                    ViewBag.isNew = false;
                    ViewBag.isPage2 = isPage2;
                    ViewBag.isAdmin = isAdmin;
                    ViewBag.RegUID = RegUID;
                    ViewBag.RegistrationID = RegID;
                    ViewBag.ParticipantID = Id;
                    
                    RoomNote returnRoomNote = new RoomNote();
                    returnRoomNote.Note = null;
                    returnRoomNote.PartID = Id;

                    return View(returnRoomNote);
                }

                ViewBag.Found = true;
                ViewBag.isNew = false;
                ViewBag.isPage2 = isPage2;
                ViewBag.isAdmin = isAdmin;
                ViewBag.RegUID = RegUID;
                ViewBag.RegistrationID = RegID;
                ViewBag.ParticipantID = Id;

                return View(roomNote.FirstOrDefault());
            }

            ViewBag.Found = false;
            ViewBag.Message = "Catchall Error";
            return View();
        }

        //
        // POST: /Participant/RoomNoteAdd/UID
        [HttpPost]
        public ActionResult RoomNoteAdd(string RegUID, bool isPage2, bool? isAdmin, int ID, RoomNote roomNote)
        {
            if (isAdmin == null)
            {
                isAdmin = false;
            }

            if (RegUID == null)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Invalid Registration Key";
                return View(roomNote);
            }

            RegistrationEntry FoundEntry = new RegistrationEntry();
            int RegID = FoundEntry.RegUIDtoID(RegUID);

            if (RegID == 0)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Invalid Registration Key";
                return View(roomNote);
            }

            var findRoomNote = from m in db.RoomNotes.Where(p => p.PartID.Equals(ID))
                select m;
         
            if (findRoomNote.FirstOrDefault() == null)
            {

                roomNote.PartID = ID;

                db.RoomNotes.Add(roomNote);
                db.SaveChanges();

                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(RegID, "RoomNote Add", roomNote.RoomNoteID);

                return RedirectToAction("Modify", new { RegUID = RegUID, isPage2 = isPage2, id = ID });
            }
            else
            {
                RoomNote newRoomNote = findRoomNote.FirstOrDefault();

                newRoomNote.Note = roomNote.Note;

                db.Entry(newRoomNote).State = EntityState.Modified;
                db.SaveChanges();

                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(RegID, "RoomNote Add", newRoomNote.RoomNoteID);
                    
                return RedirectToAction("Modify", new { RegUID = RegUID, isPage2 = isPage2, id = ID });
            }

            ViewBag.Found = false;
            ViewBag.Message = "Catchall Error";
            return View(roomNote);
        }

    }

}