using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SCRegistrationWeb.Models
{
    public class EventHistory
    {
        [Key]
        [ScaffoldColumn(false)]
        public int HistoryID { get; set; }

        [DisplayName("Time/Date")]
        public DateTime HistoryDate { get; set; }

        [DisplayName("Registration ID")]
        public int RegHistID { get; set; }

        [DisplayName("Event")]
        public string HistoryEvent { get; set; }

        [DisplayName("Event ID")]
        public int EventHistID { get; set; }

        public void AddHistory(int RegHistID = 0, string Event = "No Event Given", int EventHistID = 0)
        {
            if (RegHistID != 0)
            {
                SCRegistrationContext db = new SCRegistrationContext();

                EventHistory NewHistory = new EventHistory();

                NewHistory.HistoryDate = DateTime.Now;
                NewHistory.RegHistID = RegHistID;
                NewHistory.HistoryEvent = Event;
                NewHistory.EventHistID = EventHistID;

                db.EventHistory.Add(NewHistory);
                db.SaveChanges();

            }
            else
            {
            }
        }

    }
}