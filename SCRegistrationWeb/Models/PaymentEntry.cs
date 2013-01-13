using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace SCRegistrationWeb.Models
{
    public class PaymentEntry
    {
        [Key]
        [ScaffoldColumn(false)]
        public int PaymentID { get; set; }

        [DisplayName("Time/Date")]
        public DateTime PaymentDate { get; set; }

        [DisplayName("Payment Type")]
        public string PaymentType { get; set; }

        [DisplayName("Payment Amount")]
        public decimal PaymentAmt { get; set; }

        [DisplayName("Payment Status")]
        public int PaymentStat { get; set; }

        [DisplayName("Payment Comment")]
        public string PaymentComment { get; set; }

        [DisplayName("Registration Number")]
        public string RegID { get; set; }

        public RegistrationEntry RegistrationEntries { get; set; }

    }
}