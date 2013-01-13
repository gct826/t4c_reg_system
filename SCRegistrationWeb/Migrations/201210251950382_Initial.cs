namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegistrationEntries",
                c => new
                    {
                        RegistrationID = c.Int(nullable: false, identity: true),
                        RegistrationUID = c.String(),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RegistrationID);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.StatusID);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ServiceID);
            
            CreateTable(
                "dbo.AgeRanges",
                c => new
                    {
                        AgeRangeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.AgeRangeID);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        GenderID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.GenderID);
            
            CreateTable(
                "dbo.RegTypes",
                c => new
                    {
                        RegTypeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.RegTypeID);
            
            CreateTable(
                "dbo.Fellowships",
                c => new
                    {
                        FellowshipID = c.Int(nullable: false, identity: true),
                        ServiceID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.FellowshipID)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .Index(t => t.ServiceID);
            
            CreateTable(
                "dbo.RoomTypes",
                c => new
                    {
                        RoomTypeID = c.Int(nullable: false, identity: true),
                        RegTypeID = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.RoomTypeID)
                .ForeignKey("dbo.RegTypes", t => t.RegTypeID, cascadeDelete: true)
                .Index(t => t.RegTypeID);
            
            CreateTable(
                "dbo.ParticipantEntries",
                c => new
                    {
                        ParticipantID = c.Int(nullable: false, identity: true),
                        RegistrationID = c.Int(nullable: false),
                        StatusID = c.Int(nullable: false),
                        ServiceID = c.Int(nullable: false),
                        AgeRangeID = c.Int(nullable: false),
                        GenderID = c.Int(nullable: false),
                        RegTypeID = c.Int(nullable: false),
                        FellowshipID = c.Int(nullable: false),
                        RoomTypeID = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        ChineseName = c.String(maxLength: 20),
                        PartPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ParticipantID)
                .ForeignKey("dbo.RegistrationEntries", t => t.RegistrationID, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusID, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .ForeignKey("dbo.AgeRanges", t => t.AgeRangeID, cascadeDelete: true)
                .ForeignKey("dbo.Genders", t => t.GenderID, cascadeDelete: true)
                .ForeignKey("dbo.RegTypes", t => t.RegTypeID, cascadeDelete: true)
                .Index(t => t.RegistrationID)
                .Index(t => t.StatusID)
                .Index(t => t.ServiceID)
                .Index(t => t.AgeRangeID)
                .Index(t => t.GenderID)
                .Index(t => t.RegTypeID)
                .Index(t => t.FellowshipID)
                .Index(t => t.RoomTypeID);
            
            CreateTable(
                "dbo.RegPrices",
                c => new
                    {
                        RegTypeID = c.Int(nullable: false, identity: true),
                        AgeRangeID = c.Int(nullable: false),
                        PartTimePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FullTimePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.RegTypeID)
                .ForeignKey("dbo.AgeRanges", t => t.AgeRangeID, cascadeDelete: true)
                .Index(t => t.AgeRangeID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RegPrices", new[] { "AgeRangeID" });
            DropIndex("dbo.ParticipantEntries", new[] { "RoomTypeID" });
            DropIndex("dbo.ParticipantEntries", new[] { "FellowshipID" });
            DropIndex("dbo.ParticipantEntries", new[] { "RegTypeID" });
            DropIndex("dbo.ParticipantEntries", new[] { "GenderID" });
            DropIndex("dbo.ParticipantEntries", new[] { "AgeRangeID" });
            DropIndex("dbo.ParticipantEntries", new[] { "ServiceID" });
            DropIndex("dbo.ParticipantEntries", new[] { "StatusID" });
            DropIndex("dbo.ParticipantEntries", new[] { "RegistrationID" });
            DropIndex("dbo.RoomTypes", new[] { "RegTypeID" });
            DropIndex("dbo.Fellowships", new[] { "ServiceID" });
            DropForeignKey("dbo.RegPrices", "AgeRangeID", "dbo.AgeRanges");
            DropForeignKey("dbo.ParticipantEntries", "RoomTypeID", "dbo.RoomTypes");
            DropForeignKey("dbo.ParticipantEntries", "FellowshipID", "dbo.Fellowships");
            DropForeignKey("dbo.ParticipantEntries", "RegTypeID", "dbo.RegTypes");
            DropForeignKey("dbo.ParticipantEntries", "GenderID", "dbo.Genders");
            DropForeignKey("dbo.ParticipantEntries", "AgeRangeID", "dbo.AgeRanges");
            DropForeignKey("dbo.ParticipantEntries", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.ParticipantEntries", "StatusID", "dbo.Status");
            DropForeignKey("dbo.ParticipantEntries", "RegistrationID", "dbo.RegistrationEntries");
            DropForeignKey("dbo.RoomTypes", "RegTypeID", "dbo.RegTypes");
            DropForeignKey("dbo.Fellowships", "ServiceID", "dbo.Services");
            DropTable("dbo.RegPrices");
            DropTable("dbo.ParticipantEntries");
            DropTable("dbo.RoomTypes");
            DropTable("dbo.Fellowships");
            DropTable("dbo.RegTypes");
            DropTable("dbo.Genders");
            DropTable("dbo.AgeRanges");
            DropTable("dbo.Services");
            DropTable("dbo.Status");
            DropTable("dbo.RegistrationEntries");
        }
    }
}
