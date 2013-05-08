using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SCRegistrationWeb.Models
{
    public class SmallGroup
    {
        [Key]
        [ScaffoldColumn(false)]
        public int SmallGroupID { get; set; }

        public int PartID { get; set; }
        public int ServiceID { get; set; }

        [DisplayName("Small Group")]
        public string SmallGroupName { get; set; }

        public ParticipantEntry ParticipantEntries { get; set; }
        public Service Services { get; set; }
    }

}