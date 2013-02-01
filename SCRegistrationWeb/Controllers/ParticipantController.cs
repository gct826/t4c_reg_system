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
                                    Where(p => p.RegistrationID.Equals(Id)).Where(p => !p.StatusID.Equals((int)4))
                                select m;

                RegistrationEntry Registration = new RegistrationEntry();

                ViewBag.Found = true;   
                ViewBag.isAdmin = isAdmin;
                ViewBag.RegUID = Registration.RegIDtoUID(Id);
                ViewBag.TotalPrice = Registration.RegTotalPrice(Id);
                ViewBag.RegIsConfirm = Registration.RegIsConfirm(Id);
                ViewBag.RegIsComplete = Registration.RegIsComplete(Id);

                return PartialView(PartEntry.ToList());
            }
        }

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


        // POST: /Participant/Modify/RegUID
        [HttpPost]
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

                        return RedirectToAction("Modify", "Register", new { RegUID = RegUID });
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
        // GET: /Participant/Create?RegUID=xxx

        //public ActionResult Create(string RegUID)
        //{
        //    if (RegUID == null)
        //    {
        //        ViewBag.PartMessage = "Participant not found";
        //        return RedirectToAction("Index","Home");

        //    }
        //    else
        //    {

        //        RegistrationEntry FoundEntry = new RegistrationEntry();
        //        int RegID = FoundEntry.RegUIDtoID(RegUID);

        //        if (RegID == 0)
        //        {
        //            ViewBag.PartMessage = "Participant not found";
        //            return RedirectToAction("Index", "Home");
        //        }

        //        ViewBag.RegUID = (string)RegUID;
        //        ViewBag.RegistrationID = RegID;
        //        ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
        //        ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name");
        //        ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name");
        //        ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name");
                
        //        return View();
        //    }
        //}

        ////
        //// POST: /Participant/Create?RegUID=xxx
        //[HttpPost]
        //public ActionResult Create(string RegUID, ParticipantEntry participantentry)
        //{
        //    int RegID = (int)0;

        //    if (RegUID == null)
        //    {
        //        ViewBag.PartMessage = "Participant not found";
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        RegistrationEntry FoundEntry = new RegistrationEntry();
        //        RegID = FoundEntry.RegUIDtoID(RegUID); 
        //    }

        //    if (ModelState.IsValid && RegID != 0)
        //    {
           
        //        participantentry.RegistrationID = RegID;
        //        participantentry.StatusID = (int)1;
        //        participantentry.FellowshipID = participantentry.ServiceID;
        //        participantentry.RoomTypeID = participantentry.RegTypeID;

        //        RegPrice FoundPrice = new RegPrice();
        //        participantentry.PartPrice = FoundPrice.PriceReturn(participantentry.AgeRangeID, participantentry.RegTypeID);

        //        db.ParticipantEntries.Add(participantentry);
        //        db.SaveChanges();
 
        //        EventHistory NewEvent = new EventHistory();
        //        NewEvent.AddHistory(RegID, "New Participant Created", participantentry.ParticipantID);
 

        //        return RedirectToAction("Page2", new { RegUID = RegUID, id = participantentry.ParticipantID });
        //    }

        //    ViewBag.RegUID = (string)RegUID;
        //    ViewBag.RegistrationID = RegID;
        //    ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", participantentry.ServiceID);
        //    ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", participantentry.AgeRangeID);
        //    ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", participantentry.GenderID);
        //    ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", participantentry.RegTypeID);
        //    return View(participantentry);
        //}

        ////
        //// GET: /Participant/Edit/5?RegUID=xxx
        //public ActionResult Edit(string RegUID, int id = 0)
        //{
        //    if (RegUID == null || id == 0)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    else
        //    {
        //        RegistrationEntry FoundEntry = new RegistrationEntry();
        //        int RegID = FoundEntry.RegUIDtoID(RegUID);

        //        var participantentry = from m in db.ParticipantEntries.Include(p => p.RegistrationEntries).
        //            Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
        //            Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p => p.RoomTypes).
        //            Where(p => p.ParticipantID.Equals(id))
        //                               select m;

        //        if (participantentry == null || RegID == 0 || participantentry.FirstOrDefault().RegistrationID != RegID)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }

        //        ViewBag.RegUID = RegUID;
        //        ViewBag.RegistrationID = RegID;
        //        ViewBag.ParticipantID = participantentry.FirstOrDefault().ParticipantID;
        //        ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", participantentry.FirstOrDefault().ServiceID);
        //        ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", participantentry.FirstOrDefault().AgeRangeID);
        //        ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", participantentry.FirstOrDefault().GenderID);
        //        ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", participantentry.FirstOrDefault().RegTypeID);

        //        EventHistory NewEvent = new EventHistory();
        //        NewEvent.AddHistory(RegID, "Participant Opened", participantentry.FirstOrDefault().ParticipantID);

        //        return View(participantentry.FirstOrDefault());
        //    }
        //}

        ////
        //// POST: /Participant/Edit/5
        //[HttpPost]
        //public ActionResult Edit(string RegUID, int id, ParticipantEntry participantentry)
        //{
        //    if (RegUID == null || id == 0)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    RegistrationEntry FoundEntry = new RegistrationEntry();
        //    int RegID = FoundEntry.RegUIDtoID(RegUID);

        //    if (ModelState.IsValid && RegID != 0)
        //    {
        //        participantentry.RegistrationID = RegID;
        //        participantentry.ParticipantID = id;
        //        participantentry.StatusID = (int)1;

        //        var partFellowshipList = from m in db.Fellowships.Where(p => p.FellowshipID.Equals(participantentry.FellowshipID))
        //                             select m;
        //        Fellowship partFellowship = partFellowshipList.First();

        //        try
        //        {
        //            if (partFellowship.ServiceID != participantentry.ServiceID)
        //            {
        //                participantentry.FellowshipID = participantentry.ServiceID;
        //            }
        //        }
        //        catch
        //        {
        //        }

        //        var partRoomTypeList = from m in db.RoomTypes.Where(p => p.RoomTypeID.Equals(participantentry.RoomTypeID))
        //                                 select m;
        //        RoomType partRoomType = partRoomTypeList.FirstOrDefault();

        //        if (partRoomType.RegTypeID != participantentry.RegTypeID)
        //        {
        //            participantentry.RoomTypeID = participantentry.RegTypeID;
        //        }

        //        RegPrice FoundPrice = new RegPrice();
        //        participantentry.PartPrice = FoundPrice.PriceReturn(participantentry.AgeRangeID, participantentry.RegTypeID);

        //        db.Entry(participantentry).State = EntityState.Modified;
        //        db.SaveChanges();


        //        EventHistory NewEvent = new EventHistory();
        //        NewEvent.AddHistory(RegID, "Participant Edited", participantentry.ParticipantID);

        //        return RedirectToAction("Page2", new { RegUID = RegUID, id = participantentry.ParticipantID });
        //    }

        //    ViewBag.RegUID = RegUID;
        //    ViewBag.RegistrationID = RegID;
        //    ViewBag.ParticipantID = participantentry.ParticipantID;
        //    ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", participantentry.ServiceID);
        //    ViewBag.AgeRangeID = new SelectList(db.AgeRanges, "AgeRangeID", "Name", participantentry.AgeRangeID);
        //    ViewBag.GenderID = new SelectList(db.Genders, "GenderID", "Name", participantentry.GenderID);
        //    ViewBag.RegTypeID = new SelectList(db.RegTypes, "RegTypeID", "Name", participantentry.RegTypeID);

        //    return View(participantentry);
        //}

        ////
        //// GET: /Participant/Page2/5?RegUID=xxx
        //public ActionResult Page2(string RegUID, int id = 0)
        //{
        //    if (RegUID == null || id == 0)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    else
        //    {
        //        RegistrationEntry FoundEntry = new RegistrationEntry();
        //        int RegID = FoundEntry.RegUIDtoID(RegUID);

        //        var participantentry = from m in db.ParticipantEntries.Include(p => p.RegistrationEntries).
        //            Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
        //            Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p => p.RoomTypes).
        //            Where(p => p.ParticipantID.Equals(id))
        //                               select m;

        //        if (participantentry == null || RegID == 0 || participantentry.FirstOrDefault().RegistrationID != RegID)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }

        //        ViewBag.RegUID = RegUID;
        //        ViewBag.RegistrationID = RegID;
        //        ViewBag.ParticipantID = participantentry.FirstOrDefault().ParticipantID;
        //        ViewBag.ServiceID = new SelectList(db.Services.Where(p => p.ServiceID.Equals(participantentry.FirstOrDefault().ServiceID)), "ServiceID", "Name", participantentry.FirstOrDefault().ServiceID);
        //        ViewBag.AgeRangeID = new SelectList(db.AgeRanges.Where(p => p.AgeRangeID.Equals(participantentry.FirstOrDefault().AgeRangeID)), "AgeRangeID", "Name", participantentry.FirstOrDefault().AgeRangeID);
        //        ViewBag.GenderID = new SelectList(db.Genders.Where(p => p.GenderID.Equals(participantentry.FirstOrDefault().GenderID)), "GenderID", "Name", participantentry.FirstOrDefault().GenderID);
        //        ViewBag.RegTypeID = new SelectList(db.RegTypes.Where(p => p.RegTypeID.Equals(participantentry.FirstOrDefault().RegTypeID)), "RegTypeID", "Name", participantentry.FirstOrDefault().RegTypeID);
        //        ViewBag.FellowshipID = new SelectList(db.Fellowships.Where(p => p.ServiceID.Equals(participantentry.FirstOrDefault().ServiceID)), "FellowshipID", "Name", participantentry.FirstOrDefault().FellowshipID);
        //        ViewBag.RoomTypeID = new SelectList(db.RoomTypes.Where(p => p.RegTypeID.Equals(participantentry.FirstOrDefault().RegTypeID)), "RoomTypeID", "Name", participantentry.FirstOrDefault().RoomTypeID);
        //        ViewBag.PartPrice = participantentry.FirstOrDefault().PartPrice;

        //        return View(participantentry.FirstOrDefault());
        //    }
        //}

        ////
        //// POST: /Participant/Page2/5?RegUID=xxx
        //[HttpPost]
        //public ActionResult Page2(string RegUID, int id, ParticipantEntry participantentry)
        //{
        //    if (RegUID == null || id == 0)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
            
        //    RegistrationEntry FoundEntry = new RegistrationEntry();
        //    int RegID = FoundEntry.RegUIDtoID(RegUID);

        //    if (ModelState.IsValid && RegID != 0)
        //    {
        //        participantentry.RegistrationID = RegID;
        //        participantentry.ParticipantID = id;
        //        participantentry.StatusID = (int)2;

        //        RegPrice FoundPrice = new RegPrice();
        //        participantentry.PartPrice = FoundPrice.PriceReturn(participantentry.AgeRangeID, participantentry.RegTypeID);


        //        db.Entry(participantentry).State = EntityState.Modified;
        //        db.SaveChanges();


        //        EventHistory NewEvent = new EventHistory();
        //        NewEvent.AddHistory(RegID, "Participant Confirmed", participantentry.ParticipantID);

        //        return RedirectToAction("Modify", "Register", new { RegUID = RegUID });
        //    }

        //    ViewBag.RegUID = RegUID;
        //    ViewBag.RegistrationID = RegID;
        //    ViewBag.ParticipantID = participantentry.ParticipantID;
        //    ViewBag.ServiceID = new SelectList(db.Services.Where(p => p.ServiceID.Equals(participantentry.ServiceID)), "ServiceID", "Name", participantentry.ServiceID);
        //    ViewBag.AgeRangeID = new SelectList(db.AgeRanges.Where(p => p.AgeRangeID.Equals(participantentry.AgeRangeID)), "AgeRangeID", "Name", participantentry.AgeRangeID);
        //    ViewBag.GenderID = new SelectList(db.Genders.Where(p => p.GenderID.Equals(participantentry.GenderID)), "GenderID", "Name", participantentry.GenderID);
        //    ViewBag.RegTypeID = new SelectList(db.RegTypes.Where(p => p.RegTypeID.Equals(participantentry.RegTypeID)), "RegTypeID", "Name", participantentry.RegTypeID);
        //    ViewBag.FellowshipID = new SelectList(db.Fellowships.Where(p => p.ServiceID.Equals(participantentry.ServiceID)), "FellowshipID", "Name", participantentry.FellowshipID);
        //    ViewBag.RoomTypeID = new SelectList(db.RoomTypes.Where(p => p.RegTypeID.Equals(participantentry.RegTypeID)), "RoomTypeID", "Name", participantentry.RoomTypeID);
        //    ViewBag.PartPrice = participantentry.PartPrice;
            
        //    return View(participantentry);
        //}

        
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
    
    }
}