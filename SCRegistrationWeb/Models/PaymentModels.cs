using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace SCRegistrationWeb.Models
{
    public class PmtStatus
    {
        [Key]
        [ScaffoldColumn(false)]
        public int PmtStatusID { get; set; }

        [StringLength(20, ErrorMessage = "Too Long")]
        [Required(ErrorMessage = "Status is required")]
        [DisplayName("Status")]
        public string Name { get; set; }
    }

    public class PmtType
    {
        [Key]
        [ScaffoldColumn(false)]
        public int PmtTypeID { get; set; }

        [Required(ErrorMessage = "Payment Type is required")]
        [StringLength(20, ErrorMessage = "Too Long")]
        [DisplayName("Payment Type")]
        public string Name { get; set; }
    }

    public class PaymentEntry
    {
        [Key]
        [ScaffoldColumn(false)]
        public int PaymentID { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DisplayName("Date")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Payment Type is required")]
        [DisplayName("Payment Type")]
        public int PmtTypeID { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [DisplayName("Payment Amount")]
        public decimal PaymentAmt { get; set; }

        [DisplayName("Payment Status")]
        public int PmtStatusID { get; set; }

        [DisplayName("Payment Comment")]
        public string PaymentComment { get; set; }

        [DisplayName("Registration Number")]
        public int RegID { get; set; }

        public RegistrationEntry RegistrationEntries { get; set; }
        public PmtStatus PmtStatuses { get; set; }
        public PmtType PmtTypes { get; set; }

    }

}