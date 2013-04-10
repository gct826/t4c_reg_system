namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SmallGroupChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SmallGroups",
                c => new
                    {
                        SmallGroupID = c.Int(nullable: false, identity: true),
                        PartID = c.Int(nullable: false),
                        ServiceID = c.Int(nullable: false),
                        SmallGroupName = c.String(nullable: false),
                        ParticipantEntries_ParticipantID = c.Int(),
                    })
                .PrimaryKey(t => t.SmallGroupID)
                .ForeignKey("dbo.ParticipantEntries", t => t.ParticipantEntries_ParticipantID)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: false)
                .Index(t => t.ParticipantEntries_ParticipantID)
                .Index(t => t.ServiceID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SmallGroups", new[] { "ServiceID" });
            DropIndex("dbo.SmallGroups", new[] { "ParticipantEntries_ParticipantID" });
            DropForeignKey("dbo.SmallGroups", "ServiceID", "dbo.Services");
            DropForeignKey("dbo.SmallGroups", "ParticipantEntries_ParticipantID", "dbo.ParticipantEntries");
            DropTable("dbo.SmallGroups");
        }
    }
}
