using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SCRegistrationWeb.Models
{
    public class RoomNote
    {
        [Key]
        [ScaffoldColumn(false)]
        public int RoomNoteID { get; set; }

        public int PartID { get; set; }

        [DisplayName("Small Group")]
        public string Note { get; set; }

        public ParticipantEntry ParticipantEntries { get; set; }
    }

    public class Building
    {
        SCRegistrationContext _db = new SCRegistrationContext();

        [Key]
        [ScaffoldColumn(false)]
        public int BuildingID { get; set; }

        [StringLength(20, ErrorMessage = "Too Long")]
        [Required(ErrorMessage = "Building Name is required")]
        [DisplayName("Building Name")]
        public string Name { get; set; }

        public int RoomTypeID { get; set; }

        public RoomType RoomTypes { get; set; }

        public int TotalBeds(int id = 0)
        {
            if (id != 0)
            {
                var foundRooms = _db.Rooms.Where(s => s.BuildingID.Equals(id));

                int totalBeds = (int)0;

                foreach (var item in foundRooms)
                {
                    totalBeds = totalBeds + item.NumOfBeds;
                }

                return totalBeds;

            }

            if (id == 0)
            {
                var foundRooms = _db.Rooms;
                int totalBeds = (int) 0;

                foreach (var item in foundRooms)
                {
                    totalBeds = totalBeds + item.NumOfBeds;
                }

                return totalBeds;

            }

            return (int)-1;
        }
        
        public int TotalVacantBeds(int id = 0)
        {
            if (id != 0)
            {
                var foundRooms = _db.Rooms.Where(s => s.BuildingID.Equals(id)).ToList();

                int totalVacantBeds = (int) 0;

                foreach (var item in foundRooms)
                {
                    totalVacantBeds = totalVacantBeds + item.VacantBeds(item.RoomID);
                }

                return totalVacantBeds;

            }
            
            if (id == 0)
            {
                var foundRooms = _db.Rooms;
                int totalVacantBeds = (int)0;

                foreach (var item in foundRooms)
                {
                    totalVacantBeds = totalVacantBeds + item.VacantBeds(item.RoomID);
                }

                return totalVacantBeds;

            }

            return (int) -1;
        }
        
    }

    public class Room
    {
        SCRegistrationContext _db = new SCRegistrationContext();

        [Key]
        [ScaffoldColumn(false)]
        public int RoomID { get; set; }

        [StringLength(20, ErrorMessage = "Too Long")]
        [Required(ErrorMessage = "Room Name is required")]
        [DisplayName("Room Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Number of Beds is required")]
        [DisplayName("Number of Beds")]
        public int NumOfBeds { get; set; }

        public int BuildingID { get; set; }

        public Building Buildings { get; set; }

        public int VacantBeds(int id = 0)
        {
            if (id != 0)
            {
                var foundRoom = _db.Rooms.FirstOrDefault(s => s.RoomID.Equals(id));

                var foundParticipants = _db.RoomAssignments
                    .Where(b => b.RoomID.Equals(id))
                    .Where(b => b.IsDeleted.Equals(false))
                    .ToList();

                int isInfant = (int)0;

                foreach (var participant in foundParticipants)
                {
                ParticipantEntry foundParticipant =
                    _db.ParticipantEntries.FirstOrDefault(b => b.ParticipantID.Equals(participant.PartID));

                    if (foundParticipant != null && foundParticipant.AgeRangeID == (int)1)
                    {
                        isInfant = isInfant + 1;
                    }
                }

                if (foundRoom == null)
                {
                    return (int)-1;
                }

                int vacantBeds = foundRoom.NumOfBeds - foundParticipants.Count() + isInfant;

                return vacantBeds;

            }

            return (int)-1;
        }
    }

    public class RoomAssignment
    {
        [Key]
        [ScaffoldColumn(false)]
        public int RoomAssignmentID { get; set; }

        public int PartID { get; set; }
        public int RoomID { get; set; }

        public bool IsDeleted { get; set; }

        public ParticipantEntry ParticipantEntries { get; set; }
        public Room Rooms { get; set; }
    }


}