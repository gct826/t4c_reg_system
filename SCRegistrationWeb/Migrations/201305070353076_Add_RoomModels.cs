namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_RoomModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoomNotes",
                c => new
                    {
                        RoomNoteID = c.Int(nullable: false, identity: true),
                        PartID = c.Int(nullable: false),
                        Note = c.String(),
                        ParticipantEntries_ParticipantID = c.Int(),
                    })
                .PrimaryKey(t => t.RoomNoteID)
                .ForeignKey("dbo.ParticipantEntries", t => t.ParticipantEntries_ParticipantID)
                .Index(t => t.ParticipantEntries_ParticipantID);
            
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        BuildingID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        RoomTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BuildingID)
                .ForeignKey("dbo.RoomTypes", t => t.RoomTypeID, cascadeDelete: true)
                .Index(t => t.RoomTypeID);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        NumOfBeds = c.Int(nullable: false),
                        BuildingID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoomID)
                .ForeignKey("dbo.Buildings", t => t.BuildingID, cascadeDelete: true)
                .Index(t => t.BuildingID);
            
            CreateTable(
                "dbo.RoomAssignments",
                c => new
                    {
                        RoomAssignmentID = c.Int(nullable: false, identity: true),
                        PartID = c.Int(nullable: false),
                        RoomID = c.Int(nullable: false),
                        ParticipantEntries_ParticipantID = c.Int(),
                    })
                .PrimaryKey(t => t.RoomAssignmentID)
                .ForeignKey("dbo.ParticipantEntries", t => t.ParticipantEntries_ParticipantID)
                .ForeignKey("dbo.Rooms", t => t.RoomID, cascadeDelete: true)
                .Index(t => t.ParticipantEntries_ParticipantID)
                .Index(t => t.RoomID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RoomAssignments", new[] { "RoomID" });
            DropIndex("dbo.RoomAssignments", new[] { "ParticipantEntries_ParticipantID" });
            DropIndex("dbo.Rooms", new[] { "BuildingID" });
            DropIndex("dbo.Buildings", new[] { "RoomTypeID" });
            DropIndex("dbo.RoomNotes", new[] { "ParticipantEntries_ParticipantID" });
            DropForeignKey("dbo.RoomAssignments", "RoomID", "dbo.Rooms");
            DropForeignKey("dbo.RoomAssignments", "ParticipantEntries_ParticipantID", "dbo.ParticipantEntries");
            DropForeignKey("dbo.Rooms", "BuildingID", "dbo.Buildings");
            DropForeignKey("dbo.Buildings", "RoomTypeID", "dbo.RoomTypes");
            DropForeignKey("dbo.RoomNotes", "ParticipantEntries_ParticipantID", "dbo.ParticipantEntries");
            DropTable("dbo.RoomAssignments");
            DropTable("dbo.Rooms");
            DropTable("dbo.Buildings");
            DropTable("dbo.RoomNotes");
        }
    }
}
