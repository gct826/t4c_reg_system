using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SCRegistrationWeb.Models
{
    public class Status
    {
        [Key]
        [ScaffoldColumn(false)]
        public int StatusID { get; set; }

        [StringLength(20, ErrorMessage = "Too Long")]
        [Required(ErrorMessage = "Status is required")]
        [DisplayName("Status")]
        public string Name { get; set; }
    }

    public class Service
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ServiceID { get; set; }

        [Required(ErrorMessage = "Congregation is required")]
        [StringLength(20, ErrorMessage = "Too Long")]
        [DisplayName("Congregation 所屬會眾")]
        public string Name { get; set; }
    }

    public class AgeRange
    {
        [Key]
        [ScaffoldColumn(false)]
        public int AgeRangeID { get; set; }

        [Required(ErrorMessage = "Age Range is required")]
        [StringLength(20, ErrorMessage = "Too Long")]
        [DisplayName("Age Range")]
        public string Name { get; set; }
    }

    public class Gender
    {
        [Key]
        [ScaffoldColumn(false)]
        public int GenderID { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(20, ErrorMessage = "Too Long")]
        [DisplayName("Gender")]
        public string Name { get; set; }
    }

    public class RegType
    {
        [Key]
        [ScaffoldColumn(false)]
        public int RegTypeID { get; set; }

        [Required(ErrorMessage = "Registration Type is required")]
        [StringLength(20, ErrorMessage = "Too Long")]
        [DisplayName("Registration Type")]
        public string Name { get; set; }
    }
    
    public class Fellowship
    {
        [Key]
        [ScaffoldColumn(false)]
        public int FellowshipID { get; set; }

        [Required(ErrorMessage = "Congregation is required")]
        [DisplayName("Congregation")]
        public int ServiceID { get; set; }

        [Required(ErrorMessage = "Fellowship Name is required")]
        [StringLength(40, ErrorMessage = "Too Long")]
        [DisplayName("Fellowship")]
        public string Name { get; set; }
        
        public virtual Service Service { get; set; }
    }

    public class RoomType
    {
        [Key]
        [ScaffoldColumn(false)]
        public int RoomTypeID { get; set; }

        [Required(ErrorMessage = "Registration Type is required")]
        [DisplayName("Registration Type")]
        public int RegTypeID { get; set; }

        [Required(ErrorMessage = "Room Type is required")]
        [StringLength(40, ErrorMessage = "Too Long")]
        [DisplayName("Room Type")]
        public string Name { get; set; }

        public virtual RegType RegTypes { get; set; }
    }
    
    /*public class MaritalStatus
    {
        [Key]
        [ScaffoldColumn(false)]
        public int AgeRangeID { get; set; }

        [Required(ErrorMessage = "Marital Status is required")]
        [StringLength(20, ErrorMessage = "Too Long")]
        [DisplayName("Marital Status")]
        public string Name { get; set; }
    }*/

}