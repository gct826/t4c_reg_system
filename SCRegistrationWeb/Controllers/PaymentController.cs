using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SCRegistrationWeb.Models;

namespace SCRegistrationWeb.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PaymentController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /Payment/
        //public ActionResult Index()
        //{
        //    return View();
       // }

        //
        // GET: /Payment/List

        public ActionResult Index()
        {
            var paymententries = db.PaymentEntries.Include(p => p.PmtStatuses).Include(p => p.PmtTypes);
            return View(paymententries.OrderByDescending(p => p.PaymentDate).ToList());
        }

        //
        // GET: /Payment/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    PaymentEntry paymententry = db.PaymentEntries.Find(id);
        //    if (paymententry == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(paymententry);
        //}

        //
        // GET: /Payment/Create

        //public ActionResult Create()
        //{
        //    ViewBag.isEmpty = true;
        //    return View();
        //}

        //
        // GET: /Payment/Create
        public ActionResult Create(FormCollection values, int id =0 )
        {
            if (values.Count == 0 )
            {
                ViewBag.isEmpty = true;
                ViewBag.tempID = id;
                return View();
            }

            return View();
        }
        
        //
        // POST: /Payment/Create
        [HttpPost]
        public ActionResult Create(PaymentEntry paymententry)
        {

            if (paymententry.RegID != 0)
            {
                
                RegistrationEntry RegLookup = new RegistrationEntry();

                RegLookup.RegistrationUID = RegLookup.RegIDtoUID(paymententry.RegID);

                if (RegLookup.RegistrationUID != null)
                {

                    if (paymententry.PmtStatusID == 0)
                    {
                        ViewBag.isEmpty = false;
                        ViewBag.RegID = paymententry.RegID;
                        ViewBag.RegAmtOwes = RegLookup.RegTotalPrice(paymententry.RegID);

                        paymententry.PaymentDate = DateTime.Now;
                        paymententry.PmtStatusID = (int)1;

                        //ViewBag.PmtStatusID = new SelectList(db.PmtStatuses, "PmtStatusID", "Name");
                        ViewBag.PmtTypeID = new SelectList(db.PmtTypes, "PmtTypeID", "Name");
                        return View(paymententry);
                    }

                    if (paymententry.PmtStatusID == 1 && paymententry.PmtTypeID == 0)
                    {
                        ViewBag.isEmpty = false;
                        ViewBag.RegID = paymententry.RegID;
                        ViewBag.RegAmtOwes = RegLookup.RegTotalPrice(paymententry.RegID);
                        ViewBag.Message = "Please select a payment type";

                        paymententry.PaymentDate = DateTime.Now;
                        paymententry.PmtStatusID = (int)1;

                        ViewBag.PmtTypeID = new SelectList(db.PmtTypes, "PmtTypeID", "Name");
                        return View(paymententry);
                    }
                    
                    if (paymententry.PmtStatusID == 1 && paymententry.PaymentAmt <= (decimal)0)
                    {
                        ViewBag.isEmpty = false;
                        ViewBag.RegID = paymententry.RegID;
                        ViewBag.RegAmtOwes = RegLookup.RegTotalPrice(paymententry.RegID);
                        ViewBag.Message = "Payment Amount has to be greater then 0";

                        paymententry.PaymentDate = DateTime.Now;
                        paymententry.PmtStatusID = (int)1;

                        ViewBag.PmtTypeID = new SelectList(db.PmtTypes, "PmtTypeID", "Name", paymententry.PmtTypeID);
                        return View(paymententry);
                    }

                    if (paymententry.PmtStatusID == 1 && ModelState.IsValid)
                    {
                        db.PaymentEntries.Add(paymententry);
                        db.SaveChanges();

                        EventHistory NewEvent = new EventHistory();
                        NewEvent.AddHistory(paymententry.RegID, "Payment Entered", paymententry.PaymentID);

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.Message = "Registration ID not found";
                    ViewBag.isEmpty = true;
                    return View();
                }
            }



            ViewBag.Message = "Error";
            ViewBag.isEmpty = true;
            return View();
        }

        //
        // GET: /Payment/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PaymentEntry paymententry = db.PaymentEntries.Find(id);
            if (paymententry == null)
            {
                return HttpNotFound();
            }
            ViewBag.PmtStatusID = new SelectList(db.PmtStatuses, "PmtStatusID", "Name", paymententry.PmtStatusID);
            ViewBag.PmtTypeID = new SelectList(db.PmtTypes, "PmtTypeID", "Name", paymententry.PmtTypeID);
            return View(paymententry);
        }

        //
        // POST: /Payment/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, PaymentEntry paymententry)
        {
            if (ModelState.IsValid)
            {
                paymententry.PaymentID = id;

                db.Entry(paymententry).State = EntityState.Modified;
                db.SaveChanges();

                EventHistory NewEvent = new EventHistory();
                NewEvent.AddHistory(paymententry.RegID, "Payment Entry Modified", id);


                return RedirectToAction("Index");
            }
            ViewBag.PmtStatusID = new SelectList(db.PmtStatuses, "PmtStatusID", "Name", paymententry.PmtStatusID);
            ViewBag.PmtTypeID = new SelectList(db.PmtTypes, "PmtTypeID", "Name", paymententry.PmtTypeID);
            return View(paymententry);
        }

        //
        // GET: /Payment/RegView
        public ActionResult RegView()
        {
            var registrationList = db.RegEntries.Where(b => b.IsConfirmed.Equals(true)).ToList();

            foreach (var registration in registrationList)
            {
                decimal totalRegPrice = registration.RegTotalPrice(registration.RegistrationID);

                var paymentEntries = from m in db.PaymentEntries.Where(p => p.RegID.Equals(registration.RegistrationID))
                                     select m;

                decimal totalScholarship = (decimal) 0;
                decimal totalCashRecieved = (decimal) 0;
                decimal totalCheckRecieved = (decimal) 0;
                decimal totalRefund = (decimal) 0;
                decimal totalAdjustment = (decimal) 0;
                decimal totalCreditCard = (decimal) 0;

                foreach (var item in paymentEntries)
                {
                    if (item.PmtTypeID == 1 && item.PmtStatusID != 3)
                    {
                        totalScholarship = totalScholarship + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 2 && item.PmtStatusID != 3)
                    {
                        totalCashRecieved = totalCashRecieved + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 3 && item.PmtStatusID != 3)
                    {
                        totalCheckRecieved = totalCheckRecieved + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 4 && item.PmtStatusID != 3)
                    {
                        totalRefund = totalRefund + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 5 && item.PmtStatusID != 3)
                    {
                        totalAdjustment = totalAdjustment + item.PaymentAmt;
                    }

                    if (item.PmtTypeID == 6 && item.PmtStatusID != 3)
                    {
                        totalCreditCard = totalCreditCard + item.PaymentAmt;
                    }
                }

                ViewBag.totalRemaining = totalRegPrice - totalScholarship -
                                         totalCashRecieved - totalCheckRecieved - totalRefund -
                                         totalAdjustment - totalCreditCard;
            }
            return View();


        }
    }
}