using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SCRegistrationWeb.Models
{
    public class Headset
    {
        [Key]
        [ScaffoldColumn(false)]
        public int HeadsetID { get; set; }

        [DisplayName("Headset Required")]
        public int ParticipantID { get; set; }

        public ParticipantEntry ParticipantEntries { get; set; }
    }
}