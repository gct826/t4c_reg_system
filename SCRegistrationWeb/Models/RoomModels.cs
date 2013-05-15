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
            return (int)0;
        }
    }

    public class Room
    {
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