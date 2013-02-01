using System;
using System.Data.Entity;

namespace SCRegistrationWeb.Models
{
    public class SCRegistrationContext : DbContext
    {
        public SCRegistrationContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<RegistrationEntry> RegEntries { get; set; }

        //  Participant Attribute Data
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<AgeRange> AgeRanges { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<RegType> RegTypes { get; set; }
        public DbSet<Fellowship> Fellowships { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }

        public DbSet<ParticipantEntry> ParticipantEntries { get; set; }

        public DbSet<RegPrice> RegPrices { get; set; }

        public DbSet<EventHistory> EventHistory { get; set; }

        public DbSet<PaymentEntry> PaymentEntries { get; set; }
        public DbSet<PmtStatus> PmtStatuses { get; set; }
        public DbSet<PmtType> PmtTypes { get; set; }

    }
}
