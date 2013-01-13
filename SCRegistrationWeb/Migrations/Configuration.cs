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
                 s => s.Name,
                 new Status { Name = "Incomplete" },
                 new Status { Name = "Pending" },
                 new Status { Name = "Confirmed" },
                 new Status { Name = "Cancelled" },
                 new Status { Name = "Checked In" },
                 new Status { Name = "Checked Out" }
                 );

            context.Services.AddOrUpdate(
                v => v.Name,
                new Service { Name = "English" },
                new Service { Name = "Mandarian" },
                new Service { Name = "Cantonese" },
                new Service { Name = "T4C West" },
                new Service { Name = "Youth Group" },
                new Service { Name = "Children" },
                new Service { Name = "Nursery" }
                );

            context.AgeRanges.AddOrUpdate(
                a => a.Name,
                new AgeRange { Name = "0-1 yrs" },
                new AgeRange { Name = "1-5 yrs" },
                new AgeRange { Name = "6-12 yrs" },
                new AgeRange { Name = "Teenager" },
                new AgeRange { Name = "Adult" },
                new AgeRange { Name = "Senior" }
                );

            context.Genders.AddOrUpdate(
                 a => a.Name,
                 new Gender { Name = "Male" },
                 new Gender { Name = "Female" }
                 );

            context.RegTypes.AddOrUpdate(
                 a => a.Name,
                 new RegType { Name = "Full Time" },
                 new RegType { Name = "Part Time" }
                 );

            context.Fellowships.AddOrUpdate(
                a => a.ServiceID,
                new Fellowship { ServiceID = 1, Name = "No Fellowship Selected"},
                new Fellowship { ServiceID = 2, Name = "No Fellowship Selected"},
                new Fellowship { ServiceID = 3, Name = "No Fellowship Selected"},
                new Fellowship { ServiceID = 4, Name = "No Fellowship Selected"},
                new Fellowship { ServiceID = 5, Name = "No Fellowships"},
                new Fellowship { ServiceID = 6, Name = "No Fellowships"},
                new Fellowship { ServiceID = 7, Name = "No Fellowships"}
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
                a => a.Name,
                new RoomType { RegTypeID = 1, Name = "No Preference"},
                new RoomType { RegTypeID = 2, Name = "Part Time - No Room"},
                new RoomType { RegTypeID = 1, Name = "Family Room"},
                new RoomType { RegTypeID = 1, Name = "Small Children/Senior"},
                new RoomType { RegTypeID = 1, Name = "Dormatory"},
                new RoomType { RegTypeID = 1, Name = "Handicap Accessible"}
                );
        }
    }
}
