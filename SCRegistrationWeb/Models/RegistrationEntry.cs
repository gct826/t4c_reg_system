using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SCRegistrationWeb.Models
{
    [Bind(Exclude = "RegistrationID")]
    public class RegistrationEntry
    {
        SCRegistrationContext _db = new SCRegistrationContext();

        [Key]
        [ScaffoldColumn(false)]
        [DisplayName("Reg ID 注册号")]
        public int RegistrationID { get; set; }

        [ScaffoldColumn(false)]
        public string RegistrationUID { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DisplayName("E-mail 電郵")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [DisplayName("Phone 電話")]
        [RegularExpression(@"^\(?([2-9][0-8][0-9])\)?[- .●]?([2-9][0-9]{2})[- .●]?([0-9]{4})$", ErrorMessage = "Phone is not valid.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [ScaffoldColumn(false)]
        public Boolean IsConfirmed { get; set; }

        [ScaffoldColumn(false)]
        public DateTime DateCreated { get; set; }

        public int RegUIDtoID(string UID)
        {
            if (!String.IsNullOrEmpty(UID))
            {
                var RegEntry = _db.RegEntries.Where(s => s.RegistrationUID.Contains(UID));
                if (RegEntry != null)
                {
                    RegistrationEntry FoundEntry = RegEntry.FirstOrDefault();
                    if (FoundEntry != null)
                    {
                        return FoundEntry.RegistrationID;
                    }
                    else
                    {
                        return (int)0;
                    }
                }
                else
                {
                    return (int)0;
                }
            }
            else
            {
                return (int)0;
            }
        }

        public string RegIDtoUID(int ID=0)
        {
            if (ID != 0)
            {
                var RegEntry = _db.RegEntries.Where(s => s.RegistrationID.Equals(ID));
                if (RegEntry != null)
                {
                    RegistrationEntry FoundEntry = RegEntry.FirstOrDefault();
                    if (FoundEntry != null)
                    {
                        return FoundEntry.RegistrationUID;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public bool RegIsConfirm(int ID=0)
        {
            if (ID !=0)
            {
                var RegEntry = _db.RegEntries.Where(s => s.RegistrationID.Equals(ID));
                if (RegEntry != null)
                {
                    RegistrationEntry FoundEntry = RegEntry.FirstOrDefault();
                    if (FoundEntry != null)
                    {
                        return FoundEntry.IsConfirmed;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public decimal RegTotalPrice(int ID=0)
        {
            if (ID != 0)
            {
                var RegEntry = _db.RegEntries.Where(s => s.RegistrationID.Equals(ID));
                if (RegEntry != null)
                {
                    RegistrationEntry FoundEntry = RegEntry.FirstOrDefault();
                    if (FoundEntry != null)
                    {
                       var PartEntry = from m in _db.ParticipantEntries.Where(p => p.RegistrationID.Equals(ID)).Where(p => !p.StatusID.Equals((int)4))
                                        select m;

                        decimal totalPrice = (decimal)0;

                        foreach (var item in PartEntry)
                        {
                            totalPrice = totalPrice + item.PartPrice;
                        }
                        
                        return totalPrice;
                    }
                    else
                    {
                        return (decimal)-99;
                    }
                }
                else
                {
                    return (decimal)-99;
                }
            }
            else
            {
                return (decimal)-99;
            }
        }

        public bool RegIsComplete(int ID=0)
        {
            if (ID != 0)
            {
                var RegEntry = _db.RegEntries.Where(s => s.RegistrationID.Equals(ID));
                if (RegEntry != null)
                {
                    RegistrationEntry FoundEntry = RegEntry.FirstOrDefault();
                    if (FoundEntry != null)
                    {
                        var PartEntry = from m in _db.ParticipantEntries.Where(p => p.RegistrationID.Equals(ID)).Where(p => !p.StatusID.Equals((int)4))
                                        select m;

                        bool IsComplete = true;

                        foreach (var item in PartEntry)
                        {
                            if (item.StatusID != (int)2)
                            {
                                IsComplete = false;
                            }
                        }

                        return IsComplete;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }     
    
        public bool RegIsAllowed()
        {
            var regStartDate = new DateTime(2013, 4, 28); //the date registration is opened.
            var regEndDate = new DateTime(2013,6,10); //the day registration is closed.
            bool adminTurnOff = false;

            if (adminTurnOff)
            {
                return false;
            }

            if (DateTime.Now <= regStartDate)
            {
                return false;
            }

            if (DateTime.Now >= regEndDate)
            {
                return false;
            }

            return true;
        }


    }
}
