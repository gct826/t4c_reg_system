using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SCRegistrationWeb.Models
{
    [Bind(Exclude = "ParticipantID")]
    public class ParticipantEntry
    {
        SCRegistrationContext _db = new SCRegistrationContext();

        [Key]
        [ScaffoldColumn(false)]
        [DisplayName("ID 编号")]
        public int ParticipantID { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("Reg ID 注册号")]
        public int RegistrationID { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("Status")]
        public int StatusID { get; set; }

        [Required(ErrorMessage = "Congregation is required")]
        [DisplayName("Congregation 所屬會眾")]
        public int ServiceID { get; set; }

        [Required(ErrorMessage = "Age Range is required")]
        [DisplayName("Age Range 年龄组")]
        public int AgeRangeID { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [DisplayName("Gender 性別")]
        public int GenderID { get; set; }

        [Required(ErrorMessage = "Registration Type is required")]
        [DisplayName("Registration Type 注册类型")]
        public int RegTypeID { get; set; }

        [DisplayName("Fellowship 所屬團契")]
        public int FellowshipID { get; set; }

        [DisplayName("Rooming Preference 房型偏好")]
        public int RoomTypeID { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First Name 名(英)")]
        [StringLength(20, ErrorMessage = "Too Long")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name 姓(英)")]
        [StringLength(20, ErrorMessage = "Too Long")]
        public string LastName { get; set; }

        [DisplayName("Chinese Name 中文姓名")]
        [StringLength(20, ErrorMessage = "Too Long")]
        public string ChineseName { get; set; }

        [DisplayName("Price 价钱")]
        public decimal PartPrice { get; set; }

        public RegistrationEntry RegistrationEntries { get; set; }
        public Status Statuses { get; set; }
        public Service Services { get; set; }
        public AgeRange AgeRanges { get; set; }
        public Gender Genders { get; set; }
        public RegType RegTypes { get; set; }
        public Fellowship Fellowships { get; set; }
        public RoomType RoomTypes { get; set; }

    }


}