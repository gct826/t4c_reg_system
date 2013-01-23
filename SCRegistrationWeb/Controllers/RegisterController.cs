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
    public class RegisterController : Controller
    {
        private SCRegistrationContext _db = new SCRegistrationContext();

        //
        // GET: /Register/
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Register/Create
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Create New")]
        public ActionResult Create(FormCollection values)
        {
            var order = new RegistrationEntry();

            TryUpdateModel(order);

            try
            {
                order.Email = order.Email.ToLower();
                order.Phone = new string(order.Phone.Where(c => char.IsDigit(c)).ToArray());

                if (_db.RegEntries.Any(e => e.Email == order.Email))
                {
                    ViewBag.Message = "Registration Email already exist";
                    return View(order);
                }
                else if (_db.RegEntries.Any(e => e.Phone == order.Phone))
                {
                    ViewBag.Message = "Registration Phone number already exist";
                    return View(order);
                }
                else
                {
                    ViewBag.Message = null;

                    Guid NewKey = System.Guid.NewGuid();

                    order.RegistrationUID = NewKey.ToString();

                    order.DateCreated = DateTime.Now;
                    order.IsConfirmed = false;

                    _db.RegEntries.Add(order);
                    _db.SaveChanges();

          
                    EventHistory NewEvent = new EventHistory();
                    NewEvent.AddHistory(order.RegUIDtoID(order.RegistrationUID),"New Registration Created",0);

          
                    return RedirectToAction("Modify", "Participant", new { RegUID = order.RegistrationUID, isPage2=false, id=0 });
                    //return RedirectToAction("Index");
                }
            }
            catch
            {
                return View(order);
            }
        }

        //
        // GET: /Register/Modify/RegUID
        public ActionResult Modify(string RegUID)
        {
            if (RegUID == null)
            {
                ViewBag.Found = false;
                ViewBag.Message = RegUID;
                return View();
            }
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int FoundRegID = FoundEntry.RegUIDtoID(RegUID);

                if (FoundRegID != 0)
                {
                    EventHistory NewEvent = new EventHistory();
                    NewEvent.AddHistory(FoundRegID, "General Registration Opened", 0);

                    ViewBag.Found = true;
                    ViewBag.RegID = FoundRegID;
                    return View();
                }
                else
                {
                    ViewBag.Found = false;
                    ViewBag.Message = "Invalid Registration Key";
                    return View();
                }
            }
        }

        //
        // GET: /Register/Summary?RegID
        [ChildActionOnly]
        public ActionResult Summary(int Id = 0)
        {
            if (Id == 0)
            {
                ViewBag.Found = false;
                ViewBag.Message = "Participant not found";
                return PartialView();
            }
            else
            {
                var registrationentry = from m in _db.RegEntries.Where(p => p.RegistrationID.Equals(Id))
                                        select m;

                ViewBag.Found = true;
                return PartialView(registrationentry.ToList());
            }
        }


        //
        // POST: /Register/Unlock/RegUID
        public ActionResult Unlock(string RegUID, bool isAdmin = false)
        {
            if (RegUID == null)
            {
                ViewBag.Found = false;
                ViewBag.Message = RegUID;
                return View();
            }
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int FoundRegID = FoundEntry.RegUIDtoID(RegUID);

                if (FoundRegID != 0)
                {
                    var registrationentry = from m in _db.RegEntries.Where(p => p.RegistrationID.Equals(FoundRegID))
                                            select m;
               
                    ViewBag.Found = true;

                    FoundEntry = registrationentry.SingleOrDefault();
                    FoundEntry.IsConfirmed = false;

                    _db.Entry(FoundEntry).State = EntityState.Modified;

                    var PartEntry = from m in _db.ParticipantEntries.Include(p => p.RegistrationEntries).
                                   Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
                                   Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p => p.RoomTypes).
                                   Where(p => p.RegistrationID.Equals(FoundEntry.RegistrationID)).Where(p => !p.StatusID.Equals((int)4))
                                    select m;

                    foreach (ParticipantEntry FoundPart in PartEntry)
                    {
                        FoundPart.StatusID = (int)2;
                        _db.Entry(FoundPart).State = EntityState.Modified;
                    }
                    _db.SaveChanges();

                    if (isAdmin)
                    {
                        EventHistory NewEvent = new EventHistory();
                        NewEvent.AddHistory(FoundRegID, "Admin Registration Unlocked", 0);

                        return RedirectToAction("Detail", "SearchRegistration", new { Id = FoundRegID });
                    }
                    else
                    {
                        EventHistory NewEvent = new EventHistory();
                        NewEvent.AddHistory(FoundRegID, "General Registration Unlocked", 0);

                        return RedirectToAction("Modify", "Register", new { RegUID = RegUID });
                    }
                }
                else
                {
                    ViewBag.Found = false;
                    ViewBag.Message = "Invalid Registration Key";
                    return View();
                }
            }
        }

        //
        // POST: /Register/Lock/RegUID
        public ActionResult Lock(string RegUID, bool isAdmin = false)
        {
            if (RegUID == null)
            {
                ViewBag.Found = false;
                ViewBag.Message = RegUID;
                return View();
            }
            else
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int FoundRegID = FoundEntry.RegUIDtoID(RegUID);

                if (FoundRegID != 0)
                {
                    var registrationentry = from m in _db.RegEntries.Where(p => p.RegistrationID.Equals(FoundRegID))
                                            select m;

                    ViewBag.Found = true;

                    FoundEntry = registrationentry.SingleOrDefault();

                    var PartEntry = from m in _db.ParticipantEntries.Include(p => p.RegistrationEntries).
                                   Include(p => p.Statuses).Include(p => p.Services).Include(p => p.AgeRanges).
                                   Include(p => p.Genders).Include(p => p.RegTypes).Include(p => p.Fellowships).Include(p => p.RoomTypes).
                                   Where(p => p.RegistrationID.Equals(FoundEntry.RegistrationID)).Where(p => !p.StatusID.Equals((int)4))
                                    select m;

                    foreach (ParticipantEntry FoundPart in PartEntry)
                    {
                        FoundPart.StatusID = (int)3;
                        _db.Entry(FoundPart).State = EntityState.Modified;
                    }

                    FoundEntry.IsConfirmed = true;

                    _db.Entry(FoundEntry).State = EntityState.Modified;
                    _db.SaveChanges();

                    if (isAdmin)
                    {
                        EventHistory NewEvent = new EventHistory();
                        NewEvent.AddHistory(FoundRegID, "Admin Registration Locked", 0);

                        return RedirectToAction("Detail", "SearchRegistration", new { Id = FoundRegID });
                    }
                    else
                    {
                        EventHistory NewEvent = new EventHistory();
                        NewEvent.AddHistory(FoundRegID, "General Registration Locked", 0);

                        return RedirectToAction("Complete", "Register", new { RegUID = RegUID });
                    }
                }
                else
                {
                    ViewBag.Found = false;
                    ViewBag.Message = "Invalid Registration Key";
                    return View();
                }
            }
        }

        //
        // POST: /Register/Search
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Open Existing")]
        public ActionResult Search(FormCollection values)
        {
            var order = new RegistrationEntry();
            
            TryUpdateModel(order);
            
            try
            {
                order.Email = order.Email.ToLower();
                order.Phone = new string(order.Phone.Where(c => char.IsDigit(c)).ToArray());

                if (order.Email != null && order.Phone != null)
                {
                    var registers = from m in _db.RegEntries
                                    select m;

                    registers = registers.Where(s => s.Email.Equals(order.Email) && s.Phone.Equals(order.Phone));

                    RegistrationEntry FoundReg = registers.FirstOrDefault();
                    if (FoundReg == null)
                    {
                        ViewBag.Message = "Registration Not Found";
                        return View();
                    }
                    else
                    {
                        string RegKey = FoundReg.RegistrationUID;

                        return RedirectToAction("Modify", "Register", new { RegUID = RegKey });
                    }
                }

                else
                {
                    ViewBag.Message = "Please enter search string";
                    return View();
                }
            }
            catch
            {
                return View(order);
            }
 
        }
    
        //
        // GET: /Register/Complete/RegUID
        public ActionResult Complete(string RegUID)
        {
            {
                if (RegUID == null)
                {
                    ViewBag.Found = false;
                    return View();
                }
                else
                {
                    RegistrationEntry FoundEntry = new RegistrationEntry();
                    int FoundRegID = FoundEntry.RegUIDtoID(RegUID);

                    if (FoundRegID != 0)
                    {
                        ViewBag.Found = true;
                        ViewBag.TotalPrice = FoundEntry.RegTotalPrice(FoundRegID);
                        ViewBag.RegID = FoundRegID;

                        return View();
                    }
                    else
                    {
                        ViewBag.Found = false;
                        ViewBag.Message = "Invalid Registration Key";
                        return View();
                    }
                }
            }
        }

    }
}
    

