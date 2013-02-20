namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using SCRegistrationWeb.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<SCRegistrationWeb.Models.SCRegistrationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SCRegistrationWeb.Models.SCRegistrationContext context)
        {
            context.Statuses.AddOrUpdate(
                 s => s.StatusID,
                 new Status { StatusID = 1, Name = "Incomplete" },
                 new Status { StatusID = 2, Name = "Pending" },
                 new Status { StatusID = 3, Name = "Confirmed" },
                 new Status { StatusID = 4, Name = "Cancelled" },
                 new Status { StatusID = 5, Name = "Checked In" },
                 new Status { StatusID = 6, Name = "Checked Out" }
                 );

            context.Services.AddOrUpdate(
                v => v.ServiceID,
                new Service { ServiceID = 1, Name = "English" },
                new Service { ServiceID = 2, Name = "Mandarian" },
                new Service { ServiceID = 3, Name = "Cantonese" },
                new Service { ServiceID = 4, Name = "T4C West" },
                new Service { ServiceID = 5, Name = "Youth Group" },
                new Service { ServiceID = 6, Name = "Children" },
                new Service { ServiceID = 7, Name = "Nursery" }
                );

            context.AgeRanges.AddOrUpdate(
                a => a.AgeRangeID,
                new AgeRange { AgeRangeID = 1, Name = "0-1 yrs" },
                new AgeRange { AgeRangeID = 2, Name = "1-5 yrs" },
                new AgeRange { AgeRangeID = 3, Name = "6-12 yrs" },
                new AgeRange { AgeRangeID = 4, Name = "Teenager" },
                new AgeRange { AgeRangeID = 5, Name = "Adult" },
                new AgeRange { AgeRangeID = 6, Name = "Senior" }
                );

            context.Genders.AddOrUpdate(
                 a => a.GenderID,
                 new Gender { GenderID = 1, Name = "Male" },
                 new Gender { GenderID = 2, Name = "Female" }
                 );

            context.Fellowships.AddOrUpdate(
                a => a.FellowshipID,
                new Fellowship { FellowshipID = 1, ServiceID = 1, Name = "No Fellowship Selected" },
                new Fellowship { FellowshipID = 2, ServiceID = 2, Name = "No Fellowship Selected" },
                new Fellowship { FellowshipID = 3, ServiceID = 3, Name = "No Fellowship Selected" },
                new Fellowship { FellowshipID = 4, ServiceID = 4, Name = "No Fellowship Selected" },
                new Fellowship { FellowshipID = 5, ServiceID = 5, Name = "No Fellowships" },
                new Fellowship { FellowshipID = 6, ServiceID = 6, Name = "No Fellowships" },
                new Fellowship { FellowshipID = 7, ServiceID = 7, Name = "No Fellowships" }
                );

            context.RegTypes.AddOrUpdate(
                 a => a.Name,
                 new RegType { Name = "Full Time" },
                 new RegType { Name = "Part Time" }
                 );

            context.RegPrices.AddOrUpdate(
                a => a.AgeRangeID,
                new RegPrice { AgeRangeID = 1, PartTimePrice = (decimal)5, FullTimePrice = (decimal)5},
                new RegPrice { AgeRangeID = 2, PartTimePrice = (decimal)45, FullTimePrice = (decimal)25},
                new RegPrice { AgeRangeID = 3, PartTimePrice = (decimal)85, FullTimePrice = (decimal)45},
                new RegPrice { AgeRangeID = 4, PartTimePrice = (decimal)85, FullTimePrice = (decimal)45},
                new RegPrice { AgeRangeID = 5, PartTimePrice = (decimal)85, FullTimePrice = (decimal)45},
                new RegPrice { AgeRangeID = 6, PartTimePrice = (decimal)85, FullTimePrice = (decimal)45}
                );

            context.RoomTypes.AddOrUpdate(
                a => a.RoomTypeID,
                new RoomType { RoomTypeID = 1, RegTypeID = 1, Name = "No Preference"},
                new RoomType { RoomTypeID = 2, RegTypeID = 2, Name = "Part Time - No Room" },
                new RoomType { RoomTypeID = 3, RegTypeID = 1, Name = "Family Room" },
                new RoomType { RoomTypeID = 4, RegTypeID = 1, Name = "Small Children/Senior" },
                new RoomType { RoomTypeID = 5, RegTypeID = 1, Name = "Dormatory" },
                new RoomType { RoomTypeID = 6, RegTypeID = 1, Name = "Handicap Accessible" }
                );


            context.PmtStatuses.AddOrUpdate(
                a => a.PmtStatusID,
                new PmtStatus { PmtStatusID = 1, Name = "Pending" },
                new PmtStatus { PmtStatusID = 2, Name = "Approved" },
                new PmtStatus { PmtStatusID = 3, Name = "Declined" }
                );

            context.PmtTypes.AddOrUpdate(
                a => a.PmtTypeID,
                new PmtType { PmtTypeID = 1, Name = "Scholorship" },
                new PmtType { PmtTypeID = 2, Name = "Cash" },
                new PmtType { PmtTypeID = 3, Name = "Check" },
                new PmtType { PmtTypeID = 4, Name = "Refund" },
                new PmtType { PmtTypeID = 5, Name = "Adjustmet" }

                );



        }
    }
}
