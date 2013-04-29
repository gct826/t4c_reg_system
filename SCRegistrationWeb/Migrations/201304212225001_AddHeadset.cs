namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHeadset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Headsets",
                c => new
                    {
                        HeadsetID = c.Int(nullable: false, identity: true),
                        ParticipantID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HeadsetID)
                .ForeignKey("dbo.ParticipantEntries", t => t.ParticipantID, cascadeDelete: true)
                .Index(t => t.ParticipantID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Headsets", new[] { "ParticipantID" });
            DropForeignKey("dbo.Headsets", "ParticipantID", "dbo.ParticipantEntries");
            DropTable("dbo.Headsets");
        }
    }
}
