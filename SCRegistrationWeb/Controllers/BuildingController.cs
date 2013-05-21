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
    public class BuildingController : Controller
    {
        private SCRegistrationContext db = new SCRegistrationContext();

        //
        // GET: /Building/
        public ActionResult Index()
        {
            var buildings = db.Buildings.Include(b => b.RoomTypes);
            return View(buildings.ToList());
        }

        //
        // GET: /Building/Details/5
        public ActionResult Details(int id = 0)
        {
            Building building = db.Buildings.FirstOrDefault(b => b.BuildingID.Equals(id));
            if (building == null)
            {
                ViewBag.BuildingFound = false;
                return View();
            }

            ViewBag.BuildingFound = true;
            ViewBag.BuildingId = building.BuildingID;
            return View();
        }

        //
        // GET: /Building/BuildingHeader/5
        public ActionResult BuildingHeader(int id = 0, int roomid = 0)
        {
            Building building = db.Buildings.Include(b => b.RoomTypes).FirstOrDefault(b => b.BuildingID.Equals(id));
            if (building == null)
            {
                return HttpNotFound();
            }

            ViewBag.RoomFound = false;

            if (roomid != 0)
            {

                Room room = db.Rooms.FirstOrDefault(b => b.RoomID.Equals(roomid));

                if (room != null)
                {
                    ViewBag.RoomFound = true;
                    ViewBag.RoomId = room.RoomID;
                    ViewBag.RoomName = room.Name;
                    ViewBag.RoomNumofBeds = room.NumOfBeds;
                }
            }

            ViewBag.BuildingFound = true;
            ViewBag.BuildingId = building.BuildingID;
            ViewBag.BuildingName = building.Name;
            ViewBag.BuildingType = building.RoomTypes.Name;

            return PartialView();
        }

        //
        // GET: /Building/TotalBeds/5
        public ActionResult TotalBeds(int id = 0)
        {
            Building building = new Building();

            if (id >= 0)
            {
                ViewBag.TotalBeds = building.TotalBeds(id);
                ViewBag.TotalAvailable = building.TotalVacantBeds(id);
                return PartialView();
            }

            return PartialView();
        }

        //
        // GET: /Building/RoomList/5
        public ActionResult RoomList(int id = 0)
        {
            if (id != 0)
            {
                var rooms = db.Rooms.Where(b => b.BuildingID.Equals(id));
                return PartialView(rooms.ToList());
            }
            
            return PartialView();

        }

        //
        // GET: /Building/RoomDetail/5
        public ActionResult RoomDetail(int id = 0)
        {
            if (id != 0)
            {
                Room room = db.Rooms.FirstOrDefault(b => b.RoomID.Equals(id));

                if (room != null)
                {
                    
                var foundParticipant = db.RoomAssignments
                    .Where(b => b.RoomID.Equals(id))
                    .Where(b => b.IsDeleted.Equals(false));

                ViewBag.VacantRooms = room.VacantBeds(id);
                    
                return PartialView(foundParticipant);
                }

                return PartialView();
            }

            return PartialView();
        }

        //
        // GET: /Building/RoomBedAvail/5
        public ActionResult RoomBedAvail(int id = 0)
        {
            if (id != 0)
            {
                Room room = db.Rooms.FirstOrDefault(b => b.RoomID.Equals(id));

                if (room != null)
                {
                    ViewBag.numOfAvail = room.VacantBeds(id);

                    return PartialView();
                }
                
                return PartialView();
            }

            return PartialView();
        }
    
        //
        // GET: /Building/PartDetail/5
        public ActionResult PartDetail(int id = 0)
        {
            if (id != 0)
            {
                ParticipantEntry participant = db.ParticipantEntries
                    .Include(b => b.Genders)
                    .Include(b => b.AgeRanges)
                    .Include(b => b.Services)
                    .Include(b => b.Fellowships)
                    .Include(b => b.RoomTypes)
                    .FirstOrDefault(b => b.ParticipantID.Equals(id));

                if (participant != null)
                {
                    ViewBag.PartFound = true;
                    return PartialView(participant);
                }

                ViewBag.PartFound = false;
                return PartialView();
            }

            ViewBag.PartFound = false;
            return PartialView();

        }

        //
        // GET: /Building/RoomAssign
        public ActionResult RoomAssign(int id = 0)
        {
            if (id != 0)
            {
                Room room = db.Rooms.FirstOrDefault(b => b.RoomID.Equals(id));

                if (room != null)
                {
                    var foundParticipant = db.RoomAssignments.Where(b => b.RoomID.Equals(id));

                    int vacantRooms = room.NumOfBeds = foundParticipant.Count();

                    ViewBag.RoomFound = true;
                    ViewBag.BuildingId = room.BuildingID;
                    ViewBag.RoomId = room.RoomID;
                    ViewBag.VacantBeds = room.VacantBeds(id);
                    return View();
                }

                ViewBag.RoomFound = false;
                return View();

            }

            ViewBag.RoomFound = false;
            return View();

        }
   
        //
        // GET: /Building/PartSelectList/5
        public ActionResult PartSelectList(int id = 0, string RegID = null, string searchString = null, int RoomTypeID = 0, int ServiceID = 0)
        {
            var allParticipant = db.ParticipantEntries
                                   .Include(b => b.Genders)
                                   .Include(b => b.AgeRanges)
                                   .Include(b => b.Services)
                                   .Include(b => b.Fellowships)
                                   .Include(b => b.RoomTypes)
                                   .Where(b => b.StatusID.Equals((int) 3))
                                   .Where(b => b.RegTypeID.Equals((int) 1));
            
            if (!string.IsNullOrEmpty(RegID))
            {
                int regId = new int();

                int.TryParse(RegID, out regId);

                if (regId != 0)
                {
                    allParticipant = allParticipant.Where(s => s.RegistrationID == regId);
                }

            }

            if (!string.IsNullOrEmpty(searchString))
            {
                allParticipant = allParticipant.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString) || s.RegistrationEntries.Email.Contains(searchString) || s.RegistrationEntries.Phone.Contains(searchString));
            }

            if (ServiceID != 0)
            {
                allParticipant = allParticipant.Where(s => s.ServiceID == ServiceID);

            }

            if (RoomTypeID != 0)
            {
                allParticipant = allParticipant.Where(s => s.RoomTypeID == RoomTypeID);

            }

            var remainingParticipant = allParticipant.OrderBy(b => b.RegistrationID).ToList();
            
            var roomedParticpant = db.RoomAssignments
                                     .Include(b => b.ParticipantEntries)
                                     .Where(b => b.IsDeleted.Equals(false))
                                     .ToList();

            foreach (var roomAssignment in roomedParticpant)
            {
                ParticipantEntry foundParticipant =
                    db.ParticipantEntries.FirstOrDefault(b => b.ParticipantID.Equals(roomAssignment.PartID));

                if (foundParticipant != null)
                {
                    remainingParticipant.Remove(foundParticipant);
                }

            }

            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");
            ViewBag.RoomTypeID = new SelectList(db.RoomTypes, "RoomTypeID", "Name");
            ViewBag.RoomID = id;

            return PartialView(remainingParticipant);
        }

        //
        // GET: /Building/PartAssign/5
        public ActionResult PartAssign(int roomid = 0, int partid =0)
        {
            if (roomid == 0)
            {
                return RedirectToAction("Index");
            }


            if (partid == 0)
            {
                return RedirectToAction("Index");
            }

            var foundParticipant = db.ParticipantEntries.FirstOrDefault(b => b.ParticipantID.Equals(partid));
            
            if (foundParticipant == null)
            {
                return RedirectToAction("Index");
            }

            RoomAssignment newAssignment = new RoomAssignment();

            newAssignment.PartID = partid;
            newAssignment.RoomID = roomid;
            newAssignment.IsDeleted = false;

            if (ModelState.IsValid)
            {
                db.RoomAssignments.Add(newAssignment);
                db.SaveChanges();

                EventHistory newEvent = new EventHistory();
                newEvent.AddHistory(foundParticipant.RegistrationID, "Room Assigned", newAssignment.RoomAssignmentID);

                return RedirectToAction("RoomAssign", new {id = roomid});
            }

            return RedirectToAction("Index");

        }
    
        //
        // GET: /Building/PartUnassign/5
        public ActionResult PartUnassign(int partid = 0)
        {

            if (partid == 0)
            {
                return RedirectToAction("Index");
            }

            var foundParticipant = db.ParticipantEntries.FirstOrDefault(b => b.ParticipantID.Equals(partid));

            if (foundParticipant == null)
            {
                return RedirectToAction("Index");
            }

            var foundAssignment = db.RoomAssignments
                                  .Where(b => b.IsDeleted.Equals(false))
                                  .FirstOrDefault(b => b.PartID.Equals(partid));

            if (foundAssignment == null)
            {
                return RedirectToAction("Index");
            }

            foundAssignment.IsDeleted = true;

            if (ModelState.IsValid)
            {
                db.Entry(foundAssignment).State = EntityState.Modified;
                db.SaveChanges();


                EventHistory newEvent = new EventHistory();
                newEvent.AddHistory(foundParticipant.RegistrationID, "Room UnAssigned", foundAssignment.RoomAssignmentID);

                return RedirectToAction("RoomAssign", new {id = foundAssignment.RoomID});
            }

            return RedirectToAction("Index");

        }
    }


}