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
        [MultiButton(MatchFormKey = "action", MatchFormValue1 = "Create New Registration", MatchFormValue2 = "创建新注册")]
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
                    ViewBag.MessageEn = "Email or Phone Number already exist";
                    ViewBag.MessageCh = "电子邮件或电话号码已经存在";
                    return View(order);
                }
                else if (_db.RegEntries.Any(e => e.Phone == order.Phone))
                {
                    ViewBag.MessageEn = "Email or Phone Number already exist";
                    ViewBag.MessageCh = "电子邮件或电话号码已经存在";
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
                    NewEvent.AddHistory(order.RegUIDtoID(order.RegistrationUID), "New Registration Created", 0);


                    return RedirectToAction("Modify", "Participant", new { RegUID = order.RegistrationUID, isPage2 = false, id = 0 });
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
                return RedirectToAction("Index", "Register");
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
                    ViewBag.MessageEn = "Invalid Registration Key";
                    ViewBag.MessageCh = "没有找到登记";
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
                ViewBag.MessageEn = "Participant not found";
                ViewBag.MessageCh = "没有找到登记";
                return PartialView();
            }
            else
            {
                var registrationentry = from m in _db.RegEntries.Where(p => p.RegistrationID.Equals(Id))
                                        select m;

                RegistrationEntry FoundEntry = registrationentry.FirstOrDefault();

                ViewBag.Found = true;
                ViewBag.RegUID = FoundEntry.RegistrationUID;
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
                    ViewBag.MessageEn = "Invalid Registration Key";
                    ViewBag.MessageCh = "没有找到登记";
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
                    ViewBag.MessageEn = "Invalid Registration Key";
                    ViewBag.MessageCh = "没有找到登记";
                    return View();
                }
            }
        }

        //
        // POST: /Register/Search
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue1 = "Modify Registration", MatchFormValue2 = "修改注册")]
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
                        ViewBag.MessageEn = "Registration Not Found";
                        ViewBag.MessageCh = "没有找到登记";
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
        public ActionResult Complete(string RegUID, FormCollection values)
        {
            if (RegUID == null)
            {
                ViewBag.Found = false;
                return View();
            }

            if (values.Count == 0)
            {
                RegistrationEntry FoundEntry = new RegistrationEntry();
                int FoundRegID = FoundEntry.RegUIDtoID(RegUID);

                if (FoundRegID != 0)
                {
                    ViewBag.Found = true;
                    ViewBag.Scholarship = false;
                    ViewBag.TotalPrice = FoundEntry.RegTotalPrice(FoundRegID);
                    ViewBag.RegID = FoundRegID;
                    ViewBag.RegUID = RegUID;

                    return View();
                }
                else
                {
                    ViewBag.Found = false;
                    ViewBag.MessageEn = "Invalid Registration Key";
                    ViewBag.MessageCh = "没有找到登记";
                    return View();
                }
            }

            ViewBag.Found = false;
            return View();
        }

        //
        // POST: /Register/Complete/RegUID
        [HttpPost]
        public ActionResult Complete(string RegUID, PaymentEntry values)
        {
            if (RegUID == null)
            {
                ViewBag.Found = false;
                return View();
            }

            RegistrationEntry FoundEntry = new RegistrationEntry();
            int FoundRegID = FoundEntry.RegUIDtoID(RegUID);

            if (values.RegID == 0 && values.PaymentAmt == (decimal)0)
            {
                ViewBag.Found = true;
                ViewBag.Scholarship = true;
                ViewBag.TotalPrice = FoundEntry.RegTotalPrice(FoundRegID);
                ViewBag.RegID = FoundRegID;
                ViewBag.RegUID = RegUID;
                values.RegID = (int)FoundRegID;
                values.PaymentDate = DateTime.Now;
                values.PmtTypeID = (int)1;
                values.PmtStatusID = (int)1;

                return View(values);
            }

            if (values.RegID == FoundRegID && values.PaymentAmt <= (decimal)0)
            {
                ViewBag.Found = true;
                ViewBag.Scholarship = true;
                ViewBag.TotalPrice = FoundEntry.RegTotalPrice(FoundRegID);
                ViewBag.RegID = FoundRegID;
                ViewBag.RegUID = RegUID;
                ViewBag.MessageEn = "Please enter an Amount greater then 0";
                ViewBag.MessageCh = "请输入一个数目大于0";
                values.RegID = (int)FoundRegID;
                values.PaymentDate = DateTime.Now;
                values.PmtTypeID = (int)1;
                values.PmtStatusID = (int)1;

                return View(values);
            }

            if (values.RegID == FoundRegID && values.PaymentAmt > (decimal)0)
            {
                values.RegID = (int)FoundRegID;
                values.PaymentDate = DateTime.Now;
                values.PmtTypeID = (int)1;
                values.PmtStatusID = (int)1;

                _db.PaymentEntries.Add(values);
                _db.SaveChanges();

                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(values.RegID, "Scholarship Request Entered", values.PaymentID);

                return RedirectToAction("Modify", "Register", new { RegUID = RegUID });
            }

            ViewBag.Found = true;
            ViewBag.Scholarship = false;
            ViewBag.TotalPrice = FoundEntry.RegTotalPrice(FoundRegID);
            ViewBag.RegID = FoundRegID;

            return View();

        }


        //
        // GET: /Register/Scholarship/RegUID

        //public ActionResult Scholarship(string RegUID)
        //{
        //    {
        //        if (RegUID == null)
        //        {
        //            ViewBag.Found = false;
        //            return View();
        //        }
        //        else
        //        {
        //            RegistrationEntry FoundEntry = new RegistrationEntry();
        //            int FoundRegID = FoundEntry.RegUIDtoID(RegUID);

        //            if (FoundRegID != 0)
        //            {
        //                ViewBag.Found = true;
        //                ViewBag.TotalPrice = FoundEntry.RegTotalPrice(FoundRegID);
        //                ViewBag.RegID = FoundRegID;

        //                return View();
        //            }
        //            else
        //            {
        //                ViewBag.Found = false;
        //                ViewBag.MessageEn = "Invalid Registration Key";
        //                ViewBag.MessageCh = "没有找到登记";
        //                return View();
        //            }
        //        }
        //    }
        //}

        //
        // GET: /Register/PaymentSummary/ID
        [ChildActionOnly]
        public ActionResult PaymentSummary(int ID)
        {
            if (ID == 0)
            {
                ViewBag.Found = false;
                return PartialView();
            }

            var RegEntry = _db.RegEntries.Where(s => s.RegistrationID.Equals(ID));

            if (RegEntry != null)
            {
                RegistrationEntry FoundEntries = RegEntry.FirstOrDefault();

                decimal totalRegPrice = FoundEntries.RegTotalPrice(ID);

                var PaymentEntries = from m in _db.PaymentEntries.Where(p => p.RegID.Equals(ID))
                                     select m;

                decimal totalScholarshipPending = (decimal)0;
                decimal totalScholarshipApproved = (decimal)0;
                decimal totalCashRecieved = (decimal)0;
                decimal totalCheckPending = (decimal)0;
                decimal totalCheckApproved = (decimal)0;
                decimal totalRefundPending = (decimal)0;
                decimal totalRefundApproved = (decimal)0;
                decimal totalAdjustment = (decimal)0;

                foreach (var item in PaymentEntries)
                {
                    if (item.PmtTypeID == 1 && item.PmtStatusID == 1)
                    {
                        totalScholarshipPending = totalScholarshipPending + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 1 && item.PmtStatusID == 2)
                    {
                        totalScholarshipApproved = totalScholarshipApproved + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 2 && item.PmtStatusID != 3)
                    {
                        totalCashRecieved = totalCashRecieved + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 3 && item.PmtStatusID == 1)
                    {
                        totalCheckPending = totalCheckPending + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 3 && item.PmtStatusID == 2)
                    {
                        totalCheckApproved = totalCheckApproved + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 4 && item.PmtStatusID == 1)
                    {
                        totalRefundPending = totalRefundPending + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 4 && item.PmtStatusID == 2)
                    {
                        totalRefundApproved = totalRefundApproved + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 5 && item.PmtStatusID != 3)
                    {
                        totalAdjustment = totalAdjustment + item.PaymentAmt;
                    }

                }

                ViewBag.Found = true;

                ViewBag.totalRegPrice = totalRegPrice;
                ViewBag.totalScholarshipPending = totalScholarshipPending;
                ViewBag.totalScholarshipApproved = totalScholarshipApproved;
                ViewBag.totalCashRecieved = totalCashRecieved;
                ViewBag.totalCheckPending = totalCheckPending;
                ViewBag.totalCheckApproved = totalCheckApproved;
                ViewBag.totalRefundPending = totalRefundPending;
                ViewBag.totalRefundApproved = totalRefundApproved;
                ViewBag.totalAdjustment = totalAdjustment;
                ViewBag.totalRemaining = totalRegPrice - totalScholarshipPending - totalScholarshipApproved - totalCashRecieved - totalCheckPending - totalCheckApproved - totalRefundPending - totalRefundApproved - totalAdjustment;
                return PartialView(PaymentEntries);

            }

            ViewBag.Found = false;
            return PartialView();

        }

        //
        // GET: /Register/Edit/RegUID
        public ActionResult Edit(string RegUID)
        {
            if (RegUID == null)
            {
                ViewBag.Found = false;
                ViewBag.Message = RegUID;
                return View();
            }
            else
            {
                var registrationentry = from m in _db.RegEntries.Where(p => p.RegistrationUID.Equals(RegUID))
                                        select m;

                RegistrationEntry FoundEntry = registrationentry.FirstOrDefault();

                ViewBag.Found = true;
                ViewBag.RegUID = FoundEntry.RegistrationUID;
                return View(FoundEntry);

            }
        }

        //
        // POST: /Register/Edit/RegUID
        [HttpPost]
        public ActionResult Edit(string RegUID, RegistrationEntry RegEntry)
        {
            if (RegUID == null)
            {
                ViewBag.Found = false;
                ViewBag.Message = RegUID;
                return View();
            }
            else
            {

                RegEntry.Email = RegEntry.Email.ToLower();
                RegEntry.Phone = new string(RegEntry.Phone.Where(c => char.IsDigit(c)).ToArray());

                var registrationentry = from m in _db.RegEntries.Where(p => p.RegistrationUID.Equals(RegUID))
                                        select m;

                RegistrationEntry FoundEntry = registrationentry.FirstOrDefault();

                var searchemail = from m in _db.RegEntries.Where(p => p.Email.Equals(RegEntry.Email))
                                  select m;

                RegistrationEntry SearchEmail = searchemail.FirstOrDefault();

                var searchphone = from m in _db.RegEntries.Where(p => p.Phone.Equals(RegEntry.Phone))
                                  select m;

                RegistrationEntry SearchPhone = searchphone.FirstOrDefault();

                if (FoundEntry != null && SearchEmail != null)
                {
                    if (FoundEntry.RegistrationID != SearchEmail.RegistrationID)
                    {
                        ViewBag.Found = true;
                        ViewBag.RegUID = FoundEntry.RegistrationUID;
                        ViewBag.MessageEn = "Email already exist";
                        ViewBag.MessageCh = "电子邮件已经存在";
                        return View(RegEntry);
                    }
                }

                if (FoundEntry != null && SearchPhone != null)
                {
                    if (FoundEntry.RegistrationID != SearchPhone.RegistrationID)
                    {
                        ViewBag.Found = true;
                        ViewBag.RegUID = FoundEntry.RegistrationUID;
                        ViewBag.MessageEn = "Phone Number already exist";
                        ViewBag.MessageCh = "电话号码已经存在";
                        return View(RegEntry);
                    }
                }

                FoundEntry.Email = RegEntry.Email;
                FoundEntry.Phone = RegEntry.Phone;

                _db.Entry(FoundEntry).State = EntityState.Modified;
                _db.SaveChanges();

                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(FoundEntry.RegistrationID, "Registration LogIn Changed", 0);

                return RedirectToAction("Modify", "Register", new { RegUID = FoundEntry.RegistrationUID });

            }
        }

    
    }
}

    

