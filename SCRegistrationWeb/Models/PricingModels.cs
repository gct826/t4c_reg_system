using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SCRegistrationWeb.Models
{
    public class RegPrice
    {
        SCRegistrationContext _db = new SCRegistrationContext();

        [Key]
        [ScaffoldColumn(false)]
        public int RegTypeID { get; set; }

        [Required(ErrorMessage = "Age selection is required")]
        [DisplayName("Age Group")]
        public int AgeRangeID { get; set; }

        [Required(ErrorMessage = "Part Time Price is required")]
        [DisplayName("Part Time Price")]
        [DataType(DataType.Currency)]
        public decimal PartTimePrice { get; set; }

        [Required(ErrorMessage = "Full Time Price is required")]
        [DisplayName("Full Time Price")]
        [DataType(DataType.Currency)]
        public decimal FullTimePrice { get; set; }

        public virtual AgeRange AgeRange { get; set; }

        public decimal PriceReturn(int ARangeID = 0, int RTypeID = 0)
        {
            if (ARangeID != 0 && RTypeID != 0)
            {
                var RegPrice = _db.RegPrices.Where(p => p.AgeRangeID.Equals(ARangeID));
                if (RegPrice != null)
                {
                    RegPrice FoundRegPrice = RegPrice.First();
                    if (FoundRegPrice != null && RTypeID == 1)
                    {
                        return FoundRegPrice.PartTimePrice;
                    }                     
                    if (FoundRegPrice != null && RTypeID == 2)
                    {
                        return FoundRegPrice.FullTimePrice;
                    }
                    return (decimal)999;
                }
                else
                {
                    return (decimal)999;
                }
            }
            return (decimal)999;
        }
    }
}